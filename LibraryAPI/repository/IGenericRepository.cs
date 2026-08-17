using LibraryAPI.DTOs;
using LibraryAPI.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibraryAPI.Repositories
{
    // where T : class معناها إن النوع اللي هنبعته لازم يكون كلاس (يعني جدول من الداتابيز)
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(BookQueryParameters parameters); Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);

        // الدوال الجديدة
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        void Update(T entity);
        void Delete(T entity);
        // يجب أن ترجع Task يحتوي على IEnumerable من النوع T
        // غير السطر ده فقط
        Task<PagedResultDTO<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? pageNumber = null,
            int? pageSize = null);                        // حجم الصفحة

    }
}