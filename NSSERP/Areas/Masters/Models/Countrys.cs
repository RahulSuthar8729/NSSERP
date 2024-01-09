using System.ComponentModel.DataAnnotations;

namespace NSSERP.Areas.Masters.Models
{
    public class Countrys
    {
        [Key]
        public int CountryId { get; set; }
        public string? CountryName { get; set; }

        public string? CountryCode { get; set; }

        public char IsActive { get; set; }
        public string CreatedBy { get; set; }
    }
}
