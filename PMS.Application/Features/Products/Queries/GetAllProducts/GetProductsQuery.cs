using PMS.Domain.Common;
using PMS.Domain.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PMS.Application.Common;
using PMS.Application.Dtos.Proudct;

namespace PMS.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery : BaseRequest,IRequest<Result<List<ProductDto>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; }  = 10;
    public string SearchCriteria { get; set; }  = "";
}

internal class GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper)
    : BaseRequestHandler<GetAllProductsQuery, Result<List<ProductDto>>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<List<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            
            var products = await UnitOfWork.Repository<Domain.Entities.Product>().GetAllAsync(
                request.Page, 
                request.PageSize,
                product => product.Name.Contains(request.SearchCriteria)
                );
            
            return Result<List<ProductDto>>.Success(Mapper.Map<List<ProductDto>>(products)); 
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<List<ProductDto>>.Fail(ex.Message);
        }
    }
}