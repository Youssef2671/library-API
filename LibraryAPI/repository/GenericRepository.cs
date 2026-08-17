using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibraryAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BooksContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(BooksContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // هنا بنحدد إحنا شغالين على أنهي جدول
        }

        public async Task<PagedResultDTO<T>> GetAllAsync(
     Expression<Func<T, bool>>? filter = null,
     Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
     int? pageNumber = null,
     int? pageSize = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            // 1. حساب العدد الكلي (مهم جداً للـ Frontend)
            int totalItems = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);

            if (pageNumber.HasValue && pageSize.HasValue)
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);

            var items = await query.ToListAsync();

            // 2. إرجاع النتيجة متغلفة
            return new PagedResultDTO<T>
            {
                TotalItems = totalItems,
                CurrentPage = pageNumber ?? 1,
                PageSize = pageSize ?? totalItems,
                TotalPages = pageSize.HasValue ? (int)Math.Ceiling(totalItems / (double)pageSize.Value) : 1,
                Items = items
            };
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(); // السطر ده هو اللي بيبعت الداتا للداتابيز بجد
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            // لو الكنترولر باعت أي جداول عايز يربطها، ضيفها للكويري
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }
        // جوه كلاس GenericRepository ضيف الدالتين دول

        public async Task UpdateAsync(T entity)
        {
            // Update مش بتحتاج Async في الـ EF Core، بس بنعمل Await للـ SaveChanges
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            // Remove مش بتحتاج Async، بس بنعمل Await للـ SaveChanges
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync(BookQueryParameters parameters)
        {
            // 1. نبدأ بـ Queryable عشان ميسحبش الداتا من الداتا بيز دلوقتي
            var query = _context.Books.AsQueryable();

            // 2. الفلترة (Filtering)
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchTerm = parameters.SearchTerm.Trim().ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(searchTerm));
            }

            // 3. الترتيب (Sorting)
            if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                query = parameters.OrderBy.ToLower() switch
                {
                    "title" => query.OrderBy(b => b.Title),
                    "titledesc" => query.OrderByDescending(b => b.Title),
                    "date" => query.OrderBy(b => b.PublishDate),
                    "datedesc" => query.OrderByDescending(b => b.PublishDate),
                    _ => query.OrderBy(b => b.Id) // الترتيب الافتراضي
                };
            }
            else
            {
                // ترتيب افتراضي لو المستخدم مبعتش حاجة
                query = query.OrderBy(b => b.Id);
            }

            // 4. الترقيم (Pagination)
            // بنعمل Skip للصفحات اللي فاتت، و Take لعدد العناصر في الصفحة الحالية
            query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                         .Take(parameters.PageSize);

            // 5. التنفيذ الفعلي في الداتا بيز
            return await query.ToListAsync();
        }
    }
}