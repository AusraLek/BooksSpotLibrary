using Microsoft.EntityFrameworkCore;

namespace BooksSpotLibrary.Data
{
    public class BooksContext : DbContext
    {

        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {
        }

        public DbSet<BookEntity> Books { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<BorrowingEntity> Borrowings { get; set; }
    }
}
