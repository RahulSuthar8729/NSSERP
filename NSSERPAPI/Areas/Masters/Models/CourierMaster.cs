using System.ComponentModel.DataAnnotations;

namespace NSSERPAPI.Areas.Masters.Models
{
    public class CourierMaster
    {
        public int? Courier_Id { get; set; }
        public string? Courier_Name { get; set; }
        public string? Courier_Address { get; set; }
        public string? Courier_Phone { get; set; }      
        public string? Courier_Mobile { get; set; }       
        public string? Courier_Email { get; set; }        
        public string? Courier_Fax { get; set; }
        public string? Courier_Person { get; set; }
        public string? C_Address { get; set; }
        public string? C_Phone { get; set; }       
        public string? C_Mobile { get; set; }        
        public string? C_Email { get; set; }        
        public string? C_Fax { get; set; }      
        public string? C_Person { get; set; }       
        public string? C_State_Code { get; set; }
        public string? Data_Flag { get; set; }
        public int? Fy_Id { get; set; }
        public bool? C_Active { get; set; }        
        public string? Tracking_Link { get; set; }
    }
}
