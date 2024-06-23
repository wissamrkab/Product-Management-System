namespace PMS.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IGenericRepository<T> Repository<T>();
    Task Rollback();
    Task<int> CommitAsync(CancellationToken cancellationToken);
}