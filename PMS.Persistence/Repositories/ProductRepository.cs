using PMS.Domain.Entities;
using PMS.Domain.Interfaces.Repositories;

namespace PMS.Persistence.Repositories;

public class ProductRepository(IGenericRepository<Product> repository) : IProductRepository
{
    public async Task<Product?> GetFullProductById(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }
}