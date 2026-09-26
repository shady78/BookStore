using BookStore.API.Common;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var authors = await _authorService.GetAllAsync(cancellation);
            return Ok(ApiResponse<IEnumerable<AuthorResponse>>.Success(authors));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellation)
        {
            var author = await _authorService.GetByIdAsync(id, cancellation);
            return Ok(ApiResponse<AuthorResponse>.Success(author));
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] CreateAuthorRequest request, CancellationToken cancellation)
        {
            var author = await _authorService.CreateAsync(request, cancellation);
            return CreatedAtAction(
                nameof(Get),
                new { id = author.Id },
                ApiResponse<AuthorResponse>.Success(author));
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id ,
            [FromBody] UpdateAuthorRequest request, CancellationToken cancellation)
        {
            await _authorService.UpdateAsync(id, request, cancellation);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellation)
        {
            await _authorService.DeleteAsync(id, cancellation);
            return NoContent();
        }
    }
}
