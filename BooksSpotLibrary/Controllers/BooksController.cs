using BooksSpotLibrary.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksSpotLibrary.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksContext context;
        public BooksController()
        {
            this.context = new BooksContext();
        }

        [HttpGet("Search")]
        public IEnumerable<BookEntity> Search()
        {
            return this.context.Books.ToList();
        }
    }
}
