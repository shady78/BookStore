namespace BookStore.API.Configuration
{
    public class BookStoreSettings
    {
        public int MaxPageSize { get; set; }
        public int DefaultPageSize { get; set; }
        public string StoreName { get; set; } = string.Empty;
    }
}
