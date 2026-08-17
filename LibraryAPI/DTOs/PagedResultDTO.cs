namespace LibraryAPI.DTOs
{
    public class PagedResultDTO<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}