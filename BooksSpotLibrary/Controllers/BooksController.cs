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
        public BooksController(BooksContext context)
        {
            this.context = context;
        }

        [HttpPost("Search")]
        public IEnumerable<BookEntity> Search(SearchOptions searchOptions)
        {
            if(searchOptions == null || searchOptions.Text == null)
            {
                return this.context.Books.ToList();
            }

            var searchRecords = this.context.Books.Where(book => searchOptions.Status.Contains(book.BookStatus)).AsEnumerable();


            searchRecords = searchOptions.Category switch
            {
                "bytitle" => searchRecords.Where(book => this.MatchesSearch(book.Title, searchOptions)),
                "byauthor" => searchRecords.Where(book => this.MatchesSearch(book.Author, searchOptions)),
                "bypublisher" => searchRecords.Where(book => this.MatchesSearch(book.Publisher, searchOptions)),
                "bygenre" => searchRecords.Where(book => this.MatchesSearch(book.Genre, searchOptions)),
                "byisbncode" => searchRecords.Where(book => this.MatchesSearch(book.ISBN.ToString(), searchOptions)),
                "byyear" => searchRecords.Where(book => this.MatchesSearch(book.PublishDate.Year.ToString(), searchOptions)),
                _ => searchRecords,
            };

            return searchRecords.ToList();
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
        public ActionResult Borrow(int bookId)
        {
            var userId = 1;

            var borrowing = new BorrowingEntity { UserId = userId, BookId = bookId };
            var book = this.context.Books.SingleOrDefault(b => b.Id == bookId);

            if (book == null)
            {
                return this.NotFound();
            }

            if (book.BookStatus == "Reserved")
            {
                this.context.Borrowings.Add(borrowing);
                book.BookStatus = "Borrowed";
                this.context.Books.Attach(book);
                this.context.Entry(book).State = EntityState.Modified;
                this.context.SaveChanges();
                return this.Ok();
            }

            return this.BadRequest();
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

        private bool MatchesSearch(string value, SearchOptions searchOptions)
        {
            return value.ToLower().Contains(searchOptions.Text.ToLower());
        }
    }
}
