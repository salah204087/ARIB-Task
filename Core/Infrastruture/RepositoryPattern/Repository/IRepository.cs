using DataLayer.Helpers;
using System.Linq.Expressions;

namespace Core.Infrastruture.RepositoryPattern.Repository;

public interface IRepository<T>
    where T : Entity

{
    IQueryable<T> Query();

    ICollection<T> GetAll();

    ICollection<T> GetAll(string includeProperties = "");

    Task<ICollection<T>> GetAllAsync();

    Task<ICollection<T>> GetAllAsync(string includeProperties = "");

    T GetById(int id);

    Task<T> GetByIdAsync(int id);

    T GetByUniqueId(Guid id);

    Task<T> GetByUniqueIdAsync(Guid id);

    T Find(Expression<Func<T, bool>> match);

    Task<T> FindAsync(Expression<Func<T, bool>> match, string includeProperties = "");

    ICollection<T> FindAll(Expression<Func<T, bool>> match);

    Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match, string includeProperties = "");

    T Add(T entity);

    Task<T> AddAsync(T entity);

    T Update(T updated);

    Task<T> UpdateAsync(T updated);

    void Delete(T t);

    Task<int> DeleteAsync(T t);

    int Count();

    Task<int> CountAsync();

    IEnumerable<T> Filter(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "",
        int? page = null,
        int? pageSize = null);

    IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

    bool Exist(Expression<Func<T, bool>> predicate);


    ////////////////////////////////////////////////////////
    IEnumerable<T> GetAll1();
    T Get(int id);
    void Insert(T entity);
    void Update1(T entity);
    void Delete1(T entity);
    void Remove(T entity); //For service on CRAD Opration
    void SaveChanges();    //For service on CRAD Opration
    Task SaveChangesAsync();
    int SaveChangesTracker();
    void RemoveRange(IEnumerable<T> entity);
    Task RemoveRangeAsync(IEnumerable<T> entities);
    void AddRange(IEnumerable<T> entities);
    Task AddRangeAsync(IEnumerable<T> entities);
    void UpdateRange(IEnumerable<T> entities);
    Task UpdateRangeAsync(IEnumerable<T> entities);

}
