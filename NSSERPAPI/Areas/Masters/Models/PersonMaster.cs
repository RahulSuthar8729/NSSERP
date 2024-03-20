namespace NSSERPAPI.Areas.Masters.Models
{
    public class PersonMaster
    {
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

    }
}
