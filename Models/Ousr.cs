using System;
using System.Collections.Generic;

namespace ERS_Intergration.Models;

public partial class Ousr
{
    public short Userid { get; set; }

    public string? Password { get; set; }

    public string? Password1 { get; set; }

    public string? Password2 { get; set; }

    public short InternalK { get; set; }

    public string UserCode { get; set; } = null!;

    public string? UName { get; set; }

    public short? Groups { get; set; }

    public string? Password4 { get; set; }

    public string? Allowences { get; set; }

    public string? Superuser { get; set; }

    public decimal? Discount { get; set; }

    public string? Password3 { get; set; }

    public string? Info1File { get; set; }

    public short? Info1Field { get; set; }

    public string? Info2File { get; set; }

    public short? Info2Field { get; set; }

    public string? Info3File { get; set; }

    public short? Info3Field { get; set; }

    public string? Info4File { get; set; }

    public short? Info4Field { get; set; }

    public string? DType { get; set; }

    public string? EMail { get; set; }

    public string? PortNum { get; set; }

    public string? OutOfOffic { get; set; }

    public string? SendEmail { get; set; }

    public string? SendSms { get; set; }

    public string? DfltsGroup { get; set; }

    public string? CashLimit { get; set; }

    public decimal? MaxCashSum { get; set; }

    public string? Fax { get; set; }

    public string? SendFax { get; set; }

    public string? Locked { get; set; }

    public short? Department { get; set; }

    public short? Branch { get; set; }

    public byte[]? UserPrefs { get; set; }

    public int? Language { get; set; }

    public short? Charset { get; set; }

    public string? OpenCdt { get; set; }

    public int? CdtPrvDays { get; set; }

    public string? DsplyRates { get; set; }

    public string? AuImpRates { get; set; }

    public string? OpenDps { get; set; }

    public string? RcrFlag { get; set; }

    public string? CheckFiles { get; set; }

    public string? OpenCredit { get; set; }

    public short? CreditDay1 { get; set; }

    public short? CreditDay2 { get; set; }

    public string? WallPaper { get; set; }

    public short? WllPprDsp { get; set; }

    public string? AdvImagePr { get; set; }

    public string? ContactLog { get; set; }

    public DateTime? LastWarned { get; set; }

    public short? AlertPolFr { get; set; }

    public short? ScreenLock { get; set; }

    public string? ShowNewMsg { get; set; }

    public string? Picture { get; set; }

    public string? Position { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public string? Tel1 { get; set; }

    public string? Tel2 { get; set; }

    public string? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? EnbMenuFlt { get; set; }

    public string? ObjType { get; set; }

    public int? LogInstanc { get; set; }

    public short? UserSign { get; set; }

    public DateTime? CreateDate { get; set; }

    public short? UserSign2 { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? OneLogPwd { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? LastPwds { get; set; }

    public string? LastPwds2 { get; set; }

    public DateTime? LastPwdSet { get; set; }

    public int? FailedLog { get; set; }

    public string? PwdNeverEx { get; set; }

    public decimal? SalesDisc { get; set; }

    public decimal? PurchDisc { get; set; }

    public DateTime? LstLogoutD { get; set; }

    public int? LstLoginT { get; set; }

    public int? LstLogoutT { get; set; }

    public int? LstPwdChT { get; set; }

    public string? LstPwdChB { get; set; }

    public string? RclFlag { get; set; }

    public string? MobileUser { get; set; }

    public string? MobileImei { get; set; }

    public string? PrsWkCntEb { get; set; }

    public int? SnapShotId { get; set; }

    public string? Stdata { get; set; }

    public string? SupportUsr { get; set; }

    public short? NoStpwdNum { get; set; }

    public string? DomainUser { get; set; }

    public string? Cusagree { get; set; }

    public string? EmailSig { get; set; }

    public short? Tplid { get; set; }

    public string? DigCrtPath { get; set; }

    public string? ShowNewTsk { get; set; }

    public string? IntgrtEb { get; set; }

    public string? AllBrnchF { get; set; }

    public string? EvtNotify { get; set; }

    public string? IgnDtOwn { get; set; }

    public string? EnterAsTab { get; set; }

    public string? DotAsSep { get; set; }

    public string? MouseOnly { get; set; }

    public short? Color { get; set; }

    public string? SkinType { get; set; }

    public string? Font { get; set; }

    public int? FontSize { get; set; }

    public string? NaturalPer { get; set; }

    public string? Dppstatus { get; set; }

    public string? AutoAsnBpl { get; set; }

    public string? EncryptIv { get; set; }

    public string? HandleEdoc { get; set; }

    public string? ShowLicBal { get; set; }

    public DateTime? LicBaHdate { get; set; }

    public string? ShowFstMsg { get; set; }

    public int? ShowMsgNum { get; set; }

    public DateTime? LastClean { get; set; }

    public string? CleanChb { get; set; }

    public string? CleanIbx { get; set; }

    public string? CleanObx { get; set; }

    public string? CleanSnt { get; set; }
}
