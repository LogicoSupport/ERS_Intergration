using System;
using System.Collections.Generic;
using ERS_Intergration.Models;
using Microsoft.EntityFrameworkCore;

namespace ERS_Intergration.DataContext;

public partial class SapDbContext : DbContext
{
    public SapDbContext(DbContextOptions<SapDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Inv1> Inv1 { get; set; }

    public virtual DbSet<Oinv> Oinv { get; set; }

    public virtual DbSet<Ousr> Ousrs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP850_CI_AS");

        modelBuilder.Entity<Inv1>(entity =>
        {
            entity.HasKey(e => new { e.DocEntry, e.LineNum }).HasName("INV1_PRIMARY");

            entity.ToTable("INV1");

            entity.HasIndex(e => e.AcctCode, "INV1_ACCOUNT");

            entity.HasIndex(e => new { e.BaseEntry, e.BaseType, e.BaseLine }, "INV1_BASE_ENTRY");

            entity.HasIndex(e => e.Currency, "INV1_CURRENCY");

            entity.HasIndex(e => new { e.ItemCode, e.WhsCode, e.OpenQty }, "INV1_ITM_WHS_OQ");

            entity.HasIndex(e => e.OwnerCode, "INV1_OWNER_CODE");

            entity.HasIndex(e => e.LineStatus, "INV1_STATUS");

            entity.HasIndex(e => new { e.DocEntry, e.VisOrder }, "INV1_VIS_ORDER");

            entity.Property(e => e.AcctCode).HasMaxLength(15);
            entity.Property(e => e.ActDelDate).HasColumnType("datetime");
            entity.Property(e => e.Address).HasMaxLength(254);
            entity.Property(e => e.AllocBinC).HasMaxLength(11);
            entity.Property(e => e.AssblValue).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BackOrdr)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BaseAtCard).HasMaxLength(200);
            entity.Property(e => e.BaseCard).HasMaxLength(15);
            entity.Property(e => e.BaseOpnQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BasePrice)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("E", "DF_INV1_BasePrice");
            entity.Property(e => e.BaseQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BaseRef).HasMaxLength(16);
            entity.Property(e => e.BaseType).HasDefaultValue(-1, "DF_INV1_BaseType");
            entity.Property(e => e.BlockNum).HasMaxLength(100);
            entity.Property(e => e.Ceecflag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("S", "DF_INV1_CEECFlag")
                .HasColumnName("CEECFlag");
            entity.Property(e => e.Cestcode).HasColumnName("CESTCode");
            entity.Property(e => e.Cfopcode)
                .HasMaxLength(6)
                .HasColumnName("CFOPCode");
            entity.Property(e => e.ChgAsmBoMw)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ChgAsmBoMW");
            entity.Property(e => e.CiOppLineN).HasDefaultValue(-1, "DF_INV1_CiOppLineN");
            entity.Property(e => e.Cig).HasColumnName("CIG");
            entity.Property(e => e.Cnjpman)
                .HasMaxLength(14)
                .HasColumnName("CNJPMan");
            entity.Property(e => e.CodeBars).HasMaxLength(254);
            entity.Property(e => e.CogsAcct).HasMaxLength(15);
            entity.Property(e => e.CogsOcrCo2).HasMaxLength(8);
            entity.Property(e => e.CogsOcrCo3).HasMaxLength(8);
            entity.Property(e => e.CogsOcrCo4).HasMaxLength(8);
            entity.Property(e => e.CogsOcrCo5).HasMaxLength(8);
            entity.Property(e => e.CogsOcrCod).HasMaxLength(8);
            entity.Property(e => e.Commission).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ConsumeFct)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ConsumeFCT");
            entity.Property(e => e.CountryOrg).HasMaxLength(3);
            entity.Property(e => e.CredOrigin)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Cstcode)
                .HasMaxLength(6)
                .HasColumnName("CSTCode");
            entity.Property(e => e.CstfCofins)
                .HasMaxLength(2)
                .HasColumnName("CSTfCOFINS");
            entity.Property(e => e.CstfIpi)
                .HasMaxLength(2)
                .HasColumnName("CSTfIPI");
            entity.Property(e => e.CstfPis)
                .HasMaxLength(2)
                .HasColumnName("CSTfPIS");
            entity.Property(e => e.CtrSealQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Cup).HasColumnName("CUP");
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Cusplit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_CUSplit")
                .HasColumnName("CUSplit");
            entity.Property(e => e.DedVatSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DedVatSumF).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DedVatSumS).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DefBreak).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DeferrTax)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_DeferrTax");
            entity.Property(e => e.DelivrdQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DescOw)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_DescOW")
                .HasColumnName("DescOW");
            entity.Property(e => e.DetailsOw)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_DetailsOW")
                .HasColumnName("DetailsOW");
            entity.Property(e => e.Diotnat)
                .HasMaxLength(3)
                .HasColumnName("DIOTNat");
            entity.Property(e => e.DiscPrcnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DistribExp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DistribIs)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_DistribIS")
                .HasColumnName("DistribIS");
            entity.Property(e => e.DistribSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DocDate).HasColumnType("datetime");
            entity.Property(e => e.DropShip)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_DropShip");
            entity.Property(e => e.Dscription).HasMaxLength(200);
            entity.Property(e => e.DstrbSumFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("DstrbSumFC");
            entity.Property(e => e.DstrbSumSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("DstrbSumSC");
            entity.Property(e => e.EnSetCost)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_EnSetCost");
            entity.Property(e => e.EncryptIv)
                .HasMaxLength(100)
                .HasColumnName("EncryptIV");
            entity.Property(e => e.EquVatPer).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.EquVatSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.EquVatSumF).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.EquVatSumS).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExLineNo).HasMaxLength(10);
            entity.Property(e => e.Excisable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ExciseAmt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExpOpType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ExpType).HasMaxLength(4);
            entity.Property(e => e.ExpUuid)
                .HasMaxLength(50)
                .HasColumnName("ExpUUID");
            entity.Property(e => e.ExtTaxRate).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExtTaxSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExtTaxSumF).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExtTaxSumS).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Factor1).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Factor2).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Factor3).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Factor4).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.FisrtBin).HasMaxLength(228);
            entity.Property(e => e.Flags).HasDefaultValue(0, "DF_INV1_Flags");
            entity.Property(e => e.FreeChrgBp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_FreeChrgBP")
                .HasColumnName("FreeChrgBP");
            entity.Property(e => e.FreeTxt).HasMaxLength(100);
            entity.Property(e => e.FromWhsCod).HasMaxLength(8);
            entity.Property(e => e.GpbefDisc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("GPBefDisc");
            entity.Property(e => e.GpttlBasPr)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("GPTtlBasPr");
            entity.Property(e => e.GrossBuyPr).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.GrssProfFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("GrssProfFC");
            entity.Property(e => e.GrssProfSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("GrssProfSC");
            entity.Property(e => e.GrssProfit).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.GTotal)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("GTotal");
            entity.Property(e => e.GtotalFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("GTotalFC");
            entity.Property(e => e.GtotalSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("GTotalSC");
            entity.Property(e => e.Height1).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Height2).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ImportLog).HasMaxLength(20);
            entity.Property(e => e.IndEscala)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_IndEscala");
            entity.Property(e => e.Inmprice)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("INMPrice");
            entity.Property(e => e.InvQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.InvQtyOnly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_InvQtyOnly");
            entity.Property(e => e.InvntSttus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("O", "DF_INV1_InvntSttus");
            entity.Property(e => e.IsAqcuistn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_IsAqcuistn");
            entity.Property(e => e.IsByPrdct)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_IsByPrdct");
            entity.Property(e => e.IsCstmAct)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_IsCstmAct");
            entity.Property(e => e.IsPrscGood)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_IsPrscGood");
            entity.Property(e => e.IsSrvCall)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_isSrvCall")
                .HasColumnName("isSrvCall");
            entity.Property(e => e.Isdistrb)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ISDistrb");
            entity.Property(e => e.IsdistrbFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ISDistrbFC");
            entity.Property(e => e.IsdistrbSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ISDistrbSC");
            entity.Property(e => e.IsdtCryImp)
                .HasMaxLength(3)
                .HasColumnName("ISDtCryImp");
            entity.Property(e => e.IsdtRgnImp).HasColumnName("ISDtRgnImp");
            entity.Property(e => e.IsorCryExp)
                .HasMaxLength(3)
                .HasColumnName("ISOrCryExp");
            entity.Property(e => e.IsorRgnExp).HasColumnName("ISOrRgnExp");
            entity.Property(e => e.ItemCode).HasMaxLength(50);
            entity.Property(e => e.ItemType).HasDefaultValue(4, "DF_INV1_ItemType");
            entity.Property(e => e.ItmTaxType).HasMaxLength(2);
            entity.Property(e => e.LegalTcd)
                .HasMaxLength(250)
                .HasColumnName("LegalTCD");
            entity.Property(e => e.LegalText).HasMaxLength(254);
            entity.Property(e => e.LegalTimd)
                .HasMaxLength(250)
                .HasColumnName("LegalTIMD");
            entity.Property(e => e.LegalTtca)
                .HasMaxLength(250)
                .HasColumnName("LegalTTCA");
            entity.Property(e => e.LegalTw)
                .HasMaxLength(250)
                .HasColumnName("LegalTW");
            entity.Property(e => e.Length1).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Length2)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("length2");
            entity.Property(e => e.LicTradNum).HasMaxLength(32);
            entity.Property(e => e.LinManClsd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_LinManClsd");
            entity.Property(e => e.LinePoPrss)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_LinePoPrss");
            entity.Property(e => e.LineStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("O", "DF_INV1_LineStatus");
            entity.Property(e => e.LineTotal).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.LineType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("R", "DF_INV1_LineType");
            entity.Property(e => e.LineVat).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.LineVatS).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.LineVatlF).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.LineVendor).HasMaxLength(15);
            entity.Property(e => e.LnExcised)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LogInstanc).HasDefaultValue(0, "DF_INV1_LogInstanc");
            entity.Property(e => e.LstBinmpr)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("LstBINMPr");
            entity.Property(e => e.LstByDsFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.LstByDsSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.LstByDsSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Myftype)
                .HasMaxLength(2)
                .HasColumnName("MYFtype");
            entity.Property(e => e.Ncmcode)
                .HasDefaultValue(-1, "DF_INV1_NCMCode")
                .HasColumnName("NCMCode");
            entity.Property(e => e.NeedQty)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_NeedQty");
            entity.Property(e => e.NoInvtryMv)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_NoInvtryMv");
            entity.Property(e => e.NonDdAmt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.NonDdAmtFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NonDdAmtFC");
            entity.Property(e => e.NonDdAmtSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NonDdAmtSC");
            entity.Property(e => e.NumPerMsr).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.NumPerMsr2).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Nvecode)
                .HasMaxLength(6)
                .HasColumnName("NVECode");
            entity.Property(e => e.ObjType)
                .HasMaxLength(20)
                .HasDefaultValue("13", "DF_INV1_ObjType");
            entity.Property(e => e.OcrCode).HasMaxLength(8);
            entity.Property(e => e.OcrCode2).HasMaxLength(8);
            entity.Property(e => e.OcrCode3).HasMaxLength(8);
            entity.Property(e => e.OcrCode4).HasMaxLength(8);
            entity.Property(e => e.OcrCode5).HasMaxLength(8);
            entity.Property(e => e.OpenCreQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.OpenInvQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.OpenQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.OpenRtnQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.OpenSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.OpenSumFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("OpenSumFC");
            entity.Property(e => e.OpenSumSys).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.OrderedQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.OriBabsEnt).HasColumnName("OriBAbsEnt");
            entity.Property(e => e.OriBdocTyp).HasColumnName("OriBDocTyp");
            entity.Property(e => e.OriBlinNum).HasColumnName("OriBLinNum");
            entity.Property(e => e.OrigItem).HasMaxLength(50);
            entity.Property(e => e.PackQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PartRetire)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_PartRetire");
            entity.Property(e => e.PcDocType).HasDefaultValue(-1, "DF_INV1_PcDocType");
            entity.Property(e => e.PcQuantity).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PickOty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PickStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_PickStatus");
            entity.Property(e => e.PlPaWght).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PoNum).HasMaxLength(20);
            entity.Property(e => e.PoTrgEntry).HasMaxLength(11);
            entity.Property(e => e.PostTax)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_INV1_PostTax");
            entity.Property(e => e.PptaxExRe)
                .HasMaxLength(2)
                .HasColumnName("PPTaxExRe");
            entity.Property(e => e.PqtreqDate)
                .HasColumnType("datetime")
                .HasColumnName("PQTReqDate");
            entity.Property(e => e.PqtreqQty)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("PQTReqQty");
            entity.Property(e => e.Price).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PriceAfVat)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("PriceAfVAT");
            entity.Property(e => e.PriceBefDi).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PriceEdit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_PriceEdit");
            entity.Property(e => e.Project).HasMaxLength(20);
            entity.Property(e => e.QtyToShip).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Quantity).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Rate).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.RecogAmt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.RecogAmtFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("RecogAmtFC");
            entity.Property(e => e.RecogAmtSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("RecogAmtSC");
            entity.Property(e => e.RecogVatGr).HasMaxLength(8);
            entity.Property(e => e.RecogVatPr).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ReleasQtty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.RetCost).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.RetirApcfc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("RetirAPCFC");
            entity.Property(e => e.RetirApcsc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("RetirAPCSC");
            entity.Property(e => e.RetireApc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("RetireAPC");
            entity.Property(e => e.RetireQty).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ReturnAct).HasDefaultValue((short)-1, "DF_INV1_ReturnAct");
            entity.Property(e => e.ReturnRsn).HasDefaultValue((short)-1, "DF_INV1_ReturnRsn");
            entity.Property(e => e.RevCharge)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_RevCharge");
            entity.Property(e => e.Rg23apart1).HasColumnName("RG23APart1");
            entity.Property(e => e.Rg23apart2).HasColumnName("RG23APart2");
            entity.Property(e => e.Rg23cpart1).HasColumnName("RG23CPart1");
            entity.Property(e => e.Rg23cpart2).HasColumnName("RG23CPart2");
            entity.Property(e => e.SerialNum).HasMaxLength(17);
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
            entity.Property(e => e.ShipFromCo).HasMaxLength(50);
            entity.Property(e => e.ShipFromDe).HasMaxLength(254);
            entity.Property(e => e.ShipToCode).HasMaxLength(50);
            entity.Property(e => e.ShipToDesc).HasMaxLength(254);
            entity.Property(e => e.Shortages).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.SlpCode).HasDefaultValue(-1, "DF_INV1_SlpCode");
            entity.Property(e => e.SpecPrice)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_SpecPrice");
            entity.Property(e => e.StckAppD).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StckAppDfc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("StckAppDFC");
            entity.Property(e => e.StckAppDsc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("StckAppDSC");
            entity.Property(e => e.StckAppFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StckAppSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StckDstFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StckDstSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StckDstSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StckInmpr)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("StckINMPr");
            entity.Property(e => e.StckSumApp).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StgDesc).HasMaxLength(100);
            entity.Property(e => e.StockPrice).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StockSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StockSumFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StockSumSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.StockValue).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.SubCatNum).HasMaxLength(50);
            entity.Property(e => e.Surpluses).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Sww)
                .HasMaxLength(16)
                .HasColumnName("SWW");
            entity.Property(e => e.TargetType).HasDefaultValue(-1, "DF_INV1_TargetType");
            entity.Property(e => e.TaxAmtSrc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("S", "DF_INV1_TaxAmtSrc");
            entity.Property(e => e.TaxCode).HasMaxLength(8);
            entity.Property(e => e.TaxDistSfc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("TaxDistSFC");
            entity.Property(e => e.TaxDistSsc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("TaxDistSSC");
            entity.Property(e => e.TaxDistSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TaxOnly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TaxPerUnit).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TaxRelev)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_INV1_TaxRelev");
            entity.Property(e => e.TaxStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TaxType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Text).HasColumnType("ntext");
            entity.Property(e => e.ThirdParty)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_ThirdParty");
            entity.Property(e => e.ToDiff).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ToStock).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TotInclTax).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TotalFrgn).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TotalSumSy).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TranType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TreeType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_TreeType");
            entity.Property(e => e.TrnsCode).HasDefaultValue((short)-1, "DF_INV1_TrnsCode");
            entity.Property(e => e.UAllocated)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_Allocated");
            entity.Property(e => e.UAllocatedPerc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_AllocatedPerc");
            entity.Property(e => e.UAllocationNumber)
                .HasMaxLength(30)
                .HasColumnName("U_AllocationNumber");
            entity.Property(e => e.UCaged)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_Caged");
            entity.Property(e => e.UCagedPerc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_CagedPerc");
            entity.Property(e => e.UCampaign)
                .HasMaxLength(100)
                .HasColumnName("U_Campaign");
            entity.Property(e => e.UCampaignDiscount)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_CampaignDiscount");
            entity.Property(e => e.UCountedStock)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_CountedStock");
            entity.Property(e => e.UCustomerPrice)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_CustomerPrice");
            entity.Property(e => e.UDamagedStock)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_DamagedStock");
            entity.Property(e => e.UDistributionProgress)
                .HasMaxLength(30)
                .HasColumnName("U_DistributionProgress");
            entity.Property(e => e.UDistributionStatus)
                .HasMaxLength(30)
                .HasColumnName("U_DistributionStatus");
            entity.Property(e => e.UExpiredStock)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_ExpiredStock");
            entity.Property(e => e.ULoadSheetNumber)
                .HasMaxLength(30)
                .HasColumnName("U_LoadSheetNumber");
            entity.Property(e => e.ULoaded)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_Loaded");
            entity.Property(e => e.UOrderFillPerc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_OrderFillPerc");
            entity.Property(e => e.UPickInstructionNumber)
                .HasMaxLength(30)
                .HasColumnName("U_PickInstructionNumber");
            entity.Property(e => e.UPicked)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_Picked");
            entity.Property(e => e.UPickedPerc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_PickedPerc");
            entity.Property(e => e.UQuantity)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_Quantity");
            entity.Property(e => e.UReasonCode)
                .HasMaxLength(30)
                .HasColumnName("U_ReasonCode");
            entity.Property(e => e.UServiceItemCode)
                .HasMaxLength(30)
                .HasColumnName("U_ServiceItemCode");
            entity.Property(e => e.UShipmentNumber)
                .HasMaxLength(30)
                .HasColumnName("U_ShipmentNumber");
            entity.Property(e => e.UShortStock)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_ShortStock");
            entity.Property(e => e.UStaged)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_Staged");
            entity.Property(e => e.UVendorRebate)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_VendorRebate");
            entity.Property(e => e.UffiscBene)
                .HasMaxLength(10)
                .HasColumnName("UFFiscBene");
            entity.Property(e => e.UnitMsr)
                .HasMaxLength(100)
                .HasColumnName("unitMsr");
            entity.Property(e => e.UnitMsr2)
                .HasMaxLength(100)
                .HasColumnName("unitMsr2");
            entity.Property(e => e.UomCode).HasMaxLength(20);
            entity.Property(e => e.UomCode2).HasMaxLength(20);
            entity.Property(e => e.UomEntry).HasDefaultValue(0, "DF_INV1_UomEntry");
            entity.Property(e => e.UomEntry2).HasDefaultValue(0, "DF_INV1_UomEntry2");
            entity.Property(e => e.UpdInvntry)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_INV1_UpdInvntry");
            entity.Property(e => e.UseBaseUn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_UseBaseUn");
            entity.Property(e => e.VatAppld).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatAppldFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("VatAppldFC");
            entity.Property(e => e.VatAppldSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("VatAppldSC");
            entity.Property(e => e.VatDscntPr).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatExLn).HasColumnName("VatExLN");
            entity.Property(e => e.VatGroup).HasMaxLength(8);
            entity.Property(e => e.VatGrpSrc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_VatGrpSrc");
            entity.Property(e => e.VatPrcnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatSumFrgn).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatSumSy).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatWoDpm).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatWoDpmFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatWoDpmSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VendorNum).HasMaxLength(50);
            entity.Property(e => e.Volume).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Weight1).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Weight2).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.WhsCode).HasMaxLength(8);
            entity.Property(e => e.Width1).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Width2).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.WtCalced)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_INV1_WtCalced");
            entity.Property(e => e.WtLiable)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Oinv>(entity =>
        {
            entity.HasKey(e => e.DocEntry).HasName("OINV_PRIMARY");

            entity.ToTable("OINV");

            entity.HasIndex(e => new { e.NumAtCard, e.CardCode }, "OINV_AT_CARD");

            entity.HasIndex(e => e.CardCode, "OINV_CUSTOMER");

            entity.HasIndex(e => new { e.DocDate, e.Pindicator }, "OINV_DATE_PIND");

            entity.HasIndex(e => new { e.DocStatus, e.Canceled }, "OINV_DOC_STATUS");

            entity.HasIndex(e => new { e.Eseries, e.EdocNum }, "OINV_ESERIES");

            entity.HasIndex(e => e.FolSeries, "OINV_FOL_SERIES");

            entity.HasIndex(e => new { e.FatherCard, e.FatherType }, "OINV_FTHR_CARD");

            entity.HasIndex(e => new { e.DocNum, e.Instance, e.Segment, e.DocSubType, e.Pindicator }, "OINV_NUM");

            entity.HasIndex(e => e.OwnerCode, "OINV_OWNER_CODE");

            entity.HasIndex(e => e.Series, "OINV_SERIES");

            entity.HasIndex(e => new { e.InvntSttus, e.Canceled, e.IsIns }, "OINV_STS_CNCL");

            entity.Property(e => e.DocEntry).ValueGeneratedNever();
            entity.Property(e => e.AddLegIn).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(254);
            entity.Property(e => e.Address2).HasMaxLength(254);
            entity.Property(e => e.AgentCode).HasMaxLength(32);
            entity.Property(e => e.AggregDoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_AggregDoc");
            entity.Property(e => e.AllocNum).HasMaxLength(50);
            entity.Property(e => e.AltBaseTyp).HasDefaultValue(-1, "DF_OINV_AltBaseTyp");
            entity.Property(e => e.AqcsTax).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.AqcsTaxFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("AqcsTaxFC");
            entity.Property(e => e.AqcsTaxSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("AqcsTaxSC");
            entity.Property(e => e.AssetDate).HasColumnType("datetime");
            entity.Property(e => e.AtDocType).HasMaxLength(10);
            entity.Property(e => e.Attachment).HasColumnType("ntext");
            entity.Property(e => e.AuthCode).HasMaxLength(250);
            entity.Property(e => e.AutoCrtFlw)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_AutoCrtFlw");
            entity.Property(e => e.BankCode).HasMaxLength(30);
            entity.Property(e => e.BaseAmnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BaseAmntFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("BaseAmntFC");
            entity.Property(e => e.BaseAmntSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("BaseAmntSC");
            entity.Property(e => e.BaseDisc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BaseDiscFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BaseDiscPr).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BaseDiscSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BaseType).HasDefaultValue(-1, "DF_OINV_BaseType");
            entity.Property(e => e.BaseVtAt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.BaseVtAtFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("BaseVtAtFC");
            entity.Property(e => e.BaseVtAtSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("BaseVtAtSC");
            entity.Property(e => e.BillToOw)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_BillToOW")
                .HasColumnName("BillToOW");
            entity.Property(e => e.BlkCredMmo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_BlkCredMmo");
            entity.Property(e => e.BlockDunn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_BlockDunn");
            entity.Property(e => e.BnkAccount).HasMaxLength(50);
            entity.Property(e => e.BnkBranch).HasMaxLength(50);
            entity.Property(e => e.BnkCntry).HasMaxLength(3);
            entity.Property(e => e.BoeReserev)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_BoeReserev");
            entity.Property(e => e.BookeTdsBp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_BookeTdsBP")
                .HasColumnName("BookeTdsBP");
            entity.Property(e => e.Box1099).HasMaxLength(20);
            entity.Property(e => e.BpchCntc).HasColumnName("BPChCntc");
            entity.Property(e => e.BpchCode)
                .HasMaxLength(15)
                .HasColumnName("BPChCode");
            entity.Property(e => e.Bplid).HasColumnName("BPLId");
            entity.Property(e => e.Bplname)
                .HasMaxLength(200)
                .HasColumnName("BPLName");
            entity.Property(e => e.BpnameOw)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_BPNameOW")
                .HasColumnName("BPNameOW");
            entity.Property(e => e.BuildDesc).HasMaxLength(50);
            entity.Property(e => e.CancelDate).HasColumnType("datetime");
            entity.Property(e => e.Canceled)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_CANCELED")
                .HasColumnName("CANCELED");
            entity.Property(e => e.CardCode).HasMaxLength(15);
            entity.Property(e => e.CardName).HasMaxLength(200);
            entity.Property(e => e.CashDiscFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("CashDiscFC");
            entity.Property(e => e.CashDiscPr).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.CashDiscSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("CashDiscSC");
            entity.Property(e => e.CashDiscnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Ccdentry).HasColumnName("CCDEntry");
            entity.Property(e => e.CdcOffset).HasDefaultValue((short)0, "DF_OINV_CdcOffset");
            entity.Property(e => e.Ceecflag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_CEECFlag")
                .HasColumnName("CEECFlag");
            entity.Property(e => e.CertNum).HasMaxLength(31);
            entity.Property(e => e.CertifNum).HasMaxLength(50);
            entity.Property(e => e.CheckDigit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Cig).HasColumnName("CIG");
            entity.Property(e => e.ClosingOpt).HasDefaultValue((short)1, "DF_OINV_ClosingOpt");
            entity.Property(e => e.ClsDate).HasColumnType("datetime");
            entity.Property(e => e.CntrlBnk).HasMaxLength(15);
            entity.Property(e => e.ComTrade)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("E", "DF_OINV_ComTrade");
            entity.Property(e => e.ComTradeRt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_ComTradeRt");
            entity.Property(e => e.Comments).HasMaxLength(254);
            entity.Property(e => e.Confirmed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OINV_Confirmed");
            entity.Property(e => e.ConfrmedOn).HasColumnType("datetime");
            entity.Property(e => e.CopyNumber).HasDefaultValue(0, "DF_OINV_CopyNumber");
            entity.Property(e => e.CorrExt).HasMaxLength(25);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateTran)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_CreateTran");
            entity.Property(e => e.CreateTs).HasColumnName("CreateTS");
            entity.Property(e => e.CtActTax).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.CtActTaxFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("CtActTaxFC");
            entity.Property(e => e.CtActTaxSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("CtActTaxSC");
            entity.Property(e => e.CtlAccount).HasMaxLength(15);
            entity.Property(e => e.Cup).HasColumnName("CUP");
            entity.Property(e => e.CurSource)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("C", "DF_OINV_CurSource");
            entity.Property(e => e.CustOffice).HasMaxLength(60);
            entity.Property(e => e.DanfelgTxt)
                .HasColumnType("ntext")
                .HasColumnName("DANFELgTxt");
            entity.Property(e => e.DataSource)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_DataSource");
            entity.Property(e => e.DataVers).HasDefaultValue(1, "DF_OINV_DataVers");
            entity.Property(e => e.DateReport).HasColumnType("datetime");
            entity.Property(e => e.DeferrTax)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DigPayment)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_DigPayment");
            entity.Property(e => e.DiscPrcnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DiscSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DiscSumFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("DiscSumFC");
            entity.Property(e => e.DiscSumSy).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DmpTransId)
                .HasMaxLength(20)
                .HasColumnName("DmpTransID");
            entity.Property(e => e.DocCur).HasMaxLength(3);
            entity.Property(e => e.DocDate).HasColumnType("datetime");
            entity.Property(e => e.DocDlvry)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DocDueDate).HasColumnType("datetime");
            entity.Property(e => e.DocManClsd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_DocManClsd");
            entity.Property(e => e.DocRate).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DocStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("O", "DF_OINV_DocStatus");
            entity.Property(e => e.DocSubType)
                .HasMaxLength(2)
                .HasDefaultValue("--", "DF_OINV_DocSubType");
            entity.Property(e => e.DocTaxId)
                .HasMaxLength(32)
                .HasColumnName("DocTaxID");
            entity.Property(e => e.DocTotal).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DocTotalFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("DocTotalFC");
            entity.Property(e => e.DocTotalSy).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DocType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("I", "DF_OINV_DocType");
            entity.Property(e => e.DpayToAddr)
                .HasMaxLength(50)
                .HasColumnName("DPayToAddr");
            entity.Property(e => e.DpmAmnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmAmntFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("DpmAmntFC");
            entity.Property(e => e.DpmAmntSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("DpmAmntSC");
            entity.Property(e => e.DpmAppVat).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmAppVatF).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmAppVatS).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmAppl).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmApplFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmApplSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmAsDscnt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_DpmAsDscnt");
            entity.Property(e => e.DpmDrawn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_DpmDrawn");
            entity.Property(e => e.DpmPrcnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("O", "DF_OINV_DpmStatus");
            entity.Property(e => e.DpmVat).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmVatFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.DpmVatSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Dppstatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_DPPStatus")
                .HasColumnName("DPPStatus");
            entity.Property(e => e.DraftKey)
                .HasDefaultValue(-1, "DF_OINV_draftKey")
                .HasColumnName("draftKey");
            entity.Property(e => e.DutyStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OINV_DutyStatus");
            entity.Property(e => e.EcomerGstn)
                .HasMaxLength(15)
                .HasColumnName("EComerGSTN");
            entity.Property(e => e.EcommerBp)
                .HasMaxLength(15)
                .HasColumnName("ECommerBP");
            entity.Property(e => e.EdocCancel)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_EDocCancel")
                .HasColumnName("EDocCancel");
            entity.Property(e => e.EdocCntnt)
                .HasColumnType("ntext")
                .HasColumnName("EDocCntnt");
            entity.Property(e => e.EdocErrCod)
                .HasMaxLength(50)
                .HasColumnName("EDocErrCod");
            entity.Property(e => e.EdocErrMsg)
                .HasColumnType("ntext")
                .HasColumnName("EDocErrMsg");
            entity.Property(e => e.EdocExpFrm).HasColumnName("EDocExpFrm");
            entity.Property(e => e.EdocGenTyp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_EDocGenTyp")
                .HasColumnName("EDocGenTyp");
            entity.Property(e => e.EdocNum)
                .HasMaxLength(50)
                .HasColumnName("EDocNum");
            entity.Property(e => e.EdocPrefix)
                .HasMaxLength(10)
                .HasColumnName("EDocPrefix");
            entity.Property(e => e.EdocProces)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("C", "DF_OINV_EDocProces")
                .HasColumnName("EDocProces");
            entity.Property(e => e.EdocStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("C", "DF_OINV_EDocStatus")
                .HasColumnName("EDocStatus");
            entity.Property(e => e.EdocTest)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_EDocTest")
                .HasColumnName("EDocTest");
            entity.Property(e => e.EdocType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("F", "DF_OINV_EDocType")
                .HasColumnName("EDocType");
            entity.Property(e => e.ElCoMsg).HasMaxLength(254);
            entity.Property(e => e.ElCoStatus).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EnBnkAcct).HasColumnType("ntext");
            entity.Property(e => e.EncryptIv)
                .HasMaxLength(100)
                .HasColumnName("EncryptIV");
            entity.Property(e => e.EndDlvDate).HasColumnType("datetime");
            entity.Property(e => e.EnvTypeNfe)
                .HasDefaultValue(-1, "DF_OINV_EnvTypeNFe")
                .HasColumnName("EnvTypeNFe");
            entity.Property(e => e.EquVatSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.EquVatSumF).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.EquVatSumS).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Eseries).HasColumnName("ESeries");
            entity.Property(e => e.EwbgenType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EWBGenType");
            entity.Property(e => e.ExcDocDate).HasColumnType("datetime");
            entity.Property(e => e.ExcRefDate).HasColumnType("datetime");
            entity.Property(e => e.ExcRmvTime).HasMaxLength(8);
            entity.Property(e => e.Excised)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("O", "DF_OINV_Excised");
            entity.Property(e => e.ExclTaxRep)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_ExclTaxRep");
            entity.Property(e => e.ExepAmnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExepAmntFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExepAmntFC");
            entity.Property(e => e.ExepAmntSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExepAmntSC");
            entity.Property(e => e.ExepCuAmnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExepCuFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExepCuFC");
            entity.Property(e => e.ExepCuSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExepCuSC");
            entity.Property(e => e.ExpAnFrgn).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExpAnSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExpAnSys).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExpAppl).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ExpApplFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExpApplFC");
            entity.Property(e => e.ExpApplSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExpApplSC");
            entity.Property(e => e.Exported)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Exported");
            entity.Property(e => e.ExptVat)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExptVAt");
            entity.Property(e => e.ExptVatFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExptVAtFC");
            entity.Property(e => e.ExptVatSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("ExptVAtSC");
            entity.Property(e => e.FatherCard).HasMaxLength(15);
            entity.Property(e => e.FatherType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("P", "DF_OINV_FatherType");
            entity.Property(e => e.FcepmnMean)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_FCEPmnMean")
                .HasColumnName("FCEPmnMean");
            entity.Property(e => e.Fci)
                .HasMaxLength(36)
                .HasColumnName("FCI");
            entity.Property(e => e.Filler).HasMaxLength(8);
            entity.Property(e => e.FiscDocNum).HasMaxLength(100);
            entity.Property(e => e.Flags).HasDefaultValue(0, "DF_OINV_Flags");
            entity.Property(e => e.FlwRefDate).HasColumnType("datetime");
            entity.Property(e => e.FlwRefNum).HasMaxLength(100);
            entity.Property(e => e.FoCfrght)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("FoCFrght");
            entity.Property(e => e.FoCfrghtFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("FoCFrghtFC");
            entity.Property(e => e.FoCfrghtSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("FoCFrghtSC");
            entity.Property(e => e.FoCtax)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("FoCTax");
            entity.Property(e => e.FoCtaxFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("FoCTaxFC");
            entity.Property(e => e.FoCtaxSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("FoCTaxSC");
            entity.Property(e => e.FolioPref).HasMaxLength(4);
            entity.Property(e => e.Footer).HasColumnType("ntext");
            entity.Property(e => e.FreeChrg).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.FreeChrgFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("FreeChrgFC");
            entity.Property(e => e.FreeChrgSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("FreeChrgSC");
            entity.Property(e => e.FrmBpDate).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.GrosProfFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("GrosProfFC");
            entity.Property(e => e.GrosProfSy).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.GrosProfit).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.GrossBase).HasDefaultValue((short)0, "DF_OINV_GrossBase");
            entity.Property(e => e.GsttranTyp)
                .HasMaxLength(2)
                .HasColumnName("GSTTranTyp");
            entity.Property(e => e.Gtsrlvnt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_GTSRlvnt")
                .HasColumnName("GTSRlvnt");
            entity.Property(e => e.Handwrtten)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Handwrtten");
            entity.Property(e => e.Header).HasColumnType("ntext");
            entity.Property(e => e.IgnRelDoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_IgnRelDoc");
            entity.Property(e => e.IndFinal)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_IndFinal");
            entity.Property(e => e.Indicator).HasMaxLength(2);
            entity.Property(e => e.Installmnt).HasDefaultValue((short)1, "DF_OINV_Installmnt");
            entity.Property(e => e.Instance).HasDefaultValue((short)0, "DF_OINV_Instance");
            entity.Property(e => e.InsurOp347)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_InsurOp347");
            entity.Property(e => e.InterimTyp).HasDefaultValue((short)0, "DF_OINV_InterimTyp");
            entity.Property(e => e.InvntDirec)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("X", "DF_OINV_InvntDirec");
            entity.Property(e => e.InvntSttus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("O", "DF_OINV_InvntSttus");
            entity.Property(e => e.IsAlt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_IsAlt");
            entity.Property(e => e.IsCrin)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_isCrin")
                .HasColumnName("isCrin");
            entity.Property(e => e.IsIct)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_IsICT")
                .HasColumnName("IsICT");
            entity.Property(e => e.IsIns)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_isIns")
                .HasColumnName("isIns");
            entity.Property(e => e.IsPaytoBnk)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IsReuseNfn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_IsReuseNFN")
                .HasColumnName("IsReuseNFN");
            entity.Property(e => e.IsReuseNum)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_IsReuseNum");
            entity.Property(e => e.IsrcodLine)
                .HasMaxLength(53)
                .HasColumnName("ISRCodLine");
            entity.Property(e => e.IssReason).HasDefaultValue((short)1, "DF_OINV_IssReason");
            entity.Property(e => e.JrnlMemo).HasMaxLength(254);
            entity.Property(e => e.Kvvatcode)
                .HasColumnType("ntext")
                .HasColumnName("KVVATCode");
            entity.Property(e => e.LastPmnTyp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Letter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LetterNum).HasMaxLength(50);
            entity.Property(e => e.LicTradNum).HasMaxLength(32);
            entity.Property(e => e.LogInstanc).HasDefaultValue(0, "DF_OINV_LogInstanc");
            entity.Property(e => e.LpgFolioN).HasColumnName("LPgFolioN");
            entity.Property(e => e.LypmtAt)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("LYPmtAt");
            entity.Property(e => e.LypmtAtFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("LYPmtAtFC");
            entity.Property(e => e.LypmtAtSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("LYPmtAtSC");
            entity.Property(e => e.ManualNum).HasMaxLength(20);
            entity.Property(e => e.Max1099).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.MaxDscn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_MaxDscn");
            entity.Property(e => e.MinvDate)
                .HasColumnType("datetime")
                .HasColumnName("MInvDate");
            entity.Property(e => e.MinvNum).HasColumnName("MInvNum");
            entity.Property(e => e.Model)
                .HasMaxLength(6)
                .HasDefaultValue("0", "DF_OINV_Model");
            entity.Property(e => e.Myftype)
                .HasMaxLength(2)
                .HasColumnName("MYFtype");
            entity.Property(e => e.NbSbAmntFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NbSbAmntFC");
            entity.Property(e => e.NbSbVatFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NbSbVAtFC");
            entity.Property(e => e.NcorrInv).HasColumnName("NCorrInv");
            entity.Property(e => e.NetProc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_NetProc");
            entity.Property(e => e.NfePrntFo).HasDefaultValue(0, "DF_OINV_NfePrntFo");
            entity.Property(e => e.NfeValue).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.NnSbAmnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.NnSbAmntSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NnSbAmntSC");
            entity.Property(e => e.NnSbCuAmnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.NnSbCuFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NnSbCuFC");
            entity.Property(e => e.NnSbCuSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NnSbCuSC");
            entity.Property(e => e.NnSbVat)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NnSbVAt");
            entity.Property(e => e.NnSbVatSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NnSbVAtSC");
            entity.Property(e => e.NonDdAmt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.NonDdAmtFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NonDdAmtFC");
            entity.Property(e => e.NonDdAmtSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("NonDdAmtSC");
            entity.Property(e => e.NotRel4Mi)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_NotRel4MI")
                .HasColumnName("NotRel4MI");
            entity.Property(e => e.Notify)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NtsapprNo)
                .HasMaxLength(50)
                .HasColumnName("NTSApprNo");
            entity.Property(e => e.Ntsapprov)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_NTSApprov")
                .HasColumnName("NTSApprov");
            entity.Property(e => e.NtseTaxNo)
                .HasMaxLength(50)
                .HasColumnName("NTSeTaxNo");
            entity.Property(e => e.NtswebSite).HasColumnName("NTSWebSite");
            entity.Property(e => e.NumAtCard).HasMaxLength(200);
            entity.Property(e => e.ObjType)
                .HasMaxLength(20)
                .HasDefaultValue("13", "DF_OINV_ObjType");
            entity.Property(e => e.OnlineQuo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_OnlineQuo");
            entity.Property(e => e.OpenForLaC)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OINV_OpenForLaC");
            entity.Property(e => e.Ordered)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Ordered");
            entity.Property(e => e.OriginType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("M", "DF_OINV_OriginType");
            entity.Property(e => e.PaidDpm).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PaidDpmF).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PaidDpmS).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PaidFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("PaidFC");
            entity.Property(e => e.PaidSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PaidSumFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PaidSumSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PaidSys).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PaidToDate).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PartSupply)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OINV_PartSupply");
            entity.Property(e => e.PayBlock)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_PayBlock");
            entity.Property(e => e.PayDuMonth)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PayToCode).HasMaxLength(50);
            entity.Property(e => e.PaymentRef).HasMaxLength(27);
            entity.Property(e => e.PermitNo).HasMaxLength(20);
            entity.Property(e => e.PeyMethod).HasMaxLength(15);
            entity.Property(e => e.Pick)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Pick");
            entity.Property(e => e.PickRmrk).HasMaxLength(254);
            entity.Property(e => e.PickStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_PickStatus");
            entity.Property(e => e.Pindicator)
                .HasMaxLength(10)
                .HasDefaultValue(" ", "DF_OINV_PIndicator")
                .HasColumnName("PIndicator");
            entity.Property(e => e.PoDropPrss)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_PoDropPrss");
            entity.Property(e => e.PoPrss)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_PoPrss");
            entity.Property(e => e.PoscashN).HasColumnName("POSCashN");
            entity.Property(e => e.PoseqNum)
                .HasMaxLength(20)
                .HasColumnName("POSEqNum");
            entity.Property(e => e.PosmanufSn)
                .HasMaxLength(20)
                .HasColumnName("POSManufSN");
            entity.Property(e => e.PosrcptNo).HasColumnName("POSRcptNo");
            entity.Property(e => e.PostPmntWt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_PostPmntWT")
                .HasColumnName("PostPmntWT");
            entity.Property(e => e.Posted)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OINV_Posted");
            entity.Property(e => e.PqtgrpHw)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_PQTGrpHW")
                .HasColumnName("PQTGrpHW");
            entity.Property(e => e.PqtgrpNum).HasColumnName("PQTGrpNum");
            entity.Property(e => e.PqtgrpSer).HasColumnName("PQTGrpSer");
            entity.Property(e => e.PriceMode)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PrintSepa)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_PrintSEPA")
                .HasColumnName("PrintSEPA");
            entity.Property(e => e.Printed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Printed");
            entity.Property(e => e.Project).HasMaxLength(20);
            entity.Property(e => e.Pticode)
                .HasMaxLength(5)
                .HasColumnName("PTICode");
            entity.Property(e => e.QrcodeSpgn)
                .HasColumnType("ntext")
                .HasColumnName("QRCodeSPGn");
            entity.Property(e => e.QrcodeSrc)
                .HasColumnType("ntext")
                .HasColumnName("QRCodeSrc");
            entity.Property(e => e.Ref1).HasMaxLength(11);
            entity.Property(e => e.Ref2).HasMaxLength(11);
            entity.Property(e => e.Rel4Pptax)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Rel4PPTax")
                .HasColumnName("Rel4PPTax");
            entity.Property(e => e.RelatedTyp).HasDefaultValue(-1, "DF_OINV_RelatedTyp");
            entity.Property(e => e.ReopManCls)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ReopOriDoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RepSection).HasMaxLength(3);
            entity.Property(e => e.ReqCode).HasMaxLength(50);
            entity.Property(e => e.ReqDate).HasColumnType("datetime");
            entity.Property(e => e.ReqId).HasColumnName("ReqID");
            entity.Property(e => e.ReqName).HasMaxLength(155);
            entity.Property(e => e.ReqType).HasDefaultValue(12, "DF_OINV_ReqType");
            entity.Property(e => e.Requester).HasMaxLength(25);
            entity.Property(e => e.Reserve)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Reserve");
            entity.Property(e => e.ResidenNum)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("1", "DF_OINV_ResidenNum");
            entity.Property(e => e.RetInvoice)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_RetInvoice");
            entity.Property(e => e.RevCreRefD).HasColumnType("datetime");
            entity.Property(e => e.RevCreRefN).HasMaxLength(100);
            entity.Property(e => e.RevRefDate).HasColumnType("datetime");
            entity.Property(e => e.RevRefNo).HasMaxLength(100);
            entity.Property(e => e.Revision)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Revision");
            entity.Property(e => e.RevisionPo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_RevisionPo");
            entity.Property(e => e.RoundDif).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.RoundDifFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("RoundDifFC");
            entity.Property(e => e.RoundDifSy).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Rounding)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Rounding");
            entity.Property(e => e.Sappassprt)
                .HasColumnType("ntext")
                .HasColumnName("SAPPassprt");
            entity.Property(e => e.Segment).HasDefaultValue((short)0, "DF_OINV_Segment");
            entity.Property(e => e.SelfInv)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_selfInv")
                .HasColumnName("selfInv");
            entity.Property(e => e.SelfPosted)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_SelfPosted");
            entity.Property(e => e.SeriesStr).HasMaxLength(3);
            entity.Property(e => e.ShipPlace).HasMaxLength(60);
            entity.Property(e => e.ShipState).HasMaxLength(3);
            entity.Property(e => e.ShipToCode).HasMaxLength(50);
            entity.Property(e => e.ShipToOw)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_ShipToOW")
                .HasColumnName("ShipToOW");
            entity.Property(e => e.ShowScn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_ShowSCN")
                .HasColumnName("ShowSCN");
            entity.Property(e => e.SignDigest).HasColumnType("ntext");
            entity.Property(e => e.SignMsg).HasColumnType("ntext");
            entity.Property(e => e.SlpCode).HasDefaultValue(-1, "DF_OINV_SlpCode");
            entity.Property(e => e.SoiwizId).HasColumnName("SOIWizId");
            entity.Property(e => e.SpecDate).HasColumnType("datetime");
            entity.Property(e => e.SplitPmnt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_SplitPmnt");
            entity.Property(e => e.SplitTax).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.SplitTaxFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("SplitTaxFC");
            entity.Property(e => e.SplitTaxSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("SplitTaxSC");
            entity.Property(e => e.SrvGpPrcnt).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.SrvTaxRule)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_SrvTaxRule");
            entity.Property(e => e.Ssiexmpt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SSIExmpt");
            entity.Property(e => e.StDlvDate).HasColumnType("datetime");
            entity.Property(e => e.StampNum).HasMaxLength(16);
            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.SubStr).HasMaxLength(3);
            entity.Property(e => e.Submitted)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_submitted")
                .HasColumnName("submitted");
            entity.Property(e => e.SumAbsId).HasDefaultValue(-1, "DF_OINV_SumAbsId");
            entity.Property(e => e.SumRptDate).HasColumnType("datetime");
            entity.Property(e => e.SummryType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_SummryType");
            entity.Property(e => e.SupplCode).HasMaxLength(254);
            entity.Property(e => e.Supplier).HasMaxLength(15);
            entity.Property(e => e.SysRate).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TaxDate).HasColumnType("datetime");
            entity.Property(e => e.TaxInvNo).HasMaxLength(100);
            entity.Property(e => e.TaxOnExAp).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TaxOnExApF).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TaxOnExApS).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TaxOnExp).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TaxOnExpFc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TaxOnExpSc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ToBinCode).HasMaxLength(228);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.ToWhsCode).HasMaxLength(8);
            entity.Property(e => e.TotalExpFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("TotalExpFC");
            entity.Property(e => e.TotalExpSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("TotalExpSC");
            entity.Property(e => e.TotalExpns).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.TrackNo).HasMaxLength(30);
            entity.Property(e => e.Transfered)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_Transfered");
            entity.Property(e => e.TrnspCode).HasDefaultValue((short)-1, "DF_OINV_TrnspCode");
            entity.Property(e => e.TxInvRptDt).HasColumnType("datetime");
            entity.Property(e => e.TxInvRptNo).HasMaxLength(10);
            entity.Property(e => e.UAllocationNumber)
                .HasMaxLength(30)
                .HasColumnName("U_AllocationNumber");
            entity.Property(e => e.UAuthorisedBy)
                .HasMaxLength(30)
                .HasColumnName("U_AuthorisedBy");
            entity.Property(e => e.UBaseOperation)
                .HasMaxLength(30)
                .HasColumnName("U_BaseOperation");
            entity.Property(e => e.UCarrier)
                .HasMaxLength(30)
                .HasColumnName("U_Carrier");
            entity.Property(e => e.UCarrierConsignmentNo)
                .HasMaxLength(30)
                .HasColumnName("U_CarrierConsignmentNo");
            entity.Property(e => e.UCheckedBy)
                .HasMaxLength(30)
                .HasColumnName("U_CheckedBy");
            entity.Property(e => e.UClaimOutcome)
                .HasMaxLength(10)
                .HasColumnName("U_ClaimOutcome");
            entity.Property(e => e.UConsignment)
                .HasMaxLength(30)
                .HasColumnName("U_Consignment");
            entity.Property(e => e.UContractCode)
                .HasMaxLength(30)
                .HasColumnName("U_ContractCode");
            entity.Property(e => e.UCustomerClaimAmnt)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_CustomerClaimAmnt");
            entity.Property(e => e.UCustomerClaimNo)
                .HasMaxLength(30)
                .HasColumnName("U_CustomerClaimNo");
            entity.Property(e => e.UDefaultWarehouse)
                .HasMaxLength(10)
                .HasColumnName("U_DefaultWarehouse");
            entity.Property(e => e.UDeliveredDate)
                .HasColumnType("datetime")
                .HasColumnName("U_DeliveredDate");
            entity.Property(e => e.UDistributionDate)
                .HasColumnType("datetime")
                .HasColumnName("U_DistributionDate");
            entity.Property(e => e.UDistributionProgress)
                .HasMaxLength(30)
                .HasColumnName("U_DistributionProgress");
            entity.Property(e => e.UDistributionStatus)
                .HasMaxLength(30)
                .HasColumnName("U_DistributionStatus");
            entity.Property(e => e.UDocumentComplete)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("0", "DF_OINV_U_DocumentComplete")
                .HasColumnName("U_DocumentComplete");
            entity.Property(e => e.UDocumentLineNumber).HasColumnName("U_DocumentLineNumber");
            entity.Property(e => e.UDocumentNumber).HasColumnName("U_DocumentNumber");
            entity.Property(e => e.UEdiinvoice)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("0", "DF_OINV_U_EDIInvoice")
                .HasColumnName("U_EDIInvoice");
            entity.Property(e => e.UEdiorder)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("0", "DF_OINV_U_EDIOrder")
                .HasColumnName("U_EDIOrder");
            entity.Property(e => e.UFmcgorderType)
                .HasMaxLength(15)
                .HasColumnName("U_FMCGOrderType");
            entity.Property(e => e.UFromOperation)
                .HasMaxLength(30)
                .HasColumnName("U_FromOperation");
            entity.Property(e => e.UInTransitDate)
                .HasColumnType("datetime")
                .HasColumnName("U_InTransitDate");
            entity.Property(e => e.UInvoiceNo)
                .HasMaxLength(30)
                .HasColumnName("U_InvoiceNo");
            entity.Property(e => e.UJaborder)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("0", "DF_OINV_U_JABOrder")
                .HasColumnName("U_JABOrder");
            entity.Property(e => e.ULoad)
                .HasMaxLength(30)
                .HasColumnName("U_Load");
            entity.Property(e => e.ULoadDate)
                .HasColumnType("datetime")
                .HasColumnName("U_LoadDate");
            entity.Property(e => e.ULoadSheetDate)
                .HasColumnType("datetime")
                .HasColumnName("U_LoadSheetDate");
            entity.Property(e => e.ULoadSheetNumber)
                .HasMaxLength(30)
                .HasColumnName("U_LoadSheetNumber");
            entity.Property(e => e.UMacmobileInv)
                .HasMaxLength(30)
                .HasColumnName("U_MacmobileInv");
            entity.Property(e => e.UManifestNumber)
                .HasMaxLength(30)
                .HasColumnName("U_ManifestNumber");
            entity.Property(e => e.UOrderFill)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_OrderFill");
            entity.Property(e => e.UOrderType)
                .HasMaxLength(30)
                .HasColumnName("U_OrderType");
            entity.Property(e => e.UPickInstructionNumber)
                .HasMaxLength(30)
                .HasColumnName("U_PickInstructionNumber");
            entity.Property(e => e.UPodDate)
                .HasColumnType("datetime")
                .HasColumnName("U_PodDate");
            entity.Property(e => e.UPrincipal)
                .HasMaxLength(15)
                .HasColumnName("U_Principal");
            entity.Property(e => e.UPrincipal2)
                .HasMaxLength(50)
                .HasColumnName("U_Principal2");
            entity.Property(e => e.UPurchaseOrderNo)
                .HasMaxLength(30)
                .HasColumnName("U_PurchaseOrderNo");
            entity.Property(e => e.UReceivedBy)
                .HasMaxLength(30)
                .HasColumnName("U_ReceivedBy");
            entity.Property(e => e.URequiredDeliveryDate)
                .HasColumnType("datetime")
                .HasColumnName("U_RequiredDeliveryDate");
            entity.Property(e => e.UServiceType)
                .HasMaxLength(30)
                .HasColumnName("U_ServiceType");
            entity.Property(e => e.UShipmentNumber)
                .HasMaxLength(30)
                .HasColumnName("U_ShipmentNumber");
            entity.Property(e => e.USubContractor)
                .HasMaxLength(30)
                .HasColumnName("U_SubContractor");
            entity.Property(e => e.USubPovalue)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_SubPOValue");
            entity.Property(e => e.USupplierInvoiceDate)
                .HasColumnType("datetime")
                .HasColumnName("U_SupplierInvoiceDate");
            entity.Property(e => e.USupplierInvoiceNo)
                .HasMaxLength(30)
                .HasColumnName("U_SupplierInvoiceNo");
            entity.Property(e => e.UTmssyncDate)
                .HasColumnType("datetime")
                .HasColumnName("U_TMSSyncDate");
            entity.Property(e => e.UTmssyncError)
                .HasMaxLength(100)
                .HasColumnName("U_TMSSyncError");
            entity.Property(e => e.UToOperation)
                .HasMaxLength(30)
                .HasColumnName("U_ToOperation");
            entity.Property(e => e.UTransferred)
                .HasMaxLength(10)
                .HasDefaultValue("No", "DF_OINV_U_Transferred")
                .HasColumnName("U_Transferred");
            entity.Property(e => e.UTrip)
                .HasMaxLength(30)
                .HasColumnName("U_Trip");
            entity.Property(e => e.UTripNumber)
                .HasMaxLength(30)
                .HasColumnName("U_TripNumber");
            entity.Property(e => e.UUpliftmentNo)
                .HasMaxLength(30)
                .HasColumnName("U_UpliftmentNo");
            entity.Property(e => e.UVendorRebate)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("U_VendorRebate");
            entity.Property(e => e.UpdCardBal)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_UpdCardBal");
            entity.Property(e => e.UpdInvnt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_UpdInvnt");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateTs).HasColumnName("UpdateTS");
            entity.Property(e => e.UseBilAddr)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UseCorrVat)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_UseCorrVat");
            entity.Property(e => e.UseShpdGd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OINV_UseShpdGd");
            entity.Property(e => e.VatDate).HasColumnType("datetime");
            entity.Property(e => e.VatJenum)
                .HasDefaultValue(-1, "DF_OINV_VatJENum")
                .HasColumnName("VatJENum");
            entity.Property(e => e.VatPaid).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatPaidFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("VatPaidFC");
            entity.Property(e => e.VatPaidSys).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatPercent).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.VatSumFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("VatSumFC");
            entity.Property(e => e.VatSumSy).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Vatfirst)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("VATFirst");
            entity.Property(e => e.VatregNum)
                .HasMaxLength(32)
                .HasColumnName("VATRegNum");
            entity.Property(e => e.VclPlate).HasMaxLength(20);
            entity.Property(e => e.VersionNum).HasMaxLength(13);
            entity.Property(e => e.Volume).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.WddStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("-", "DF_OINV_WddStatus");
            entity.Property(e => e.Weight).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.Wtapplied)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("WTApplied");
            entity.Property(e => e.WtappliedF)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("WTAppliedF");
            entity.Property(e => e.WtappliedS)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("WTAppliedS");
            entity.Property(e => e.Wtdetails)
                .HasMaxLength(100)
                .HasColumnName("WTDetails");
            entity.Property(e => e.Wtsum)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("WTSum");
            entity.Property(e => e.WtsumFc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("WTSumFC");
            entity.Property(e => e.WtsumSc)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("WTSumSC");
        });

        modelBuilder.Entity<Ousr>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("OUSR_PRIMARY");

            entity.ToTable("OUSR");

            entity.HasIndex(e => e.InternalK, "OUSR_INTERNAL").IsUnique();

            entity.HasIndex(e => e.Password, "OUSR_PASSWORD");

            entity.HasIndex(e => e.UserCode, "OUSR_USER_CODE").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("USERID");
            entity.Property(e => e.Address).HasMaxLength(254);
            entity.Property(e => e.AdvImagePr)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AlertPolFr).HasDefaultValue((short)5, "DF_OUSR_AlertPolFr");
            entity.Property(e => e.AllBrnchF)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OUSR_AllBrnchF");
            entity.Property(e => e.Allowences)
                .HasColumnType("ntext")
                .HasColumnName("ALLOWENCES");
            entity.Property(e => e.AuImpRates)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_AuImpRates");
            entity.Property(e => e.AutoAsnBpl)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_AutoAsnBPL")
                .HasColumnName("AutoAsnBPL");
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.Branch).HasDefaultValue((short)-2, "DF_OUSR_Branch");
            entity.Property(e => e.CashLimit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_CashLimit");
            entity.Property(e => e.CdtPrvDays).HasDefaultValue(1, "DF_OUSR_CdtPrvDays");
            entity.Property(e => e.CheckFiles)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_CheckFiles");
            entity.Property(e => e.CleanChb)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_CleanChb");
            entity.Property(e => e.CleanIbx)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_CleanIbx");
            entity.Property(e => e.CleanObx)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_CleanObx");
            entity.Property(e => e.CleanSnt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_CleanSnt");
            entity.Property(e => e.ContactLog)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_ContactLog");
            entity.Property(e => e.Country).HasMaxLength(3);
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.CreditDay1).HasDefaultValue((short)1, "DF_OUSR_CreditDay1");
            entity.Property(e => e.CreditDay2).HasDefaultValue((short)15, "DF_OUSR_CreditDay2");
            entity.Property(e => e.Cusagree)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CUSAgree");
            entity.Property(e => e.DType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("S", "DF_OUSR_dType")
                .HasColumnName("dType");
            entity.Property(e => e.Department).HasDefaultValue((short)-2, "DF_OUSR_Department");
            entity.Property(e => e.DfltsGroup).HasMaxLength(8);
            entity.Property(e => e.DigCrtPath).HasColumnType("ntext");
            entity.Property(e => e.Discount)
                .HasColumnType("numeric(19, 6)")
                .HasColumnName("DISCOUNT");
            entity.Property(e => e.DomainUser).HasMaxLength(50);
            entity.Property(e => e.DotAsSep)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_DotAsSep");
            entity.Property(e => e.Dppstatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_DPPStatus")
                .HasColumnName("DPPStatus");
            entity.Property(e => e.DsplyRates)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_DsplyRates");
            entity.Property(e => e.EMail)
                .HasMaxLength(100)
                .HasColumnName("E_Mail");
            entity.Property(e => e.EmailSig).HasColumnType("ntext");
            entity.Property(e => e.EnbMenuFlt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_EnbMenuFlt");
            entity.Property(e => e.EncryptIv)
                .HasMaxLength(100)
                .HasColumnName("EncryptIV");
            entity.Property(e => e.EnterAsTab)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_EnterAsTab");
            entity.Property(e => e.EvtNotify)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OUSR_EvtNotify");
            entity.Property(e => e.FailedLog).HasDefaultValue(0, "DF_OUSR_FailedLog");
            entity.Property(e => e.Fax).HasMaxLength(50);
            entity.Property(e => e.Font).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("F", "DF_OUSR_GENDER")
                .HasColumnName("GENDER");
            entity.Property(e => e.Groups)
                .HasDefaultValue((short)0, "DF_OUSR_GROUPS")
                .HasColumnName("GROUPS");
            entity.Property(e => e.HandleEdoc)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OUSR_HandleEDoc")
                .HasColumnName("HandleEDoc");
            entity.Property(e => e.IgnDtOwn)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_IgnDtOwn");
            entity.Property(e => e.Info1File).HasMaxLength(4);
            entity.Property(e => e.Info2File).HasMaxLength(4);
            entity.Property(e => e.Info3File).HasMaxLength(4);
            entity.Property(e => e.Info4File).HasMaxLength(4);
            entity.Property(e => e.InternalK).HasColumnName("INTERNAL_K");
            entity.Property(e => e.IntgrtEb)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_IntgrtEb");
            entity.Property(e => e.LastClean).HasColumnType("datetime");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("lastLogin");
            entity.Property(e => e.LastPwdSet).HasColumnType("datetime");
            entity.Property(e => e.LastPwds).HasColumnType("ntext");
            entity.Property(e => e.LastPwds2)
                .HasMaxLength(254)
                .HasDefaultValue("0", "DF_OUSR_LastPwds2");
            entity.Property(e => e.LastWarned).HasColumnType("datetime");
            entity.Property(e => e.LicBaHdate)
                .HasColumnType("datetime")
                .HasColumnName("LicBaHDate");
            entity.Property(e => e.Locked)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_Locked");
            entity.Property(e => e.LogInstanc).HasColumnName("logInstanc");
            entity.Property(e => e.LstLogoutD).HasColumnType("datetime");
            entity.Property(e => e.LstPwdChB).HasMaxLength(25);
            entity.Property(e => e.MaxCashSum).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.MobileImei)
                .HasMaxLength(64)
                .HasColumnName("MobileIMEI");
            entity.Property(e => e.MobileUser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_MobileUser");
            entity.Property(e => e.MouseOnly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_MouseOnly");
            entity.Property(e => e.NaturalPer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_NaturalPer");
            entity.Property(e => e.NoStpwdNum)
                .HasDefaultValue((short)0, "DF_OUSR_NoSTPwdNum")
                .HasColumnName("NoSTPwdNum");
            entity.Property(e => e.ObjType)
                .HasMaxLength(20)
                .HasDefaultValue("12", "DF_OUSR_objType")
                .HasColumnName("objType");
            entity.Property(e => e.OneLogPwd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OUSR_OneLogPwd");
            entity.Property(e => e.OpenCdt)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_OpenCdt");
            entity.Property(e => e.OpenCredit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.OpenDps)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_OpenDps");
            entity.Property(e => e.OutOfOffic)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_OutOfOffic");
            entity.Property(e => e.Password)
                .HasMaxLength(254)
                .HasDefaultValue("0", "DF_OUSR_PASSWORD")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Password1)
                .HasMaxLength(8)
                .HasColumnName("PASSWORD1");
            entity.Property(e => e.Password2)
                .HasMaxLength(8)
                .HasColumnName("PASSWORD2");
            entity.Property(e => e.Password3)
                .HasMaxLength(8)
                .HasColumnName("PASSWORD3");
            entity.Property(e => e.Password4)
                .HasMaxLength(254)
                .HasColumnName("PASSWORD4");
            entity.Property(e => e.Picture).HasMaxLength(200);
            entity.Property(e => e.PortNum).HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(90);
            entity.Property(e => e.PrsWkCntEb)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_PrsWkCntEb");
            entity.Property(e => e.PurchDisc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.PwdNeverEx)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_PwdNeverEx");
            entity.Property(e => e.RclFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_RclFlag");
            entity.Property(e => e.RcrFlag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_RcrFlag");
            entity.Property(e => e.SalesDisc).HasColumnType("numeric(19, 6)");
            entity.Property(e => e.ScreenLock).HasDefaultValue((short)30, "DF_OUSR_ScreenLock");
            entity.Property(e => e.SendEmail)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_SendEMail")
                .HasColumnName("SendEMail");
            entity.Property(e => e.SendFax)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_SendFax");
            entity.Property(e => e.SendSms)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_SendSMS")
                .HasColumnName("SendSMS");
            entity.Property(e => e.ShowFstMsg)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_ShowFstMsg");
            entity.Property(e => e.ShowLicBal)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OUSR_ShowLicBal");
            entity.Property(e => e.ShowMsgNum).HasDefaultValue(100, "DF_OUSR_ShowMsgNum");
            entity.Property(e => e.ShowNewMsg)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OUSR_ShowNewMsg");
            entity.Property(e => e.ShowNewTsk)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("Y", "DF_OUSR_ShowNewTsk");
            entity.Property(e => e.SkinType).HasMaxLength(254);
            entity.Property(e => e.SnapShotId).HasDefaultValue(0, "DF_OUSR_SnapShotId");
            entity.Property(e => e.Stdata)
                .HasMaxLength(40)
                .HasColumnName("STData");
            entity.Property(e => e.Superuser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_SUPERUSER")
                .HasColumnName("SUPERUSER");
            entity.Property(e => e.SupportUsr)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasDefaultValue("N", "DF_OUSR_SupportUsr");
            entity.Property(e => e.Tel1).HasMaxLength(50);
            entity.Property(e => e.Tel2).HasMaxLength(50);
            entity.Property(e => e.Tplid).HasColumnName("TPLId");
            entity.Property(e => e.UName)
                .HasMaxLength(155)
                .HasColumnName("U_NAME");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.UserCode)
                .HasMaxLength(25)
                .HasColumnName("USER_CODE");
            entity.Property(e => e.UserPrefs).HasColumnType("image");
            entity.Property(e => e.UserSign).HasColumnName("userSign");
            entity.Property(e => e.UserSign2).HasColumnName("userSign2");
            entity.Property(e => e.WallPaper).HasColumnType("ntext");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
