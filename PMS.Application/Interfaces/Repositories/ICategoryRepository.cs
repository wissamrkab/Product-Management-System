using PMS.Application.Dtos.Category;

namespace PMS.Application.Interfaces.Repositories;

public interface ICategoryRepository
{ 
    Task<List<CategoryWithProductCountDto>> GetCategoriesWithProductCountAsync();
}