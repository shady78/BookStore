using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
