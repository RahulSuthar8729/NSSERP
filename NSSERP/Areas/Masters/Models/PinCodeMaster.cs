namespace NSSERP.Areas.Masters.Models
{
    public class PinCodeMaster
    {
        public int PincodeID { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string CountryCode { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string Pincode { get; set; }
        public int CityID { get; set; }

        public string CityName { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public List<CityMaster> citys { get; set; }

    }
}
