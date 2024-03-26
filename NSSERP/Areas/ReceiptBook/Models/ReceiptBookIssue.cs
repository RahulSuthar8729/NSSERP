using static NSSERP.Areas.NationalGangotri.Models.DonationReceiveMaster;

namespace NSSERP.Areas.ReceiptBook.Models
{
    public class ReceiptBookIssue
    {
        public IEnumerable<ReceiptBookIssue> masterDetails { get; set; }
        public List<ReceiptBookType> ReceiptBookTypeList { get; set; }


        public string? msg { get; set; }
        public string? SubBy { get; set; }
        public string? ChkSub { get; set; }
        public List<IFormFile> DocUpload { get; set; }
        public string? Docs { get; set; } = "";       
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
        public string? DataFlag { get; set; } = "GANGOTRI";
        public int? FYId { get; set; } = 0;
        public List<PersonMaster> PersonDetails { get; set; }
        public IEnumerable<EProgramMaster> ReceiveInEventList { get; set; }
        public IEnumerable<Citys> CityList { get; set; }
    }
}
