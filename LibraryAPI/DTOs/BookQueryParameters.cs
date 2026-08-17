namespace LibraryAPI.DTOs
{
    public class BookQueryParameters
    {
        const int maxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        // للفلترة (مثلاً البحث بجزء من اسم الكتاب)
        public string? SearchTerm { get; set; }

        // للترتيب (مثلاً: title, publishDate, authorId)
        public string? OrderBy { get; set; }
    }
}