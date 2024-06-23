using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PMS.Application.Common;
using PMS.Application.Dtos.Proudct;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Common;
using PMS.Domain.Entities;
using PMS.Domain.Exceptions;

namespace PMS.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : BaseRequest,IRequest<Result<ProductDto>>
{
    [Required] public Guid Id { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required] public string Isbn { get; set; } = null!;
    [Required] public decimal Price { get; set; } = 0;
    [Required] public List<Guid> Categories { get; set; } = null!;
}

internal class UpdateProductCommandHandler(
    IUnitOfWork unitOfWork, 
    IMediator mediator, 
    IMapper mapper,
    IProductRepository productRepository)
    : BaseRequestHandler<UpdateProductCommand, Result<ProductDto>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<ProductDto>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var existingCategories = (await UnitOfWork.Repository<Category>()
                .GetAllAsync(category => command.Categories.Contains(category.Id))).ToList();
            
            var existingCategoriesIds = existingCategories.Select(category => category.Id).ToList();
            
            if (existingCategoriesIds.Count != command.Categories.Count)
            {
                var nonExistentCategoryIds = command.Categories
                    .Except(existingCategoriesIds)
                    .Select(CategoriesExceptions.SpecificCategoryDoesNotExist)
                    .ToList();

                return nonExistentCategoryIds;
            }
            
            var product = await productRepository.GetFullProductById(command.Id);
            
            if (product == null) return ProductsExceptions.ProductDoesNotExist;
            
            var productResult = product.Update(command.Name, command.Isbn, command.Price, existingCategories, null);
            
            if (!productResult.IsSuccess) return productResult.ToDtoResult<ProductDto>(Mapper);
                
            UnitOfWork.Repository<Product>().UpdateAsync(product);
            product.AddDomainEvent(new ProductUpdatedEvent(product));
            await UnitOfWork.CommitAsync(cancellationToken);
           
            return product.ToSuccessResult().ToDtoResult<ProductDto>(Mapper);
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<ProductDto>.Fail(ex.Message);
        }
    }
}