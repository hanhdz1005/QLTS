using QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Core.Interface
{
    public interface IGenericRepository<T> where T : BaseEntity<Guid>
    {
        IQueryable<T> Query();
        IQueryable<T> Query(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(Guid id, T entity);
        Task DeleteAsync(Guid id);
        Task<bool> CheckAsync(Expression<Func<T, bool>> predicate);
    }
}
