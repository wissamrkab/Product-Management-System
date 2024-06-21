using PMS.Domain.Entities;
using PMS.Application.Common.Mapping;

namespace PMS.Application.Dtos.CategoryDto_s;

public class CategoryDto : IMapFrom<Category>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Image { get; set; } = null!;
    public Guid? ParentId { get; set; }
    public List<CategoryDto> SubCategories { get; set; } = null!;
}