namespace LibraryAPI.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string AuthorName { get; set; } // Flattening the data
    }
}