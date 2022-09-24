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
            };

            var book2 = new BookEntity
            {
                Id = 124,
                Title = "TestTitle",
                Author = "Vard",
                BookStatus = "Reserved",
            };
            var book3 = new BookEntity
            {
                Id = 125,
                Title = "TestTitle",
                Author = "Vard",
                BookStatus = "Borrowed",
            };
            database.Books.Add(book);
            database.Books.Add(book2);
            database.Books.Add(book3);
            database.SaveChanges();

            return database;
        }
    }
}