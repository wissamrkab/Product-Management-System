using System.Linq.Expressions;
using PMS.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using PMS.Application.Interfaces.Repositories;

namespace PMS.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> DbSet;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        DbSet = _dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (include != null)
            query = include(query);


        return await query.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id);
    }  
    
    public async Task<IEnumerable<T>?> GetAllByDIdsAsync(List<Guid> guids, Func<IQueryable<T>
        , IIncludableQueryable<T, object>>? include = null)
    {
        if (guids.IsNullOrEmpty())
        {
            return new List<T>();
        }
        var query = _dbContext.Set<T>().AsQueryable();

        if (include != null)
            query = include(query);
        
        query = query.Where(x => guids.Contains(EF.Property<Guid>(x, "Id")));
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>>? wheres = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (include != null)
            query = include(query);

        if (wheres != null) query = query.Where(wheres);

        // Calculate skip count based on page number and page size
        var skipCount = (page - 1) * pageSize;

        // Apply pagination
        query = query.Skip(skipCount).Take(pageSize);

        return await query.ToListAsync();
    }
    
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> wheres ,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (include != null)
            query = include(query);
        
        query = query.Where(wheres);
        
        return await query.ToListAsync();
    }

    public Task<List<T>> GetAllAsync()
    {
        var query = _dbContext.Set<T>().ToListAsync();
        return query;
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void UpdateAsync(T entity , Expression<Func<T, bool>>? wheres ,Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        
        DbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }
    public T DeleteAsync(T entity)
    {
            DbSet.Remove(entity);
            
            return entity;
    }
    
}