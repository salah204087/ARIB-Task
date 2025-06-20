using Core.Infrastruture.UnitOfWork;
using DataLayer;
using DataLayer.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastruture.RepositoryPattern.Repository;

public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    private readonly IUnitOfWork _unitOfWork;

    public Repository(ApplicationDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<T>();
        _unitOfWork = new UnitOfWork.UnitOfWork(context);
    }

    public IQueryable<T> Query()
        => _dbContext.Set<T>().AsQueryable();

    public ICollection<T> GetAll()
         => _dbContext.Set<T>().ToList();

    public ICollection<T> GetAll(string includeProperties = "")
    {
        IQueryable<T> query = _dbContext.Set<T>();
        foreach (
        var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }
        return query.ToList();
    }

    public async Task<ICollection<T>> GetAllAsync()
       => await _dbContext.Set<T>().ToListAsync();
    public async Task<ICollection<T>> GetAllAsync(string includeProperties = "")
    {
        IQueryable<T> query = _dbContext.Set<T>();
        foreach (
        var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }
        return await query.ToListAsync();
    }

#pragma warning disable CS8603 // Possible null reference return.
    public T GetById(int id)
        => _dbContext.Set<T>().Find(id);

    public async Task<T> GetByIdAsync(int id)
        => await _dbContext.Set<T>().FindAsync(id);

    public T GetByUniqueId(Guid id)
        => _dbContext.Set<T>().Find(id);

    public async Task<T> GetByUniqueIdAsync(Guid id)
        => await _dbContext.Set<T>().FindAsync(id);

    public T Find(Expression<Func<T, bool>> match)
        => _dbContext.Set<T>().SingleOrDefault(match);

    public async Task<T> FindAsync(Expression<Func<T, bool>> match, string includeProperties = "")
    {
        IQueryable<T> query = _dbContext.Set<T>();
        foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }
        return await query.SingleOrDefaultAsync(match);
    }

    public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        => _dbContext.Set<T>().Where(match).ToList();

    public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match, string includeProperties = "")
    {
        IQueryable<T> query = _dbContext.Set<T>().Where(match);
        foreach (
        var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }
        return await query.ToListAsync();
    }

    public T Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        _dbContext.SaveChanges();
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        await _unitOfWork.Commit();
        return entity;
    }

    public T Update(T updated)
    {
        if (updated is null)
        {
            return null;
        }

        _dbContext.Set<T>().Attach(updated);
        _dbContext.SaveChanges();

        return updated;
    }

    public async Task<T> UpdateAsync(T updated)
    {
        if (updated is null)
        {
            return null;
        }

        _dbContext.Set<T>().Attach(updated);
        await _unitOfWork.Commit();

        return updated;
    }

    public void Delete(T t)
    {
        _dbContext.Set<T>().Remove(t);
        _dbContext.SaveChanges();
    }

    public async Task<int> DeleteAsync(T t)
    {
        _dbContext.Set<T>().Remove(t);
        return await _unitOfWork.Commit();
    }

    public int Count()
        => _dbContext.Set<T>().Count();

    public async Task<int> CountAsync()
        => await _dbContext.Set<T>().CountAsync();

    public IEnumerable<T> Filter(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "", int? page = null,
        int? pageSize = null)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        if (includeProperties is not null)
        {
            foreach (
                var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (page is not null && pageSize is not null)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return query.ToList();
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        => _dbContext.Set<T>().Where(predicate);

    public bool Exist(Expression<Func<T, bool>> predicate)
    {
        var exist = _dbContext.Set<T>().Where(predicate);
        return exist.Any();
    }

    //////////////////////////////////////Another Type from Function////////////////////////////////////////////

    public IEnumerable<T> GetAll1()
      => _dbSet.AsEnumerable();

    public T Get(int id)
      => _dbContext.Set<T>().Find(id);

    public void Insert(T entity)
    {
        if (entity is null)
        {
            throw new NotImplementedException("entity");
        }
        _dbSet.Add(entity);
        _dbContext.SaveChanges();
    }

    public void Update1(T entity)
    {
        if (entity is null)
        {
            throw new NotImplementedException("entity is null");
        }
        _dbSet.Update(entity);
        _dbContext.SaveChanges();
    }

    public void Delete1(T entity)
    {
        if (entity is null)
        {
            throw new NotImplementedException("entity is null");
        }
        _dbSet.Remove(entity);
        _dbContext.SaveChanges();
    }

    public void Remove(T entity)
    {
        if (entity is null)
        {
            throw new NotImplementedException("entity is null");
        }
        _dbSet.Remove(entity);

    }

    public void SaveChanges()
      => _dbContext.SaveChanges();

    public async Task SaveChangesAsync()
      => await _dbContext.SaveChangesAsync();

    public int SaveChangesTracker()
      => _dbContext.SaveChanges();

    public void RemoveRange(IEnumerable<T> entity)
    {
        if (entity is null)
        {
            throw new NotImplementedException("entity is null");
        }
        _dbSet.RemoveRange(entity);
    }

    public async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        if (entities is null)
        {
            throw new NotImplementedException("entity is null");
        }
        _dbSet.RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
        _dbContext.SaveChanges();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        _dbContext.SaveChanges();
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        await _dbContext.SaveChangesAsync();
    }
#pragma warning restore CS8603 // Possible null reference return.
}