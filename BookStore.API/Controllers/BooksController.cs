using BookStore.API.DTOs;
using BookStore.API.Models;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")] // api/books
    [ApiController] // Automatic model validation
    // FluentValidation   
    // Filter 
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> Books = new()
          {
              new Book{Id = 1 , Title="Clean Code", Author = "Robert C.Marten",
              Price = 250.00m, StockQuantity= 12}
          };
        private static int _nextId = 2;
        // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<BookResponse>> GetAll()
        {
            var response = Books.Select(MapToResponse);
            return Ok(response);
        }

        // GET api/books/1
        [HttpGet("{id:int}")]
        public ActionResult<BookResponse> Get(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book is null)
            {
                return NotFound($"Book with id {id} was not found.");
            }
            return Ok(MapToResponse(book));
        }

        // Post api/books
        [HttpPost]
        public ActionResult<BookResponse> Create(CreateBookRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Title is required.");
            }
            if (request.Price <= 0)
            {
                return BadRequest(new { message = "Price must be greater than zero." });
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

            var response = MapToResponse(book);

            // data , api/book/id , get
            return CreatedAtAction(
                actionName: nameof(Get),
                routeValues: new { id = book.Id },
               value: response);
        }

        // PUT api/book/1
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateBookRequest request)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book is null)
                return NotFound();
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Title is required.");
            }
            if (request.Price <= 0)
            {
                return BadRequest(new { message = "Price must be greater than zero." });
            }
            book.Title = request.Title;
            book.Author = request.Author;
            book.Price = request.Price;
            book.StockQuantity = request.StockQuantity;

            return NoContent();
        }


        // Delete api/book/1
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book is null)
            {
                return NotFound();
            }
            Books.Remove(book);
            return NoContent();
        }

        // helper method 
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

/*
   private readonly List<Book> Books;

        public BooksController(List<Book> books)
        {
            Books = books;
        }

        //[HttpGet] // api/books
        //public IActionResult GetAll()
        //{
        //    return Ok(Books);
        //}

        //// Attribute routing 
        //[HttpGet("{id:int:min(1)}/reviews")] // api/books/5/reviews
        //[HttpGet("{str:length(13)}")]
        //[HttpGet("{title:alpha}")]
        //[HttpGet("{title:regex()}")]
        //public IActionResult Get(int id) { return Ok(); }
        //Route template

        // Model Binding
        //[HttpGet("{id:int}")]
        //public IActionResult GetById([FromRoute] int id)
        //{
        //    // api/books/5
        //}
        //[HttpGet]
        //public IActionResult GetAll([FromQuery] string? category, [FromQuery] int page = 1)
        //{
        //    // query string
        //    // Get api/books?category=fic&page=2
        //}

        //[HttpPost]
        //public IActionResult Create([FromBody] object request)
        //{

        //}

        //[HttpGet]
        //public IActionResult GetAll([FromHeader(Name = "X-Client-Version")]
        //string? clientVersion)
        //{

        //}



        // IActionResult

        //[HttpGet("{id:int}")]
        //public IActionResult GetById(int id)
        //{
        //    // var book = _books.FirstOrDefault(x => x.id == id);
        //    //if (book  is null)
        //    //{
        //    // return NotFound();
        //    //}
        //    return Ok(book);
        //}

        // ActionResult<T>
        [HttpGet("{id:int}")]
        public ActionResult<Book> GetById(int id)
        {
            var book = Books.FirstOrDefault(x => x.Id == id);
            if (book is null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public ActionResult<BookResponse> Create(CreateBookRequest request)
        {
            if (request.Price <=0)
            {
                return BadRequest(new { message = "Price must be greater than zero." });
            }
            var book = new Book()
            {
                Id = 1,
                Title = request.Title,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Author = request.Author
            };

            Books.Add(book);

            var response = MapToResponse(book);

            // data , api/book/id , get
            return CreatedAtAction(
                actionName: nameof(GetById),
                routeValues: new { id = book.Id },
               value: response);
        }
        private static BookResponse MapToResponse(Book book) => new()
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            StockQuantity = book.StockQuantity,
            Price = book.Price,
        };
 */