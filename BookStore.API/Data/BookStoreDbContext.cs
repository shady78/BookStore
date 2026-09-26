using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<BookCategory> BookCategory => Set<BookCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Author
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreDbContext).Assembly);

            // Task make All Configuraiton to implement IEntityTypeConfiguration
            //// Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(150);

                entity.HasIndex(c => c.Name)
                .IsUnique();
            });

            //// Book 
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(300);

                entity.Property(b => b.Price)
                .HasPrecision(18, 2);

                entity.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //// BookCategory (join entity)
            modelBuilder.Entity<BookCategory>(entity =>
            {
                entity.HasKey(bc => new { bc.BookId, bc.CategoryId });

                entity.HasOne(b => b.Book)
               .WithMany(a => a.BookCategories)
               .HasForeignKey(b => b.BookId)
               .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Category)
               .WithMany(a => a.BookCategories)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

            });


            // Seed Data 
            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Bio = "Author of clean code",
                    Name = "Robert " +
                "C marten"
                },
                new Author
                {
                    Id = 2,
                    Bio = "Co-author of the progrmatic Programmer.",
                    Name = "Aundy hunt "
                });

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name="Software Engineering."},
                new Category { Id = 2, Name="Best sellers."});

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Clean Code",
                    Price = 250.00m,
                    StockQuantity = 12,
                    AuthorId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "The Programtic Programmer",
                    Price = 250.00m,
                    StockQuantity = 12,
                    AuthorId = 2
                });

            modelBuilder.Entity<BookCategory>().HasData(
                new BookCategory { BookId = 1, CategoryId = 1 },
                new BookCategory { BookId = 1, CategoryId = 2 },
                new BookCategory { BookId = 2, CategoryId = 1 });
            base.OnModelCreating(modelBuilder);
        }
    }
}
