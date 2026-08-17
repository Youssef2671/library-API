using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;

namespace LibraryAPI.Data
{
    public class BooksContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global Query Filters
            modelBuilder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<Author>().HasQueryFilter(a => !a.IsDeleted);
        }

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