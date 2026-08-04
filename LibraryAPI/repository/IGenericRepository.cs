using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibraryAPI.Repositories
{
    // where T : class معناها إن النوع اللي هنبعته لازم يكون كلاس (يعني جدول من الداتابيز)
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);

        // الدوال الجديدة
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        
        void Update(T entity);
        void Delete(T entity);

    }
}