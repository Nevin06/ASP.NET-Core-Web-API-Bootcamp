using Courseproject.Common.Model;
using System.Linq.Expressions;

namespace Courseproject.Common.Interfaces;
//62
//Repositories always work with entities
public interface IGenericRepository<T> where T : BaseEntity
{
    //T is the type of entity we are working with in this repository
    Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes);
    Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes);
    //nullable entity because we may not find entity for this Id
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<int> InsertAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}
