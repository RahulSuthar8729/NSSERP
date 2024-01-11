namespace NSSERP.Areas.NationalGangotri.Models
{
    public class ProvisionalReceiptModel
    {
        public int ReceiveID { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string NamePrefix { get; set; }
        public string FullName { get; set; }
        public string PrefixToFullName { get; set; }
        public string RelationToFullName { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentModeName { get; set; }
        public string Purpose {  get; set; }
        public string FullAddress { get; set; }
        public int PinCode { get; set; }
        public string MobileNo { get; set; }

    }
}
