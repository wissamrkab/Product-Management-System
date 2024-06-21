using PMS.Domain.Common;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Products.Commands.DeleteProduct;

public class ProductDeletedEvent(Product product) : BaseEvent
{
    public Product GetProduct() => product;
}