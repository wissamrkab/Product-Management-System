using System.ComponentModel.DataAnnotations;
using PMS.Domain.Common;
using AutoMapper;
using MediatR;
using PMS.Application.Common;
using PMS.Application.Dtos.Proudct;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : BaseRequest,IRequest<Result<ProductDto>>
{
    [Required]
    public Guid Id { get; set; }
}

internal class DeleteProductCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper)
    : BaseRequestHandler<DeleteProductCommand, Result<ProductDto>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<ProductDto>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var product = await UnitOfWork.Repository<Product>()
                .GetByIdAsync(command.Id);
            
         
            if (product == null)
                return Result<ProductDto>.Fail("You do not have permissions to delete the product image.");
            
            await UnitOfWork.Repository<Product>().DeleteAsync(command.Id);
            product.AddDomainEvent(new ProductDeletedEvent(product));

            await UnitOfWork.CommitAsync(cancellationToken);
            return Result<ProductDto>.Success(Mapper.Map<ProductDto>(product));
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<ProductDto>.Fail(ex.Message);
        }
    }
}