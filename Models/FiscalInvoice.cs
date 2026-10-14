namespace ERS_Intergration.Models
{
    public enum FiscalStatus { Pending, Signed, Failed }

    // Only NormalSale is used today; the others avoid a migration later
    public enum FiscalDocType { NormalSale, NormalRefund, Cancellation, DebitNote, CreditNote }

    public class FiscalInvoice
    {
        public long Id { get; set; }

        // SAP side. ObjType 13 = A/R Invoice (OINV), 14 = A/R Credit Memo (ORIN)
        // Source system key (who created the invoice and their reference for it)
        public required string SourceSystem { get; set; }      // "EMA" | "MACMOBILE"
        public required string SourceReference { get; set; }   // EMA = DocEntry, Macmobile = U_MacmobileInv

        // Optional SAP link (null for Macmobile invoices until they reach EMA)
        public int? SapObjType { get; set; }
        public int? SapDocEntry { get; set; }
        public int? SapDocNum { get; set; }
        public FiscalDocType DocType { get; set; }

        // Our side
        public string RequestId { get; set; } = "";
        public FiscalStatus Status { get; set; }
        public int Attempts { get; set; }
        public string? RequestJson { get; set; }
        public string? ResponseJson { get; set; }   // raw SDC response, the source of truth
        public string? ErrorMessage { get; set; }

        // Pulled out of the response for easy querying and printing
        public string? SdcInvoiceNumber { get; set; }   // needed later as referentDocumentNumber
        public DateTimeOffset? SdcDateTime { get; set; }
        public string? InvoiceCounter { get; set; }
        public string? VerificationUrl { get; set; }
        public string? VerificationQrCode { get; set; } // base64 GIF
        public string? Journal { get; set; }            // ready-made receipt text
        public decimal? TotalAmount { get; set; }
        public decimal? TotalTax { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public uint Version { get; set; }
    }
}
