namespace NSSERPAPI.Areas.ReceiptBook.Models
{
    public class ReceiptBookIssue
    {
        public string? Docs { get; set; }
        public int? ProvReceiptFrom { get; set; }
        public int? ProvReceiptTo { get; set; }
        public int? RRSNo { get; set; }
        public int? book_issue_no { get; set; }
        public int? PersonId { get; set; }
        public int? PersonCatCode { get; set; }
        public DateTime? SubmitDate { get; set; }
        public int? ReceiptBookNoFrom { get; set; }
        public int? ReceiptBookNoTo { get; set; }
        public string? IssueAuthority { get; set; }
        public int? ReceiptUsed { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? Submitted { get; set; }
        public string? TakenBy { get; set; }
        public string? UserName { get; set; }
        public string? Remark { get; set; }
        public string? BookHolderName { get; set; }
        public string? Event { get; set; }
        public string? EventName { get; set; }
        public string? EventPlace { get; set; }
        public DateTime? EventFDate { get; set; }
        public DateTime? EventTDate { get; set; }
        public int? EventId { get; set; }
        public int? RefPersonid { get; set; }
        public int? BookRRSNo { get; set; }
        public char? DeptSubmitt { get; set; }
        public string? ReceiptBookType { get; set; }
        public string? DataFlag { get; set; }
        public int? FYId { get; set; } = 0;

    }
}
