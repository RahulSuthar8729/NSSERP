using NSSERP.Areas.Masters.Models;
using static NSSERP.Areas.NationalGangotri.Models.BackOfficeModel;
using static NSSERP.Areas.NationalGangotri.Models.DonorMaster;

namespace NSSERP.Areas.ReceiptBook.Models
{
    public class PersonMaster
    {
        public IEnumerable<PersonMaster> masterDetails { get; set; }
        public List<IFormFile> DocUpload { get; set; }
        public string? Docs { get; set; } = "";
        public string? SelectedDocumentType { get; set; } = "";
        public string? msg { get; set; }
        public int? person_id { get; set; }
        public string? person_name { get; set; }
        public string? address { get; set; }
        public string? designation { get; set; }
        public string? remarks { get; set; }
        public string? user_name { get; set; }
        public string? phone_no { get; set; }
        public string? person_from { get; set; }
        public DateTime? doe { get; set; }
        public int? category_code { get; set; }
        public int? emp_num { get; set; }
        public string? status { get; set; }
        public string? emailid { get; set; }
        public string? other { get; set; }
        public DateTime? from_doe { get; set; }
        public DateTime? to_doe { get; set; }
        public int? city { get; set; }
        public int? file_count { get; set; }
        public string scan_files { get; set; }
        public int? ngcode { get; set; }
        public string? phresi { get; set; }
        public string? phoffice { get; set; }
        public int? bus_type { get; set; }
        public string? bus_in { get; set; }
        public DateTime? status_doe { get; set; }
        public int? whatsappno { get; set; }
        public string? website { get; set; }
        public string? proffession { get; set; }
        public string? desg { get; set; }
        public string? address2 { get; set; }
        public string? address3 { get; set; }
        public int? country { get; set; }
        public int? state { get; set; }
        public int? district { get; set; }
        public int? pincode { get; set; }
        public string? data_flag { get; set; } = "GANGOTRI";
        public int? fy_id { get; set; } = 0;
        public int? last_call_id { get; set; }
        public DateTime? active_status_doe { get; set; }
        public List<EmployeMaster> EmployeeDetils { get; set; }
        public List<PersonCatMaster> PersonCatDetails { get; set; }
        public IEnumerable<BussinessType> bussinessTypesList { get; set; }
        public List<Countrys> CountryList { get; set; }
        public IEnumerable<Citys> CityList { get; set; }
    }
}
