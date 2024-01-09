using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSSERP.Areas.Masters.Models
{
    public class StateMaster
    {
        [Key]
        public int StateID { get; set; }

        
        public string StateName { get; set; }
        public char IsActive { get; set; }
        public string CreatedBy { get; set; }

       
        public int? CountryId { get; set; }

        public Countrys CountryMaster { get; set; }
    }
}
