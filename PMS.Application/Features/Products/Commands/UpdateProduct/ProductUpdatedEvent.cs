using PMS.Domain.Common;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Products.Commands.UpdateProduct;

public class ProductUpdatedEvent(Product product) : BaseEvent
{
    public Product GetProduct() => product;
}