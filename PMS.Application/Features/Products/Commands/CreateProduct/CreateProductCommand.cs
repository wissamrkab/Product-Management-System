﻿using System.ComponentModel.DataAnnotations;
using PMS.Domain.Common;
using PMS.Domain.Exceptions;
using AutoMapper;
using MediatR;
using PMS.Application.Common;
using PMS.Application.Dtos.Proudct;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : BaseRequest,IRequest<Result<ProductDto>>
{
    [Required] public string Name { get; set; } = null!;
    [Required] public string Isbn { get; set; } = null!;
    [Required] public decimal Price { get; set; } = 0;
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
            
            var productResult = Product.Create(
                command.Name,
                command.Isbn,
                command.Price,
                existingCategories,
                null
            );

            if (!productResult.IsSuccess || productResult.Data == null) return productResult.ToDtoResult<ProductDto>(Mapper);
            
            var product = productResult.Data;
            product.AddDomainEvent(new ProductCreatedEvent(product));
            
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