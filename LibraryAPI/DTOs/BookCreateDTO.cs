using Microsoft.AspNetCore.Http;

namespace LibraryAPI.DTOs
{
    public class BookCreateDTO
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public IFormFile ImageFile { get; set; } // لاستقبال الصورة
    }
}