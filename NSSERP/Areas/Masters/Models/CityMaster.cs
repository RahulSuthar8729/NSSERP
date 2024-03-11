namespace NSSERP.Areas.Masters.Models
{
    public class CityMaster
    {
        public string? msg { get; set; }
        public IEnumerable<CityMaster> masterDetails { get; set; }

        public int? City_Code { get; set; }
        public string? City_Name { get; set; }
        public string? CreatedBy { get; set; }
        
        public string? Data_Flag { get; set; } = "GANGOTRI";

    }
}
