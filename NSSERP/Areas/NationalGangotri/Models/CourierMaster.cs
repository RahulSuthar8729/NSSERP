using System.ComponentModel.DataAnnotations;

namespace NSSERP.Areas.NationalGangotri.Models
{
    public class CourierMaster
    {
        public IEnumerable<CourierMaster> masterDetails { get; set; }
        public string? msg { get; set; }
        public int? Courier_Id { get; set; }

        
        public string? Courier_Name { get; set; }

      
        public string? Courier_Address { get; set; }

   
        public string? Courier_Phone { get; set; }

        
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Courier mobile must be a 10-digit number.")]
        public string? Courier_Mobile { get; set; }

        
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Courier_Email { get; set; }

        [StringLength(15, ErrorMessage = "Courier fax must not exceed 15 characters.")]
        public string? Courier_Fax { get; set; }

      
        public string? Courier_Person { get; set; }

        
        public string? C_Address { get; set; }
      
        public string? C_Phone { get; set; }

       
        [RegularExpression(@"^\d{10}$", ErrorMessage = "C mobile must be a 10-digit number.")]
        public string? C_Mobile { get; set; }

       
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? C_Email { get; set; }

        [StringLength(15, ErrorMessage = "C fax must not exceed 15 characters.")]
        public string? C_Fax { get; set; }

        [StringLength(200, ErrorMessage = "C person must not exceed 200 characters.")]
        public string? C_Person { get; set; }

       
        [StringLength(200, ErrorMessage = "C state code must not exceed 200 characters.")]
        public string? C_State_Code { get; set; }
     
        public string? Data_Flag { get; set; } = "GANGOTRI";

        public int?   Fy_Id { get; set; } = 0;

       
        public bool? C_Active { get; set; }

        [StringLength(350, ErrorMessage = "Tracking link must not exceed 350 characters.")]
        public string? Tracking_Link { get; set; }
    }
}
