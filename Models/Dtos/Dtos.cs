using System.Text.Json.Serialization;

namespace ERS_Intergration.Models.Dtos
{
    public class InvoiceRequest
    {
        public string DateAndTimeOfIssue { get; set; } = "";
        public string InvoiceType { get; set; } = "Normal";
        public string TransactionType { get; set; } = "Sale";
        public object InvoiceCategory { get; set; } = 0;   // 0 = plain sale, "B2B" = buyer identified
        public string? DocumentFunction { get; set; }       // later: CreditNote / DebitNote / CancellationInvoice
        public List<PaymentDto> Payment { get; set; } = new();
        public string Cashier { get; set; } = "";
        public string? BuyerId { get; set; }
        public string InvoiceNumber { get; set; } = "";
        public string? ReferentDocumentNumber { get; set; } // later: refunds, notes, cancellations
        public string? ReferentDocumentDT { get; set; }
        public OptionsDto Options { get; set; } = new();
        public List<ItemDto> Items { get; set; } = new();
        public Dictionary<string, string>? InvoiceAdditionalFields { get; set; }
    }
    public class PaymentDto { public decimal Amount { get; set; } public string PaymentType { get; set; } = "Other"; }
    public class OptionsDto { public int OmitTextualRepresentation { get; set; } public int OmitQRCodeGen { get; set; } }

    public class ItemDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)] public string? Gtin { get; set; }
        public string Name { get; set; } = "";
        public decimal Quantity { get; set; }
        public List<string> Labels { get; set; } = new();
        public decimal UnitPriceTaxExcluded { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public Dictionary<string, string> ItemAdditionalFields { get; set; } = new();
    }

    public class FiscaliseSaleRequest
    {
        public required string SourceSystem { get; set; }     // "EMA" | "MACMOBILE"
        public required string SourceReference { get; set; }  // EMA = DocEntry, Macmobile = U_MacmobileInv
        public required string Cashier { get; set; }
        public DateTime InvoiceDate { get; set; }             // full date and time
        public decimal? InvoiceTotal { get; set; }            // optional, incl. VAT, compared to sum of lines
        public List<SalePayment> Payments { get; set; } = new();
        public List<SaleLine> Lines { get; set; } = new();
    }

    public class SalePayment
    {
        public string PaymentType { get; set; } = "Cash";
        public decimal Amount { get; set; }
    }

    public class SaleLine
    {
        public string? ItemCode { get; set; }
        public string? Description { get; set; }
        public string? Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }         // after discount, excl. VAT
        public decimal LineTotal { get; set; }         // excl. VAT
        public decimal LineTotalInclVat { get; set; }  // incl. VAT
        public required string VatGroup { get; set; }
    }
}
