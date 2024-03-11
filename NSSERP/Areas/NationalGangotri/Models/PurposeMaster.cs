namespace NSSERP.Areas.NationalGangotri.Models
{
    public class PurposeMaster
    {
        public IEnumerable<PurposeMaster> masterDetails { get; set; }
        public string? msg { get; set; }
        public int? Purpose_id { get; set; }
        public string? Purpose { get; set; }
        public char? front_end { get; set; }
        public int? From_end_Order { get; set; }
        public string? Data_Flag { get; set; } = "GANGOTRI";
    }
}
