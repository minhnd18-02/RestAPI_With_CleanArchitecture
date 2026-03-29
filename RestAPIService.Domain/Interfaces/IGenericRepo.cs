using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Domain.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T?> FindEntityAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Filter();
    }
}
