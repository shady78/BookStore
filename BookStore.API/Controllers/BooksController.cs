using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")] // api/books
    [ApiController] // Automatic model validation
    // FluentValidation   
    // Filter 
    public class BooksController : ControllerBase
    {
        private static readonly List<object> Books = new()
        {
            new {Id=1, Title = "Clean Code", Price = 250},
            new {Id = 2 , Title ="OOP", Price=300}
        };

        [HttpGet] // api/books
        public IActionResult GetAll()
        {
            return Ok(Books);
        }
    }
}
