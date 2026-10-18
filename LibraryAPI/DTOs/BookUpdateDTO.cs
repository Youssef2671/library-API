public class BookUpdateDTO
{
    public string Title { get; set; }
    public int AuthorId { get; set; }
    public IFormFile? ImageFile { get; set; } // علامة الاستفهام دي مهمة جداً هنا
}