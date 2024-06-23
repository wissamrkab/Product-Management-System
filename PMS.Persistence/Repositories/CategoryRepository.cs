using Microsoft.EntityFrameworkCore;
using PMS.Application.Dtos.Category;
using PMS.Application.Interfaces.Repositories;
using PMS.Persistence.Contexts;

namespace PMS.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext applicationDbContext) : ICategoryRepository
{
    public async Task<List<CategoryWithProductCountDto>> GetCategoriesWithProductCountAsync()
    {
        return await applicationDbContext.Categories
            .Select(c => new CategoryWithProductCountDto
            {
                Name = c.NameEn,
                Count = c.Products.Count
            })
            .ToListAsync();
    }
}