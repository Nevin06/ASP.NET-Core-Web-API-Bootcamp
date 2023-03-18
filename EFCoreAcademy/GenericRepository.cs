using EFCoreAcademy.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCoreAcademy;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private ApplicationDbContext DbContext { get; }
    private DbSet<T> DbSet { get; }
    public GenericRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<T>();
    }
    public void Delete(T entity)
    {
        if (DbContext.Entry(entity).State == EntityState.Detached)
            DbSet.Attach(entity);
        DbSet.Remove(entity);
    }

    public async Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;
        foreach(var include in includes)
            query = query.Include(include);
        if (skip != null)
            query = query.Skip(skip.Value);
        if (take != null)
            query = query.Take(take.Value);

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;

        query = query.Where(e=> e.Id == id);
        foreach (var include in includes)
            query = query.Include(include);

        return await query.SingleOrDefaultAsync();
    }

    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;
        foreach (var filter in filters)
            query = query.Include(filter);
        if (skip != null)
            query = query.Skip(skip.Value);
        if (take != null)
            query = query.Take(take.Value);
        return await query.ToListAsync();
    }

    public async Task<int> InsertAsync(T entity)
    {
        await DbSet.AddAsync(entity); 
        return entity.Id; 
    }

    public async Task SaveChangesAsync()
    {
        await DbContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        DbSet.Attach(entity);
        DbSet.Entry(entity).State = EntityState.Modified;
    }
}
