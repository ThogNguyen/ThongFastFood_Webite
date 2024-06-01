using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string? FullName { get; set; }
        
        [StringLength(100)]
        public string? Address { get; set; }
    }
}
