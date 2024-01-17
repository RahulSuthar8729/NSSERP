namespace NSSERP.Areas.NationalGangotri.Models
{
    public class DepositeDetailsModel
    {
        public int DepositePaymentMode { get; set; }
        public DateTime DepositeDate { get; set; }
        public int? DepositeCurrencyID { get; set; }
        public decimal? DepositeAmount { get; set; }
        public string? DepositeBankID { get; set; }
    }
}
