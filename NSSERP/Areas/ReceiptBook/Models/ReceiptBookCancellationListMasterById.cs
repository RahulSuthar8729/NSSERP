using static NSSERP.Areas.NationalGangotri.Models.BackOfficeModel;

namespace NSSERP.Areas.ReceiptBook.Models
{
    public class ReceiptBookCancellationListMasterById
    {
        public int? PROVD_AUTOID { get; set; }
        public int? Book_Id { get; set; }
        public string? TP { get; set; }
        public string? Receipt_No { get; set; }
        public string? Don_Receipt_No { get; set; }
        public string? CancelFlag { get; set; }
        public string? NGCODE { get; set; }
        public decimal? Amount { get; set; }
        public string? Cancel_Reason { get; set; }
        public string? UsedIn { get; set; }
        public string? Miss_By_sadhak { get; set; }
        public string? Miss_Remark { get; set; }
        public string? Miss_EntryBy { get; set; }
        public DateTime? Miss_EntryDate { get; set; }
        public string? Miss_EntryTime { get; set; }
        public string? Block { get; set; }
        public string? DATA_FLAG { get; set; }
        public int? FY_ID { get; set; }
        public string? Cancel_Remark { get; set; }
        public int? Cancel_Remark_Id { get; set; }
        public int? Cancel_SubRemark_Id { get; set; }
        public string? Sadhak { get; set; }
        
    }
}
