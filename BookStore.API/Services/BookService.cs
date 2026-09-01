using BookStore.API.Intefaces;

namespace BookStore.API.Services
{
    public class BookService : IBookService
    {
        private static readonly List<Book> Books = new()
          {
              new Book{Id = 1 , Title="Clean Code", Author = "Robert C.Marten",
              Price = 250.00m, StockQuantity= 12}
          };
        private static int _nextId = 2;

        public IEnumerable<BookResponse> GetAll()
        {
            return Books.Select(MapToResponse);
        }

        public ServiceResult<BookResponse> GetById(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            return book is null ?
                ServiceResult<BookResponse>.Fail(errorMessage: "Book not found") :
                ServiceResult<BookResponse>.Ok(MapToResponse(book));
        }
        public ServiceResult<BookResponse> Create(CreateBookRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return ServiceResult<BookResponse>.Fail(errorMessage: "Title is required.");
            }
            if (request.Price <= 0)
            {
                return ServiceResult<BookResponse>.Fail(errorMessage: "Price must be greater than zero.");
            }
            var book = new Book()
            {
                Id = _nextId,
                Title = request.Title,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Author = request.Author
            };
            Books.Add(book);

            return ServiceResult<BookResponse>.Ok(MapToResponse(book));
        }

        public ServiceResult<bool> Delete(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book is null)
                ServiceResult<BookResponse>.Fail(errorMessage: "Book not found");

            Books.Remove(book!);
            return ServiceResult<bool>.Ok(true);
        }


        public ServiceResult<bool> Update(int id, UpdateBookRequest request)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book is null)
                return ServiceResult<bool>.Fail(errorMessage: "Book not found");

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return ServiceResult<bool>.Fail(errorMessage:"Title is required.");
            }
            if (request.Price <= 0)
            {
                return ServiceResult<bool>.Fail(errorMessage: "Price must be greater than zero.");
            }
            book.Title = request.Title;
            book.Author = request.Author;
            book.Price = request.Price;
            book.StockQuantity = request.StockQuantity;

            return ServiceResult<bool>.Ok(true);
        }

        private static BookResponse MapToResponse(Book book) => new()
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            StockQuantity = book.StockQuantity,
            Price = book.Price,
        };
    }
}
