using ERS_Intergration.DataContext;
using ERS_Intergration.Models;
using ERS_Intergration.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static ERS_Intergration.Services.SdcClient;

namespace ERS_Intergration.Services
{
    public class FiscalisationService
    {
        private const int ArInvoice = 13;
        private readonly InvoiceMapper _mapper;
        private readonly SdcClient _sdc;
        private readonly FiscalDbContext _db;
        private readonly SapDbContext _sap;
        private static readonly SemaphoreSlim SdcLock = new(1, 1);
        private static string BuildRequestId(string system, string reference)
        {
            var prefix = system == "MACMOBILE" ? "MM-" : "EMA-";
            var id = prefix + reference;
            if (id.Length <= 32) return id;
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(system + ":" + reference)));
            return prefix + hash[..(32 - prefix.Length)];
        }

        private static void ApplyResponse(FiscalInvoice row, string result)
        {
            row.ResponseJson = result;

            using var doc = JsonDocument.Parse(result);
            var root = doc.RootElement;

            if (!root.TryGetProperty("invoiceNumber", out var num) || string.IsNullOrEmpty(num.GetString()))
            {
                row.Status = FiscalStatus.Failed;
                row.ErrorMessage = "SDC response had no invoiceNumber (not signed).";
                return;
            }

            row.Status = FiscalStatus.Signed;
            row.SdcInvoiceNumber = num.GetString();
            row.SdcDateTime = root.GetProperty("sdcDateTime").GetDateTimeOffset().ToUniversalTime();
            row.InvoiceCounter = root.GetProperty("invoiceCounter").GetString();
            row.VerificationUrl = root.GetProperty("verificationUrl").GetString();
            row.VerificationQrCode = root.GetProperty("verificationQRCode").GetString();
            row.Journal = root.GetProperty("journal").GetString();
            row.TotalAmount = root.GetProperty("totalAmount").GetDecimal();
            row.TotalTax = root.GetProperty("taxItems").EnumerateArray()
                              .Sum(t => t.GetProperty("amount").GetDecimal());
        }

        private async Task SendAsync(FiscalInvoice row, InvoiceRequest req)
        {
            await SdcLock.WaitAsync();
            try
            {
                try
                {
                    var result = await _sdc.CreateInvoiceAsync(req, row.RequestId);
                    ApplyResponse(row, result);
                }
                catch (TaskCanceledException)
                {
                    row.ErrorMessage = "Timed out waiting for the SDC. Verify with GET v4/invoices/" + row.RequestId + " before resending.";
                }
                catch (Exception ex)
                {
                    row.Status = FiscalStatus.Failed;
                    row.ErrorMessage = ex.Message;
                }

                row.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
            finally
            {
                SdcLock.Release();
            }
        }
        public FiscalisationService(InvoiceMapper mapper, SdcClient sdc, FiscalDbContext db,SapDbContext sap)
        {
            _mapper = mapper; _sdc = sdc; _db = db; _sap = sap; 
        }

        public async Task<FiscalInvoice> SubmitSaleAsync(int docNum)
        {
            var matches = await _sap.Oinv.AsNoTracking()
                .Where(x => x.DocNum == docNum)
                .Select(x => x.DocEntry)
                .Take(2)
                .ToListAsync();

            if (matches.Count == 0)
                throw new InvalidOperationException($"Invoice with DocNum '{docNum}' was not found in SAP.");
            if (matches.Count > 1)
                throw new InvalidOperationException($"DocNum '{docNum}' matches more than one invoice (different series?). Not safe to fiscalise.");

            var docEntry = matches[0];

            var sourceRef = docEntry.ToString();

            var row = await _db.FiscalInvoices.FirstOrDefaultAsync(f =>
                        f.SourceSystem == "EMA" && f.SourceReference == sourceRef && f.DocType == FiscalDocType.NormalSale);

            // Already fiscalised: hand back the stored result, never resend
            if (row?.Status == FiscalStatus.Signed) return row;

            // Sent earlier but no answer: the SDC may have signed it, so don't resend blindly
            if (row?.Status == FiscalStatus.Pending) return row;

            // Throws on bad SAP data before any row is created
            var req = await _mapper.MapSales(docNum);

            if (row == null)
            {
                row = new FiscalInvoice
                {
                    SourceSystem = "EMA",
                    SourceReference = docEntry.ToString(),
                    SapObjType = ArInvoice,
                    SapDocEntry = docEntry,
                    DocType = FiscalDocType.NormalSale,
                    RequestId = $"INV-{docEntry}"
                };
                _db.FiscalInvoices.Add(row);
            }
            row.SapDocNum = docNum;
            row.Status = FiscalStatus.Pending;
            row.Attempts++;
            row.RequestJson = JsonSerializer.Serialize(req);
            row.ErrorMessage = null;
            row.UpdatedAt = DateTime.UtcNow;
            // Unique index / RowVersion stop a parallel double-submit here
            await _db.SaveChangesAsync();

            await SendAsync(row, req);
            return row;
        }

        public async Task<FiscalInvoice> SubmitSaleAsync(FiscaliseSaleRequest s)
        {
            var system = (s.SourceSystem ?? "").Trim().ToUpperInvariant();
            var reference = (s.SourceReference ?? "").Trim();
            if (system != "EMA" && system != "MACMOBILE")
                throw new InvalidOperationException("SourceSystem must be EMA or MACMOBILE.");
            if (reference.Length == 0)
                throw new InvalidOperationException("SourceReference is required.");

            var row = await _db.FiscalInvoices.FirstOrDefaultAsync(f =>
                f.SourceSystem == system && f.SourceReference == reference && f.DocType == FiscalDocType.NormalSale);

            // Same rules as before: never resend a signed or pending invoice
            if (row?.Status == FiscalStatus.Signed) return row;
            if (row?.Status == FiscalStatus.Pending) return row;

            var req = _mapper.MapSales(s);   // throws on bad data before any row is created

            if (row == null)
            {
                row = new FiscalInvoice
                {
                    SourceSystem = system,
                    SourceReference = reference,
                    DocType = FiscalDocType.NormalSale,
                    RequestId = BuildRequestId(system, reference)
                };
                _db.FiscalInvoices.Add(row);
            }
            row.Status = FiscalStatus.Pending;
            row.Attempts++;
            row.RequestJson = JsonSerializer.Serialize(req);
            row.ErrorMessage = null;
            row.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await SendAsync(row, req);
            return row;
        }
        public Task<FiscalInvoice?> FindAsync(string sourceSystem, string sourceReference) =>
    _db.FiscalInvoices.AsNoTracking().FirstOrDefaultAsync(f =>
        f.SourceSystem == sourceSystem.ToUpper() && f.SourceReference == sourceReference
        && f.DocType == FiscalDocType.NormalSale);

        public async Task<int> RecoverPendingAsync(TimeSpan minAge, CancellationToken ct)
        {
            // minAge keeps us away from invoices that are in flight right now
            var cutoff = DateTime.UtcNow - minAge;
            var stuck = await _db.FiscalInvoices
                .Where(f => f.Status == FiscalStatus.Pending && f.UpdatedAt < cutoff)
                .OrderBy(f => f.UpdatedAt)
                .Take(50)
                .ToListAsync(ct);

            var resolved = 0;
            foreach (var row in stuck)
            {
                await SdcLock.WaitAsync(ct);
                try
                {
                    var (status, body) = await _sdc.LookupAsync(row.RequestId);

                    if (status == SdcLookup.Unavailable) continue;   // try again next sweep

                    if (status == SdcLookup.Found)
                    {
                        try { ApplyResponse(row, body!); }
                        catch (Exception ex)
                        {
                            // Keep Pending: the SDC may have signed it, a human must look
                            row.ErrorMessage = "Recovery could not read SDC response: " + ex.Message;
                            row.ResponseJson = body;
                        }
                    }
                    else // NotFound: the SDC never received it
                    {
                        row.Status = FiscalStatus.Failed;
                        row.ErrorMessage = "Recovered: SDC has no record of this RequestId. Safe to resend.";
                    }

                    row.UpdatedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync(ct);
                    resolved++;
                }
                catch (DbUpdateConcurrencyException)
                {
                    _db.Entry(row).State = EntityState.Detached; // someone else touched it; next sweep
                }
                finally
                {
                    SdcLock.Release();
                }
            }
            return resolved;
        }
    }
}
