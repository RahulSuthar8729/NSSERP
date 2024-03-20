namespace NSSERPAPI.Areas.ReceiptBook.Models
{
    public class ReceiptBookCancellationJson
    {
        public int? Book_Id { get; set; }
        public int? Cancel_Remark_Id { get; set; }
        public int? Sadhak { get; set; }
        public string? TP { get; set; }
        public char? cancelFlag { get; set; }
        public char? Block { get; set; }
        public int? PersonId { get; set; }
        public int? FY_ID { get; set; }
        public string? Data_Flag { get; set; }
        public string? Cancel_Reason { get; set; }
        public string? Cancel_Remark { get; set; }
    }
}
