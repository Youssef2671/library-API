using System.Collections.Generic;

namespace LibraryAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Property: عشان EF Core يفهم إن المؤلف ليه كتب كتير
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}