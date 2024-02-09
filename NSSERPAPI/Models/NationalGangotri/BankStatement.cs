namespace NSSERPAPI.Models.NationalGangotri
{
    public class BankStatement
    {
        public int? ReceiveID { get; set; }
        public int? BANK_Code { get; set; }
        public int? BankID { get; set; }
        public DateTime? DOE { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Bank_Name { get; set; }
        public string? Particular { get; set; }
        public string? ChqNo { get; set; }
        public decimal? DR { get; set; }
        public decimal? CR { get; set; }
        public decimal? BALANCE { get; set; }
        public decimal? Curr_Rate { get; set; }
        public string? Branch { get; set; }
        public string? MobileNo { get; set; }
        public string? DataFlag { get; set; }
        public decimal? ReceiveAmt { get; set; }
    }
}
