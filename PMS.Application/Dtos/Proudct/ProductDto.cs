using PMS.Application.Common.Mapping;
using PMS.Application.Dtos.Audit;
using PMS.Application.Dtos.CategoryDto_s;

namespace PMS.Application.Dtos.Proudct;

public class ProductDto : Auditable, IMapFrom<Domain.Entities.Product>
{
    public string Name { get; set; } = null!;
    public string Isbn { get; set; } = null!;
    public decimal Price { get; set; }
    public ICollection<CategoryDto> Categories { get; private set; } = null!;
}