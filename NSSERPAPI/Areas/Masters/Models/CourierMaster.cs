using System.ComponentModel.DataAnnotations;

namespace NSSERPAPI.Areas.Masters.Models
{
    public class CourierMaster
    {
        public int? courier_id { get; set; }
        public string? courier_name { get; set; }
        public string? courier_address { get; set; }
        public string? courier_phone { get; set; }      
        public string? courier_mobile { get; set; }       
        public string? courier_email { get; set; }        
        public string? courier_fax { get; set; }
        public string? courier_person { get; set; }
        public string? c_address { get; set; }
        public string? c_phone { get; set; }       
        public string? c_mobile { get; set; }        
        public string? c_email { get; set; }        
        public string? c_fax { get; set; }      
        public string? c_person { get; set; }       
        public string? c_state_code { get; set; }
        public string? data_flag { get; set; }
        public int? fy_id { get; set; }
        public bool? c_active { get; set; }        
        public string? tracking_link { get; set; }
    }
}
