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

        [HttpGet("Search")]
        public IEnumerable<BookEntity> Search()
        {
            return this.context.Books.ToList();
        }

        [HttpGet("{bookId}")]
        public BookEntity GetBook(int bookId)
        {
            return this.context.Books
                .Where(book => book.Id == bookId)
                .FirstOrDefault();
        }

        [HttpGet("reserve/{bookId}")]
        public void Reserve(int bookId)
        {
            var userId = 1;

            var reservation = new ReservationEntity { UserId = userId, BookId = bookId };
            this.context.Reservations.Add(reservation);

            var book = this.context.Books.SingleOrDefault(b => b.Id == bookId);
            book.BookStatus = "Reserved";
            this.context.Books.Attach(book);
            this.context.Entry(book).State = EntityState.Modified;

            this.context.SaveChanges();
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


    }
}
