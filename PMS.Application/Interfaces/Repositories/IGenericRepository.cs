using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace PMS.Application.Interfaces.Repositories;

public interface IGenericRepository<T>
{
    Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    
    Task<IEnumerable<T>?> GetAllByDIdsAsync(List<Guid> guids, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    public Task<int> GetCountAsync(Expression<Func<T, bool>>? wheres = null);
    Task<IEnumerable<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>>? wheres = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> wheres ,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    
    Task<List<T>> GetAllAsync();
    
    Task AddAsync(T entity);
    
    void UpdateAsync(T entity , Expression<Func<T, bool>>? wheres =null ,Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    
    Task DeleteAsync(Guid id);

    T DeleteAsync(T entity);
}