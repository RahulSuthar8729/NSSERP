using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSSERP.Areas.Masters.Models
{
    public class StateMaster
    {
        public string? msg { get; set; }
        public IEnumerable<StateMaster> masterDetails { get; set; }  
        public string? Short_Name { get; set; }
       
        public int? State_Code { get; set; }

        public string? State_Name { get; set; }

        public int? Country_Code { get; set; }
        public string? Country_Name { get; set; }

        public string? Data_FLag { get; set; } = "GANGOTRI";

    }
}
