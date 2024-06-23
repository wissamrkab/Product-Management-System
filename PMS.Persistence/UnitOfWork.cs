using System.Collections;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Common.Interfaces;
using PMS.Persistence.Contexts;
using PMS.Persistence.Repositories;

namespace PMS.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDomainEventDispatcher? _dispatcher;
    private Hashtable? _repositories;

    public UnitOfWork(ApplicationDbContext dbContext, IDomainEventDispatcher dispatcher)
    {
        _dbContext = dbContext;
        _dispatcher = dispatcher;
    }

    public IGenericRepository<T> Repository<T>()
    {
        _repositories ??= new Hashtable();

        var type = typeof(T).Name;

        if (_repositories.ContainsKey(type)) return (IGenericRepository<T>)_repositories[type]!;
        var repositoryType = typeof(GenericRepository<>);

        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);

        _repositories.Add(type, repositoryInstance);

        return (IGenericRepository<T>)_repositories[type]!;
    }

    public Task Rollback()
    {
        _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        return Task.CompletedTask;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}