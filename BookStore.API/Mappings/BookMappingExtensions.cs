namespace BookStore.API.Mappings
{
    public static class BookMappingExtensions
    {
        public static BookResponse ToRespose(this Book book)
        {
            return new BookResponse
            {
                Id = book.Id,
                Author = book.Author,
                Title = book.Title,
                Price = book.Price,
                StockQuantity = book.StockQuantity
            };
        }
        public static IEnumerable<BookResponse> ToResponseList(this IEnumerable<Book> books)
        {
            return books.Select(book => ToRespose(book));
        }

        public static Book ToEntity(this CreateBookRequest request)
        {
            return new Book
            {
                Title = request.Title,
                Author = request.Author,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };
        }
        public static void ApplyUpdate(this Book book, UpdateBookRequest request)
        {
            book.Title = request.Title;
            book.Author = request.Author;
            book.Price = request.Price;
            book.StockQuantity = request.StockQuantity;
        }
    }
}
