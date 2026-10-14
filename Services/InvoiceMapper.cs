using ERS_Intergration.DataContext;
using ERS_Intergration.Models.Dtos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace ERS_Intergration.Services
{
    public class InvoiceMapper
    {
        private readonly Dictionary<string, string> _labels;
        private static decimal R(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
        private readonly SapDbContext _db;
        
        public InvoiceMapper(IConfiguration cfg, SapDbContext dbContext) {
            
            _labels = cfg.GetSection("TaxLabels").Get<Dictionary<string, string>>() ?? new();
            _db = dbContext;
        }
       // for now we will make it to accept an invoice number but for production this needs to be automatic

        public async Task<InvoiceRequest> MapSales(int docNum)
        {
            //for now cashier will be a string we pass but we will have to look at who invoked that invoice
            if (docNum <= 0)
            {
                throw new InvalidOperationException("Invoice number required");
            }
            var invoice = await _db.Oinv.FirstOrDefaultAsync( x => x.DocNum== docNum);

            if (invoice == null)
            {
                throw new InvalidOperationException(
                    $"Invoice with Docnum '{docNum}' was not found in SAP."
                );
            }

            var lines = await _db.Inv1
                .Where(l => l.DocEntry == invoice.DocEntry)
                .ToListAsync();

            if (invoice.Canceled == "Y")
            {
                throw new InvalidOperationException(
                    "Canceled invoices cannot be processed. Invoice is cancelled in SAP."
                );
            }

            if (lines.Count == 0)
            {
                throw new InvalidOperationException("Invoice has no lines");
            }
            var taxId = invoice.LicTradNum?.Trim();
            var b2b = !string.IsNullOrEmpty(taxId);

            var req = new InvoiceRequest
            {
                // Using "now": which date goes on a fiscal invoice for an older SAP invoice is a question for ERS
                DateAndTimeOfIssue = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                InvoiceNumber = invoice.DocNum.ToString(),
                Cashier = "Test Cashier"
            };
            foreach (var l in lines)
            {
                var qty = Convert.ToDecimal(l.Quantity);
                var price = Convert.ToDecimal(l.Price);        // after line discount (PriceBefDi is before)
                var net = Convert.ToDecimal(l.LineTotal);      // excl. VAT
                var gross = Convert.ToDecimal(l.GTotal);       // incl. VAT
                if (qty <= 0) throw new InvalidOperationException($"Line {l.LineNum}: quantity must be positive.");
                if (Math.Abs(price * qty - net) > 0.02m)
                    throw new InvalidOperationException($"Line {l.LineNum}: price x quantity does not match line total.");

                var vg = l.VatGroup?.Trim() ?? "";
                if (!_labels.TryGetValue(vg, out var label))
                    throw new InvalidOperationException($"No SDC tax label configured for SAP tax code '{vg}' (line {l.LineNum}).");

                req.Items.Add(new ItemDto
                {
                    Name = l.Dscription ?? l.ItemCode ?? "Item",
                    Quantity = qty,
                    Labels = { label },
                    UnitPriceTaxExcluded = price,
                    TaxableAmount = R(net),
                    UnitPrice = R(gross / qty),
                    TotalAmount = R(gross),
                    ItemAdditionalFields = { ["unit"] = l.UnitMsr ?? "unit", ["itemCode"] = l.ItemCode ?? "" }
                });
            }

            var total = req.Items.Sum(i => i.TotalAmount);
            if (Math.Abs(total - Convert.ToDecimal(invoice.DocTotal)) > 0.05m)
                throw new InvalidOperationException($"Line totals ({total}) differ from SAP document total ({invoice.DocTotal}): header discount or rounding?");

            // Placeholder until ERS confirms the payment type for credit sales
            req.Payment.Add(new PaymentDto { Amount = total, PaymentType = "Other" });

            if (b2b)
            {
                req.InvoiceCategory = "B2B";
                req.BuyerId = taxId;
                req.InvoiceAdditionalFields = new()
                {
                    ["buyerName"] = invoice.CardName ?? "",
                    ["buyerAddress"] = (invoice.Address ?? "").Replace("\r\n", ", ").Replace("\n", ", ")
                };
            }
            return req;
        }

        private static readonly HashSet<string> PaymentTypes = new(StringComparer.OrdinalIgnoreCase)
    { "Cash", "Card", "Check", "WireTransfer", "Voucher", "MobileMoney", "Other" };
        //please not that The SDC needs the VAT-inclusive total per line, and the mapper doesn’t know the rates (the SDC does).
        //So the caller must send LineTotalInclVat. Both Macmobile and EMA already have it.
        public InvoiceRequest MapSales(FiscaliseSaleRequest s)
        {
            if (s.Lines.Count == 0) throw new InvalidOperationException("Invoice has no lines");
            if (s.Payments.Count == 0) throw new InvalidOperationException("At least one payment is required");
            if (string.IsNullOrWhiteSpace(s.Cashier)) throw new InvalidOperationException("Cashier is required");

            var req = new InvoiceRequest
            {
                DateAndTimeOfIssue = s.InvoiceDate.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                InvoiceNumber = s.SourceReference,
                Cashier = s.Cashier
            };

            var n = 0;
            foreach (var l in s.Lines)
            {
                n++;
                if (l.Quantity <= 0) throw new InvalidOperationException($"Line {n}: quantity must be positive.");
                if (Math.Abs(l.UnitPrice * l.Quantity - l.LineTotal) > 0.02m)
                    throw new InvalidOperationException($"Line {n}: price x quantity does not match line total.");
                if (l.LineTotalInclVat < l.LineTotal)
                    throw new InvalidOperationException($"Line {n}: total incl. VAT is less than total excl. VAT.");

                var vg = l.VatGroup?.Trim() ?? "";
                if (!_labels.TryGetValue(vg, out var label))
                    throw new InvalidOperationException($"No SDC tax label configured for VAT group '{vg}' (line {n}).");

                req.Items.Add(new ItemDto
                {
                    Name = l.Description ?? l.ItemCode ?? "Item",
                    Quantity = l.Quantity,
                    Labels = { label },
                    UnitPriceTaxExcluded = l.UnitPrice,
                    TaxableAmount = R(l.LineTotal),
                    UnitPrice = R(l.LineTotalInclVat / l.Quantity),
                    TotalAmount = R(l.LineTotalInclVat),
                    ItemAdditionalFields = { ["unit"] = l.Unit ?? "unit", ["itemCode"] = l.ItemCode ?? "" }
                });
            }

            var total = req.Items.Sum(i => i.TotalAmount);
            if (s.InvoiceTotal.HasValue && Math.Abs(total - s.InvoiceTotal.Value) > 0.05m)
                throw new InvalidOperationException($"Line totals ({total}) differ from invoice total ({s.InvoiceTotal}).");

            foreach (var p in s.Payments)
            {
                if (!PaymentTypes.Contains(p.PaymentType))
                    throw new InvalidOperationException($"Unknown payment type '{p.PaymentType}'.");
                req.Payment.Add(new PaymentDto { Amount = R(p.Amount), PaymentType = p.PaymentType });
            }

            var paid = req.Payment.Sum(p => p.Amount);
            if (Math.Abs(paid - total) > 0.05m)
                throw new InvalidOperationException($"Payments ({paid}) do not match invoice total ({total}).");

            return req;
        }

    }
}
