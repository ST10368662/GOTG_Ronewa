using Microsoft.AspNetCore.Identity;
namespace GOTG.Ronewa.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Organisation { get; set; }
    }
}
