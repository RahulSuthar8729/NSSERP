namespace NSSERP.Areas.Masters.Models
{
    public class DistrictMaster
    {
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public int? CityID { get; set; }
        public int? StateID { get; set; }
        public string StateName { get; set; }
        public int? CountryID { get; set; }
        public string CountryName { get; set; }
        public char IsActive { get; set; }       
        public string CreatedBy { get; set; }
        public List<Countrys> Countrys { get; set; }
        public List<StateMaster> States { get; set; }
        public List<CityMaster> Citys { get; set; }

    }
}
