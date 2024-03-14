using static NSSERP.Areas.NationalGangotri.Models.DonationReceiveMaster;

namespace NSSERP.Areas.ReceiptBook.Models
{
    public class SeriesMaster
    {
        public IEnumerable<SeriesMaster> masterDetails { get; set; }
        public string? msg { get; set; }
        public int? Series_Code { get; set; }
        public DateTime? Date_Print { get; set; }
        public int? Series_From { get; set; }
        public int? Series_To { get; set; }
        public string? Type { get; set; }
        public int? Total_Receipt_Book { get; set; }
        public string? Data_Flag { get; set; } = "GANGOTRI";
        public List<ReceiptBookType> ReceiptBookTypeList { get; set; }
     
    }
}
