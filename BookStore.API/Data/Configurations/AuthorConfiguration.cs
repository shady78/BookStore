using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BookStore.API.Data.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> modelBuilder)
        {

            modelBuilder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Property(a => a.Bio)
                .HasMaxLength(1000);

        }
    }
}
