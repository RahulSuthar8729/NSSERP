namespace NSSERPAPI.Areas.Masters.Models
{
    public class PurposeYojanaMaster
    {
        public int? Yojna_ID { get; set; }
        public int? Purpose_ID { get; set; }
        public string? Purpose { get; set; }
        public string? Yojna { get; set; }
        public decimal? Yojna_Amount { get; set; }
        public char? Status { get; set; }
        public int? Ord { get; set; }
        public int? Qty { get; set; }
        public int? Start_Limit { get; set; }
        public int? Ord_Yojna { get; set; }
        public string? DATA_FLAG { get; set; }
        public int? FY_ID { get; set; }
        public int? Donor_Count { get; set; }
        public int? Ref_No { get; set; }
        public char? FOR_OPERATION { get; set; }
        public char? OPERATION_PHOTO { get; set; }
        public char? IsNamePlate { get; set; }
    }
}
