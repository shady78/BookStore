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
        public async Task<ActionResult<IEnumerable<BookResponse>>> GetAll(CancellationToken cancellation)
        {
            return Ok(await _bookService.GetAllAsync(cancellation));
        }

        // GET api/books/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookResponse>> Get(int id, CancellationToken cancellation)
        {
            var book = await _bookService.GetByIdAsync(id, cancellation);
            return book.Success ?
                Ok(book.Data)
                : NotFound(new { message = book.ErrorMessage });
        }

        // Post api/books
        [HttpPost]
        public async Task<ActionResult<BookResponse>> Create(CreateBookRequest request,
            CancellationToken cancellation)
        {
            var result = await _bookService.CreateAsync(request, cancellation);
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookRequest request,
            CancellationToken cancellation)
        {
            var result = await _bookService.UpdateAsync(id, request, cancellation);
            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return NoContent();
        }


        // Delete api/book/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            var result = await _bookService.DeleteAsync(id, cancellation);
            return result.Success
                ? NoContent() : NotFound(new { message = result.ErrorMessage });
        }


    }
}