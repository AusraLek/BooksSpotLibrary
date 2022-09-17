namespace BooksSpotLibrary.Data
{
    public class BookEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishDate { get; set; }
        public string Genre { get; set; }
        public long ISBN { get; set; }
        public string BookStatus { get; set; }
    }
}
