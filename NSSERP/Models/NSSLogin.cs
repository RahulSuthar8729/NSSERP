using System.ComponentModel.DataAnnotations;

namespace NSSERP.Models
{
    public class NSSLogin
    {
        public string Username { get; set; }

        public String FullName { get; set; }

        public string Password { get; set; }

        [Display(Name = "Stay Signed In")]
        public bool KeepLoggedIn { get; set; }
    }
}
