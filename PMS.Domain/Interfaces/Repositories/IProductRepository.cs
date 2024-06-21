using PMS.Domain.Entities;

namespace PMS.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetFullProductById(Guid id);
}