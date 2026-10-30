namespace BookStore.API.Models.Permissions
{
    public record PermissionDefinition(
        string Name,
        string Description,
        string Module);
}
