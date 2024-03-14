namespace NSSERPAPI.Areas.ReceiptBook.Models
{
    public class SeriesMaster
    {
        public int? Series_Code { get; set; }
        public DateTime? Date_Print { get; set; }
        public int? Series_From { get; set; }
        public int? Series_To { get; set; }
        public string? Type { get; set; }
        public int? Total_Receipt_Book { get; set; }
        public string? Data_Flag { get; set; }
    }
}
