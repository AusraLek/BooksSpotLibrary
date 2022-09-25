using BooksSpotLibrary.Controllers;
using BooksSpotLibrary.Data;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;

namespace BooksSpotLibrary.Tests
{
    [TestClass]
    public class BooksControllerTests
    {
        private readonly BooksController controller;
        private readonly BooksContext database;

        public BooksControllerTests()
        {
            this.database = CreateDatabase();
            this.controller = new BooksController(this.database);
        }

        [TestMethod]
        public void GetBookWhenBookExists()
        {
            // Act
            var result = this.controller.GetBook(123);

            // Assert
            result
                .Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            result
                .Result
                .Should()
                .BeOfType<OkObjectResult>()
                .Which
                .Value
                .Should()
                .BeEquivalentTo(new BookEntity
                {
                    Id = 123,
                    Title = "TestTitle",
                    Author = "Vard",
                    BookStatus = "Available",
                    Genre = "TestGenre",
                    Publisher = "TestPublisher",
                });
        }

        [TestMethod]
        public void GetBookWhenNotExist()
        {
            // Act
            var result = this.controller.GetBook(17);

            // Assert
            result
                .Result
                .Should()
                .BeOfType<NotFoundResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void ReserveBookWhenNotExists()
        {
            // Act
            var result = this.controller.Reserve(12);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void ReserveBookWhenExistsAndAvailable()
        {
            // Arrange
            var bookId = 123;

            // Act
            var result = this.controller.Reserve(bookId);

            // Assert
            result
                .Should()
                .BeOfType<OkResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            this.database.Reservations
                .Where(reservation => reservation.BookId == bookId)
                .FirstOrDefault()
                .Should()
                .BeEquivalentTo(new ReservationEntity { UserId = 1, BookId = bookId, Id = 1});

            this.database.Books
                .Where(book => book.Id == bookId)
                .FirstOrDefault()
                .BookStatus
                .Should()
                .Be("Reserved");
        }

        [TestMethod]
        public void ReserveBookWhenExistsAndNotAvailable()
        {
            // Arrange
            var bookId = 124;

            // Act
            var result = this.controller.Reserve(bookId);

            // Assert
            result
                .Should()
                .BeOfType<BadRequestResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void BorrowBookWhenNotExists()
        {
            // Act
            var result = this.controller.Borrow(18);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void BorrowBookWhenExistsAndAvailable()
        {
            // Arrange
            var bookId = 123;

            // Act
            var result = this.controller.Borrow(bookId);

            // Assert
            result
                .Should()
                .BeOfType<OkResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);

            this.database.Borrowings
                .Where(borrowing => borrowing.BookId == bookId)
                .FirstOrDefault()
                .Should()
                .BeEquivalentTo(new BorrowingEntity { UserId = 1, BookId = bookId, Id = 1});

            this.database.Books
                .Where(book => book.Id == bookId)
                .FirstOrDefault()
                .BookStatus
                .Should()
                .Be("Borrowed");
        }

        [TestMethod]
        public void BorrowBookWhenExistsAndNotAvailable()
        {
            // Arrange
            var bookId = 125;

            // Act
            var result = this.controller.Borrow(bookId);

            // Assert
            result
                .Should()
                .BeOfType<BadRequestResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Add()
        {
            // Arrange
            var book = new BookEntity
            {
                Title = "AddBookTestTitle",
                Author = "Name",
                BookStatus = "Available",
            };

            // Act
            this.controller.Add(book);

            // Assert
            this.database.Books
                .Where(book => book.Title == "AddBookTestTitle")
                .FirstOrDefault()
                .Should()
                .BeEquivalentTo(book);
        }

        [TestMethod]
        public void DeleteWhenBookExists()
        {
            // Arrange
            var bookId = 127;

            // Act
            var result = this.controller.Delete(bookId);

            // Assert
            this.database.Books
                .Where(book => book.Id == bookId)
                .FirstOrDefault()
                .Should()
                .BeNull();

            result
                .Should()
                .BeOfType<OkResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.OK);
        }

        [TestMethod]
        public void DeleteWhenBookNotExist()
        {
            // Act
            var result = this.controller.Delete(457);

            // Assert
            result
                .Should()
                .BeOfType<NotFoundResult>()
                .Which
                .StatusCode
                .Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        [DataRow("bytitle", "Title", 129)]
        [DataRow("byauthor", "Author", 128)]
        [DataRow("bypublisher", "Publisher", 130)]
        [DataRow("bygenre", "Genre", 131)]
        [DataRow("byisbncode", "1234567", 132)]
        [DataRow("byyear", "2000", 133)]
        public void SearchByTitle(string category, string text, int bookId)
        {
            // Arrange
            var searchOptions = new SearchOptions
            {
                Category = category,
                Text = text,
                Status = new [] {"Available", "Borrowed", "Reserved"}
            };

            // Act
            var result = this.controller.Search(searchOptions);

            // Assert
            result
                 .Select(book => book.Id)
                 .Should()
                 .Contain(bookId);
        }

        private BooksContext CreateDatabase()
        {
            var random = new Random();
            var contextOptions = new DbContextOptionsBuilder<BooksContext>()
            .UseInMemoryDatabase(random.Next(1, 500).ToString())
            .Options;
            var database = new BooksContext(contextOptions);
            var book = new BookEntity
            {
                Id = 123,
                Title = "TestTitle",
                Author = "Vard",
                BookStatus = "Available",
                Genre = "TestGenre",
                Publisher = "TestPublisher",
            };

            var book2 = new BookEntity
            {
                Id = 124,
                Title = "TestTitle",
                Author = "Vard",
                BookStatus = "Reserved",
                Genre = "TestGenre",
                Publisher = "TestPublisher",
            };
            var book3 = new BookEntity
            {
                Id = 125,
                Title = "TestTitle",
                Author = "Vard",
                BookStatus = "Borrowed",
                Genre = "TestGenre",
                Publisher = "TestPublisher",
            };
            var bookDelete = new BookEntity
            {
                Id = 127,
                Title = "TestTitle",
                Author = "Vard",
                BookStatus = "Borrowed",
                Genre = "TestGenre",
                Publisher = "TestPublisher",
            };
            var bookByAuthor = new BookEntity
            {
                Id = 128,
                Title = "TestTitle",
                Author = "Author",
                BookStatus = "Borrowed",
                Genre = "TestGenre",
                Publisher = "TestPublisher",
            };
            var bookByTitle = new BookEntity
            {
                Id = 129,
                Title = "Title",
                Author = "Author",
                BookStatus = "Available",
                Genre = "TestGenre",
                Publisher = "TestPublisher",
            };
            var bookBypPublisher = new BookEntity
            {
                Id = 130,
                Title = "TestTitle",
                Author = "Author",
                BookStatus = "Borrowed",
                Publisher = "Publisher",
                Genre = "TestGenre",
            };
            var bookByGenre = new BookEntity
            {
                Id = 131,
                Title = "TestTitle",
                Author = "Author",
                BookStatus = "Available",
                Genre = "Genre",
                Publisher = "TestPublisher",
            };
            var bookByISBNCode = new BookEntity
            {
                Id = 132,
                Title = "TestTitle",
                Author = "Author",
                BookStatus = "Available",
                ISBN = 1234567,
                Genre = "TestGenre",
                Publisher = "TestPublisher",
            };
            var bookByYear = new BookEntity
            {
                Id = 133,
                Title = "TestTitle",
                Author = "Author",
                BookStatus = "Available",
                ISBN = 1234567,
                PublishDate = new DateTime(2000,02,02),
                Genre = "TestGenre",
                Publisher = "TestPublisher",
            };

            database.Books.Add(book);
            database.Books.Add(book2);
            database.Books.Add(book3);
            database.Books.Add(bookDelete);
            database.Books.Add(bookByAuthor);
            database.Books.Add(bookByTitle);
            database.Books.Add(bookBypPublisher);
            database.Books.Add(bookByGenre);
            database.Books.Add(bookByISBNCode);
            database.Books.Add(bookByYear);
            database.SaveChanges();

            return database;
        }
    }
}