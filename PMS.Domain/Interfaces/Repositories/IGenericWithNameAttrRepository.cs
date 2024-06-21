using Microsoft.EntityFrameworkCore.Query;

namespace PMS.Domain.Interfaces.Repositories;

public interface IGenericWithNameAttrRepository<T> : IGenericRepository<T>
{
    Task<T?> GetByNameAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    
    Task<List<T>> GetAllWithNamesAsync(List<string> names);

    
}