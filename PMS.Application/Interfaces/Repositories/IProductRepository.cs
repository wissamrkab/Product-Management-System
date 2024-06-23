using PMS.Domain.Entities;

namespace PMS.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetFullProductById(Guid id);

    Task<IEnumerable<Product>> GetFullProductList(int page = 1, int pageSize = 10,
        string searchCriteria = "", List<Guid>? categoryIds = null);

}