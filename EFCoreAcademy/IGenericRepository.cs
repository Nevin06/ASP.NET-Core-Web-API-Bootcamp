using EFCoreAcademy.Models;
using System.Linq.Expressions;

namespace EFCoreAcademy;
//Generic parameter T inherits from BaseEntity class
//we work with only entities
public interface IGenericRepository <T> where T : BaseEntity
{
    //skip no of entities for paging
    Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes);
    Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<int> InsertAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}
