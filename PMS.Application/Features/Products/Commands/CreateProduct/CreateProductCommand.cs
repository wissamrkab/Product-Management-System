using System.ComponentModel.DataAnnotations;
using PMS.Domain.Common;
using PMS.Domain.Exceptions;
using PMS.Domain.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PMS.Application.Common;
using PMS.Application.Dtos.Proudct;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : BaseRequest,IRequest<Result<ProductDto>>
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string Isbn { get; set; } = null!;
    [Required] public decimal Price { get; set; }
    [Required] public List<Guid> Categories { get; set; } = null!;
}

internal class CreateProductCommandHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IMapper mapper)
    : BaseRequestHandler<CreateProductCommand, Result<ProductDto>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<ProductDto>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var existingCategoryIds = (await UnitOfWork.Repository<Category>()
                .GetAllAsync(category => command.Categories.Contains(category.Id))).Select(category => category.Id).ToList();
            
            if (existingCategoryIds.Count != command.Categories.Count)
            {
                var nonExistentCategoryIds = command.Categories
                    .Except(existingCategoryIds)
                    .Select(CategoriesExceptions.SpecificCategoryDoesNotExist)
                    .ToList();

                return nonExistentCategoryIds;
            }
            
            var productResult = Product.Create(
                command.Name,
                command.Isbn,
                command.Price,
                null
            );

            if (!productResult.IsSuccess) return productResult.ToDtoResult<ProductDto>(Mapper);
            
            var product = productResult.Data;
            product?.AddDomainEvent(new ProductCreatedEvent(product));
            
            await UnitOfWork.Repository<Product>().AddAsync(product);
            await UnitOfWork.CommitAsync(cancellationToken);
            
            return productResult.ToDtoResult<ProductDto>(Mapper);
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<ProductDto>.Fail(ex.Message);
        }
    }
}