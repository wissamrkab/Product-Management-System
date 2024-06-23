using PMS.Application.Common.Mapping;

namespace PMS.Application.Dtos.Category;

public class CategoryDto : IMapFrom<Domain.Entities.Category>
{
    public Guid Id { get; set; }
    public string NameEn { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public Guid? ParentId { get; set; }
}

public class CategoryWithSubCategoriesDto : CategoryDto
{
    public List<CategoryWithSubCategoriesDto> SubCategories { get; set; } = null!;
}