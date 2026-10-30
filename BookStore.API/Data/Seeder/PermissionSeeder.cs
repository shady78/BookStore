using BookStore.API.Models.Permissions;

namespace BookStore.API.Data.Seeder
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(BookStoreDbContext context)
        {
            var existingPermissions = await context.Permissions.ToListAsync();
            foreach (var permissionDefinition in SystemPermissions.All)
            {
                var existingPermission = existingPermissions
                    .FirstOrDefault(permission => permission.Name == permissionDefinition.Name);
                if (existingPermission is null)
                {
                    context.Permissions.Add(new Permission
                    {
                        Name = permissionDefinition.Name,
                        Description = permissionDefinition.Description,
                        Module = permissionDefinition.Module
                    });
                    continue;
                }

                existingPermission.Description = permissionDefinition.Description;
                existingPermission.Module = permissionDefinition.Module;
            }
            await context.SaveChangesAsync();
        }
    }
}
