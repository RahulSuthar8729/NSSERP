namespace NSSERP.Areas.Masters.Models
{
    public class CityMaster
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int? StateID { get; set; }
        public int? CountryID { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public char IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public List<Countrys> Countries { get; set; }
        public List<StateMaster> States { get; set; }
        public List<DistrictMaster> Districts { get; set; }
        public IEnumerable<Countrys> CountriesList { get; set; }
        public IEnumerable<StateMaster> StatesList { get; set; }
     
    }
}
