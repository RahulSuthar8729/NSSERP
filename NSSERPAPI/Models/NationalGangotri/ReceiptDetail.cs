namespace NSSERPAPI.Models.NationalGangotri
{
    public class ReceiptDetail
    {
        public int REF_ID { get; set; }
        public int REF_NO { get; set; }
        public string? Campaign { get; set; }
        public int? HeadID { get; set; }
        public string? Head { get; set; }
        public int? SubHeadID { get; set; }
        public string? SubHeadName { get; set; }
        public string? Purpose { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public decimal? ReceiveAmount { get; set; }
        public decimal? AnnounceAmount { get; set; }
        public string? CreatedBy { get; set; }
        public string? HeadName { get; set; }

    }
}
