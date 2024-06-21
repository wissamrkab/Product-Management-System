using PMS.Domain.Common;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Products.Commands.CreateProduct;

public class ProductCreatedEvent(Product product) : BaseEvent
{
    public Product GetProduct() => product;
}