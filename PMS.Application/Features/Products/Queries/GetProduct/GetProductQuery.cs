using System.ComponentModel.DataAnnotations;
using PMS.Domain.Common;
using PMS.Domain.Exceptions;
using PMS.Domain.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using PMS.Application.Common;
using PMS.Application.Dtos.Proudct;

namespace PMS.Application.Features.Products.Queries.GetProduct;

public class GetProductQuery : BaseRequest,IRequest<Result<ProductDto>>
{
    [Required]
    public Guid Id { get; set; }

}

internal class GetProductQueryHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IMapper mapper,
    IProductRepository repository)
    : BaseRequestHandler<GetProductQuery, Result<ProductDto>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await repository.GetFullProductById(request.Id);
            
            return Result<ProductDto>.Success(Mapper.Map<ProductDto>(product));
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<ProductDto>.Fail(ex.Message);
        }


    }

  
}