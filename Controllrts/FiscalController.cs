using ERS_Intergration.Models;
using ERS_Intergration.Models.Dtos;
using ERS_Intergration.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERS_Intergration.Controllrts
{
    [ApiController, Route("api/fiscal")]
    [ServiceFilter(typeof(ApiKeyFilter))]
    public class FiscalController : Controller
    {
        private ApiKeyEntry Caller => (ApiKeyEntry)HttpContext.Items[ApiKeyFilter.ItemKey]!;

        private bool CanAct(string? sourceSystem) =>
            Caller.SourceSystem == "*" ||
            string.Equals(Caller.SourceSystem, sourceSystem?.Trim(), StringComparison.OrdinalIgnoreCase);

        private IActionResult WrongSystem(string? requested) =>
            StatusCode(403, new { error = $"This API key is not allowed to act on '{requested}' invoices." });
        private readonly ILogger<FiscalController> _logger;
        private readonly InvoiceMapper _mapper;
        private readonly SdcClient _sdc;
        private readonly FiscalisationService _fiscal;

        public FiscalController(ILogger<FiscalController> logger, InvoiceMapper mapper, SdcClient sdc, FiscalisationService fiscal)
        {
            _logger = logger;
            _mapper = mapper;
            _sdc = sdc;
            _fiscal = fiscal;
        }

        [HttpPost("invoices/{docNum:int}")]
        public async Task<IActionResult> Fiscalise(int docNum)
        {
            if (!CanAct("EMA")) return WrongSystem("EMA");
            try
            {
                var row = await _fiscal.SubmitSaleAsync(docNum);

                return row.Status switch
                {
                    FiscalStatus.Signed => Content(row.ResponseJson!, "application/json"),
                    FiscalStatus.Pending => Conflict(new { row.RequestId, row.ErrorMessage, message = "Awaiting SDC confirmation, not resent." }),
                    _ => StatusCode(502, new { row.RequestId, row.ErrorMessage })
                };
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error fiscalising invoice {DocNum}", docNum);
                return BadRequest(new { error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                // Two requests for the same invoice at once: the unique index / row version stopped the second
                _logger.LogWarning(ex, "Concurrent submit for invoice {DocNum}", docNum);
                return Conflict(new { error = "This invoice is already being processed." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fiscalising invoice {DocNum}", docNum);
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpPost("invoices")]
        public async Task<IActionResult> FiscaliseSale([FromBody] FiscaliseSaleRequest request)
        {
            // GUARD: does this API key's SourceSystem match the invoice's SourceSystem?
            if (!CanAct(request.SourceSystem)) return WrongSystem(request.SourceSystem);
            try
            {
                var row = await _fiscal.SubmitSaleAsync(request);

                return row.Status switch
                {
                    FiscalStatus.Signed => Ok(ToResult(row)),
                    FiscalStatus.Pending => Conflict(new { row.RequestId, row.ErrorMessage, message = "Awaiting SDC confirmation, not resent." }),
                    _ => StatusCode(502, new { row.RequestId, row.ErrorMessage })
                };
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error fiscalising {System}/{Ref}", request.SourceSystem, request.SourceReference);
                return BadRequest(new { error = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Concurrent submit for {System}/{Ref}", request.SourceSystem, request.SourceReference);
                return Conflict(new { error = "This invoice is already being processed." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fiscalising {System}/{Ref}", request.SourceSystem, request.SourceReference);
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpGet("invoices/{sourceSystem}/{sourceReference}")]
        public async Task<IActionResult> GetFiscalised(string sourceSystem, string sourceReference)
        {
            if (!CanAct(sourceSystem)) return WrongSystem(sourceSystem);
            var row = await _fiscal.FindAsync(sourceSystem, sourceReference);
            if (row == null) return NotFound();
            return row.Status == FiscalStatus.Signed
                ? Ok(ToResult(row))
                : Conflict(new { row.RequestId, status = row.Status.ToString(), row.ErrorMessage });
        }

        private static object ToResult(FiscalInvoice row) => new
        {
            row.RequestId,
            row.SourceSystem,
            row.SourceReference,
            sdcInvoiceNumber = row.SdcInvoiceNumber,
            sdcDateTime = row.SdcDateTime,
            invoiceCounter = row.InvoiceCounter,
            verificationUrl = row.VerificationUrl,
            verificationQrCode = row.VerificationQrCode,
            journal = row.Journal,
            totalAmount = row.TotalAmount,
            totalTax = row.TotalTax
        };
    }
}
