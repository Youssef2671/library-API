using LibraryAPI.Data;
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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
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
    }
}