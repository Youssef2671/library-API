using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;

namespace LibraryAPI.Data
{
    public class BooksContext : DbContext
    {
        // الكونستراكتور ده هو اللي هيستقبل إعدادات الاتصال بالداتابيز من بره
        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {
        }

        // تسجيل الجداول
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
    }
}