namespace NSSERP.Areas.ReceiptBook.Models
{
    public class ReceiptbookCancellationDetails
    {
        public IEnumerable<ReceiptbookCancellationDetails> masterDetails { get; set; }
        public int? receipt_book_no { get; set; }
        public char? PostChq { get; set; }
        public int? PersonId { get; set; }
        public string? DataFlag { get; set; }
        public string? msg { get; set; }
        public string? TP { get; set; }
        public string? receiptbook_type { get; set; }
        public int? receipt_from { get; set; }
        public int? receipt_to { get; set; }
        public DateTime? date_print { get; set; }
    }
}
