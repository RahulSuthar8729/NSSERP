namespace NSSERP.Areas.NationalGangotri.Models
{
    public class SubjectMaster
    {
        public IEnumerable<SubjectMaster> masterDetails { get; set; }
        public string? msg { get; set; }
        public int? subject_id { get; set; }
        public string? subject { get; set; }
        public string? data_flag { get; set; } = "GANGOTRI";
        public int? fy_id { get; set; } = 0;
    }
}
