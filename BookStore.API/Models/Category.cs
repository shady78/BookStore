using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Models
{
    public class Category
    {
        //[Key]
        public int Id { get; set; }
        //[Required]
        //[MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<BookCategory> BookCategories { get; set; } =
        new List<BookCategory>();
    }
}
