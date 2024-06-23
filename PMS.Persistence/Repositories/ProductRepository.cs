using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Entities;

namespace PMS.Persistence.Repositories;

public class ProductRepository(IGenericRepository<Product> repository) : IProductRepository
{
    public async Task<Product?> GetFullProductById(Guid id)
    {
        return await repository.GetByIdAsync(id,
            products => products.Include(product => 
                product.Categories
                )
            );
    }

    public async Task<IEnumerable<Product>> GetFullProductList(int page = 1, int pageSize = 10, string searchCriteria = "", List<Guid>? categoryIds = null)
    {
        return await repository.GetAllAsync(
            page,
            pageSize,
            product => (product.Name.Contains(searchCriteria) || product.Isbn.Contains(searchCriteria))
            && (categoryIds == null || product.Categories.Any(cat => categoryIds.Contains(cat.Id))) ,
            products => products.Include(product => 
                product.Categories
            )
        );
    }
}