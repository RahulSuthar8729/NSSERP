namespace NSSERP.Areas.NationalGangotri.Models
{
    public class PostTypeMaster
    {
        public IEnumerable<PostTypeMaster> masterDetails { get; set; }
        public string? msg { get; set; }
        public int? Post_Type_ID { get; set; }
        public string? Post_Type_Name { get; set; }
        public string? Post_type_Mobile { get; set; }
        public string? Tracking_Link { get; set; }
        public string? DATA_FLAG { get; set; } = "GANGOTRI";
        public int? FY_ID { get; set; } = 0;


    }
}
