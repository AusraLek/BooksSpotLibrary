namespace BooksSpotLibrary.Data
{
    public class BorrowingEntity
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
