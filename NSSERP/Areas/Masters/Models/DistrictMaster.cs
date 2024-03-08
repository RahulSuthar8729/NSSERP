using System.ComponentModel.DataAnnotations;

namespace NSSERP.Areas.Masters.Models
{
    public class DistrictMaster
    {
        public string? msg { get; set; }
        public IEnumerable<DistrictMaster> masterDetails { get; set; }  
        
        public int? State_Code { get; set; }
        public string? State_Name { get; set; }
        public int? Country_Code { get; set; }
        public string? Country_Name { get; set; }           
        public string? CreatedBy { get; set; }
        public List<Countrys> CountryList { get; set; }
        public IEnumerable<StateMaster> StateList { get; set; }        
        public int? District_Code { get; set; }    
        public string? District_Name { get; set; }
        public string? Data_Flag { get; set; } = "GANGOTRI";
    }
}
