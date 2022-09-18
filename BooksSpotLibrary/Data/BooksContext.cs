using Microsoft.EntityFrameworkCore;

namespace BooksSpotLibrary.Data
{
    public class BooksContext : DbContext
    {

        public BooksContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-4SKOP0M;Database=BooksDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<BookEntity> Books { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<BorrowingEntity> Borrowings { get; set; }
    }
}
