using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NSSERP.Models
{
    public class SelectCountry
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    

    }
}
