using System.ComponentModel.DataAnnotations;

namespace NSSERP.Areas.Masters.Models
{
    public class Countrys
    {
        public string? msg { get; set; }
        public IEnumerable<Countrys> masterDetails { get; set; }   
        public decimal? Country_Code { get; set; }
        public string? Country_Name { get; set; }
        public decimal? Country_CallCode { get; set; }
        public string? Status { get; set; }
        public string? Data_Flag { get; set; } = "GANGOTRI";
        public string? CreatedBy { get; set; }
    }
}
