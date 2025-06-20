using Core.Infrastruture.RepositoryPattern.Repository;
using DataLayer;
using DataLayer.Helpers;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Infrastruture.UnitOfWork;

public class UnitOfWork(ApplicationDbContext _dbContext) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
    private bool disposed = false;
    private IDbContextTransaction? _dbTransaction;

    public Dictionary<Type, object> Repositories
    {
        get { return _repositories; }
        set { Repositories = value; }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IRepository<T> Repository<T>() where T : Entity
    {
        if (Repositories.Keys.Contains(typeof(T)))
        {
            return Repositories[typeof(T)] as IRepository<T>;
        }

        IRepository<T> repo = new Repository<T>(_dbContext);

        Repositories.Add(typeof(T), repo);
        return repo;
    }

    public async Task<int> Commit()
    {
        return await _dbContext.SaveChangesAsync();
    }


    // Transaction
    public async Task BeginTransactionAsync()
    {
        _dbTransaction ??= await _dbContext.Database.BeginTransactionAsync();
    }
    public async Task CommitTransactionAsync()
    {
        if (_dbTransaction != null)
        {
            await _dbTransaction.CommitAsync();
        }
    }
    public async Task RollbackTransactionAsync()
    {
        if (_dbTransaction != null)
        {
            await _dbTransaction.RollbackAsync();
            await _dbTransaction.DisposeAsync();
            _dbTransaction = null;
        }
    }
}