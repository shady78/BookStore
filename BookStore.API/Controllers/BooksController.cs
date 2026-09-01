using BookStore.API.Intefaces;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")] // api/books
    [ApiController] // Automatic model validation
    // FluentValidation   
    // Filter 
    public class BooksController(IBookService _bookService) : ControllerBase
    {

        // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<BookResponse>> GetAll()
        {
            return Ok(_bookService.GetAll());
        }

        // GET api/books/1
        [HttpGet("{id:int}")]
        public ActionResult<BookResponse> Get(int id)
        {
            var book = _bookService.GetById(id);
            return book.Success ?
                Ok(book.Data)
                : NotFound(new { message = book.ErrorMessage });
        }

        // Post api/books
        [HttpPost]
        public ActionResult<BookResponse> Create(CreateBookRequest request)
        {
           var result = _bookService.Create(request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            // data , api/book/id , get
            return CreatedAtAction(
                actionName: nameof(Get),
                routeValues: new { id = result.Data!.Id },
               value: result.Data);
        }

        // PUT api/book/1
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateBookRequest request)
        {
            var result = _bookService.Update(id, request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return NoContent();
        }


        // Delete api/book/1
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result =_bookService.Delete(id);
            return result.Success 
                ? NoContent() : NotFound(new { message = result.ErrorMessage });
        }

     
    }
}