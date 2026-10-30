namespace BookStore.API.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}