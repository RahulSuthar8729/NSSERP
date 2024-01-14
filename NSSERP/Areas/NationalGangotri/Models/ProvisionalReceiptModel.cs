namespace NSSERP.Areas.NationalGangotri.Models
{
    public class ProvisionalReceiptModel
    {
        public int ReceiveID { get; set; }
        public DateTime ReceiveDate { get; set; }
        public ProvisionalReceiptModel()
        {
            ReceiveDate = DateTime.MinValue;
        }
        public string NamePrefix { get; set; }
        public string FullName { get; set; }
        public string PrefixToFullName { get; set; }
        public string RelationToFullName { get; set; }
        public string TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentModeName { get; set; }
        public string Purpose { get; set; } // Represent as a string
        public List<string> PurposeString => Purpose?.Split(',').ToList(); // Convert to a list
        public string FullAddress { get; set; }
        public string PinCode { get; set; }
        public string MobileNo { get; set; }
    }
}
