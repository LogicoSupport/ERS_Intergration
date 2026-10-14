using ERS_Intergration.Models;
using Microsoft.EntityFrameworkCore;

namespace ERS_Intergration.DataContext
{
    public class FiscalDbContext : DbContext
    {
        public FiscalDbContext(DbContextOptions<FiscalDbContext> options) : base(options) { }

        public DbSet<FiscalInvoice> FiscalInvoices => Set<FiscalInvoice>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<FiscalInvoice>(e =>
            {
                
                e.HasIndex(x => x.RequestId).IsUnique();
                e.Property(x => x.RequestId).HasMaxLength(32).IsRequired();
                e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
                e.Property(x => x.DocType).HasConversion<string>().HasMaxLength(20);
                e.Property(x => x.InvoiceCounter).HasMaxLength(30);
                e.Property(x => x.SdcInvoiceNumber).HasMaxLength(60);
                e.Property(x => x.TotalAmount).HasPrecision(18, 2);
                e.Property(x => x.TotalTax).HasPrecision(18, 2);
                e.Property(x => x.Version).IsRowVersion();
                e.HasIndex(x => new { x.SourceSystem, x.SourceReference, x.DocType }).IsUnique();
                e.Property(x => x.SourceSystem).HasMaxLength(20).IsRequired();
                e.Property(x => x.SourceReference).HasMaxLength(50).IsRequired();
            });
        }
    }
}
