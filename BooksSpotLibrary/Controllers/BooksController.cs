using BooksSpotLibrary.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("Search/{category?}/{text?}")]
        public IEnumerable<BookEntity> Search(string category, string text)
        {
            if(text == null)
            {
                return this.context.Books.ToList();
            }
            if(category == "bytitle")
            {
                return this.context.Books.Where(book => book.Title.ToLower().Contains(text.ToLower())).ToList();
            }
            if (category == "byauthor")
            {
                return this.context.Books.Where(book => book.Author.ToLower().Contains(text.ToLower())).ToList();
            }
            if (category == "bypublisher")
            {
                return this.context.Books.Where(book => book.Publisher.ToLower().Contains(text.ToLower())).ToList();
            }
            if (category == "bygenre")
            {
                return this.context.Books.Where(book => book.Genre.ToLower().Contains(text.ToLower())).ToList();
            }
            if (category == "byisbncode")
            {
                return this.context.Books.Where(book => book.ISBN.ToString().Contains(text.ToLower())).ToList();
            }
            if (category == "byyear")
            {
                return this.context.Books.Where(book => book.PublishDate.Year.ToString() == text).ToList();
            }

            return this.context.Books.ToList();
        }

        [HttpGet("{bookId}")]
        public ActionResult<BookEntity> GetBook(int bookId)
        {
            var book = this.context.Books
                .Where(book => book.Id == bookId)
                .FirstOrDefault();

            if (book != null)
            {
                return this.Ok(book);  
            }
            return this.NotFound();

        }

        [HttpGet("reserve/{bookId}")]
        public ActionResult Reserve(int bookId)
        {
            var userId = 1;
            var reservation = new ReservationEntity { UserId = userId, BookId = bookId };
            var book = this.context.Books.SingleOrDefault(b => b.Id == bookId);

            if(book == null)
            {
                return this.NotFound();
            }

            if (book.BookStatus == "Available")
            {
                this.context.Reservations.Add(reservation);
                book.BookStatus = "Reserved";
                this.context.Books.Attach(book);
                this.context.Entry(book).State = EntityState.Modified;
                this.context.SaveChanges();
                return this.Ok();
            }

            return this.BadRequest();
        }

        [HttpGet("borrow/{bookId}")]
        public void Borrow(int bookId)
        {
            var userId = 1;

            var borrowing = new BorrowingEntity { UserId = userId, BookId = bookId };
            this.context.Borrowings.Add(borrowing);

            var book = this.context.Books.SingleOrDefault(b => b.Id == bookId);
            book.BookStatus = "Borrowed";
            this.context.Books.Attach(book);
            this.context.Entry(book).State = EntityState.Modified;

            this.context.SaveChanges();
        }

        [HttpPost("add")]
        public void Add(BookEntity book)
        {
            this.context.Books.Add(book);
            this.context.SaveChanges();
        }

        [HttpGet("return/{bookId}")]
        public void Return(int bookId)
        {
            var book = this.context.Books.SingleOrDefault(b => b.Id == bookId);
            book.BookStatus = "Available";
            this.context.Books.Attach(book);
            this.context.Entry(book).State = EntityState.Modified;

            this.context.SaveChanges();
        }
         [HttpPost("update/{bookId}")]
         public void Update(BookEntity book, int bookId)
         {
            //if (!string.IsNullOrWhiteSpace
            book.Id = bookId;
            this.context.Books.Attach(book);
            this.context.Entry(book).State = EntityState.Modified;
            this.context.SaveChanges();
        } 

        [HttpGet("delete/{bookId}")]
        public ActionResult Delete(int bookId)
        {
            var book = this.context.Books.SingleOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                this.context.Books.Remove(book);
                context.SaveChanges();
                return this.Ok();
            }
            return this.NotFound();
        }
    }
}
