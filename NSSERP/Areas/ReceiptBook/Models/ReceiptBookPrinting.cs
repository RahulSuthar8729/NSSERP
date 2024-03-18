using static NSSERP.Areas.NationalGangotri.Models.DonationReceiveMaster;

namespace NSSERP.Areas.ReceiptBook.Models
{
    public class ReceiptBookPrinting
    {
        public IEnumerable<ReceiptBookPrinting> masterDetails { get; set; }
        public List<ReceiptBookType> ReceiptBookTypeList { get; set; }
        public string? msg { get; set; }        
        public string? data_flag { get; set; } = "GANGOTRI";
        public int? fy_id { get; set; } = 0;
        public int? receipt_book_no { get; set; }
        public int? receipt_from { get; set; }
        public int? receipt_to { get; set; }
        public DateTime? date_print { get; set; }
        public int? person_id { get; set; }
        public string? receiptbook_type { get; set; }
        public string? user_name { get; set; }
        public int? total_receipt { get; set; }
        public int? balance_receipt { get; set; }
        public int? total_receipt_used { get; set; }
        public int? total_receipt_cancel { get; set; }
        public int? balance_from { get; set; }
        public int? balance_to { get; set; }
        public string? tp { get; set; }
        public int? user_id { get; set; }
        public int? p_book_rrs_no { get; set; }
        public char? post_chq { get; set; }
        public bool? blocked { get; set; }
        public string? block_reason { get; set; }
        public int? blocked_by_id { get; set; }
        public string? blocked_byname { get; set; }
        public DateTime? blocked_date { get; set; }

    }
}
