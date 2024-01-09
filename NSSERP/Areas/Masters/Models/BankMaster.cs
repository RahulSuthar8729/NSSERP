using System.ComponentModel.DataAnnotations;

namespace NSSERP.Areas.Masters.Models
{
    public class BankMaster
    {
        public int BankID { get; set; }
        public string CountryName { get; set; }

        public string BankName { get; set; }

     
        public string BranchName { get; set; }

      
        public string IFSCCode { get; set; }

    
        public int CountryID { get; set; }


        public string IsActive { get; set; }


        public string CreatedBy { get; set; }

       
        public Countrys Countrys { get; set; }
    }
}
