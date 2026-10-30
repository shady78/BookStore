namespace BookStore.API.Models
{
    public class RolePermission
    {
        public string RoleId { get; set; } = string.Empty;
        public ApplicationRole Role { get; set; } = default!;
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = default!;
    }
}
