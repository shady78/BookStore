using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.API.Data.Configurations
{
    public class PermissionsConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> modelBuilder)
        {
            modelBuilder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Property(a => a.Description)
                .HasMaxLength(1000);

            modelBuilder.Property(p => p.Module)
                .IsRequired()
                .HasMaxLength (100);

            modelBuilder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
