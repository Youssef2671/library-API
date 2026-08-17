namespace LibraryAPI.Models
{
    public class Book :ISoftDelete
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; } // عشان هنحتاج نرفع صورة للغلاف بعدين

        // Foreign Key
        public int AuthorId { get; set; }

        // Navigation Property: عشان نقدر نوصل لبيانات المؤلف من الكتاب
        public Author Author { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsDeleted { get; set; } = false; // ضفنا الخاصية دي
    }
}