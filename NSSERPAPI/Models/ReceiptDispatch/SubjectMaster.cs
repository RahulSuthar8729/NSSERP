namespace NSSERPAPI.Models.ReceiptDispatch
{
    public class SubjectMaster
    {
        public int? subject_id { get; set; }
        public string? subject { get; set; }
        public string? data_flag { get; set; }
        public int? fy_id { get; set; } = 0;
    }
}
