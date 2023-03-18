﻿using Courseproject.Common.Interfaces;
using Courseproject.Common.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Courseproject.Infrastructure;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private ApplicationDbContext ApplicationDbContext { get; }
    private DbSet<T> DbSet { get; }

    public GenericRepository(ApplicationDbContext applicationDbContext)
    {
        ApplicationDbContext = applicationDbContext;
        DbSet = applicationDbContext.Set<T>();
    }

    public void Delete(T entity)
    {
        //Is this entity tracked by our ApplicationDbContext and if not we add it and attach it
        if (ApplicationDbContext.Entry(entity).State == EntityState.Detached)
            DbSet.Attach(entity);
        DbSet.Remove(entity);
    }

    public async Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;
        foreach (var include in includes)
            query = query.Include(include);
        if (skip != null)
            query = query.Skip(skip.Value);
        if (take != null)
            query = query.Take(take.Value);

        return await query.AsNoTracking().ToListAsync(); //error fixed 77
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;

        query = query.Where(entity => entity.Id == id);
        foreach (var include in includes)
            query = query.Include(include);

        return await query.AsNoTracking().SingleOrDefaultAsync(); //error fixed 77
    }

    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = DbSet;
        foreach (var filter in filters)
            query = query.Where(filter);

        foreach (var include in includes)
            query = query.Include(include);
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
        await ApplicationDbContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        DbSet.Attach(entity);
        DbSet.Entry(entity).State = EntityState.Modified;
    }
}
