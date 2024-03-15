namespace NSSERPAPI.Areas.ReceiptBook.Models
{
    public class ReceiptBookRRSMaster
    {
        public int? book_rrs_no { get; set; }
        public int? person_id { get; set; }
        public DateTime? rrs_date { get; set; }
        public int? receipt_book_no { get; set; }
        public int? receipt_used { get; set; }
        public string? receiptbook_type { get; set; }
        public string? issue_authority { get; set; }
        public string? user_name { get; set; }
        public int? user_id { get; set; }
        public string? remark { get; set; }
        public int? issue_hod { get; set; }
        public char? sanction_flg { get; set; }
        public string? remark_hod { get; set; }
        public int? sanction_user_id { get; set; }
        public string? sanction_user_name { get; set; }
        public string? dept_remark { get; set; }
        public char? discard { get; set; }
        public int? p_cat_code { get; set; }
        public char? type { get; set; }
        public int? @event { get; set; } 
        public int? dept_user_id { get; set; }
        public string? dept_user_name { get; set; }
        public int? old_pid { get; set; }
        public string? data_flag { get; set; }
        public int? fy_id { get; set; }
        public string? receipt_book_type { get; set; }
    }
}
