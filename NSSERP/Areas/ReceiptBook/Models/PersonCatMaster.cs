namespace NSSERP.Areas.ReceiptBook.Models
{
    public class PersonCatMaster
    {
        public IEnumerable<PersonCatMaster> masterDetails { get; set; }
        public string? msg { get; set; }
        public int? code { get; set; }
        public string? name { get; set; }
        public string? data_flag { get; set; } = "GANGOTRI";
        public int? fy_id { get; set; } = 0;
    }
}
