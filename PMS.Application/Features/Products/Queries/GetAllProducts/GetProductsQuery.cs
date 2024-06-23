using PMS.Domain.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PMS.Application.Common;
using PMS.Application.Dtos;
using PMS.Application.Dtos.Proudct;
using PMS.Application.Interfaces.Repositories;

namespace PMS.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQuery : BaseRequest,IRequest<Result<PaginationDto<List<ProductDto>>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; }  = 10;
    public string SearchCriteria { get; set; }  = "";
    public List<Guid>? CategoryIds { get; set; }
}

internal class GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper, IProductRepository productRepository)
    : BaseRequestHandler<GetAllProductsQuery, Result<PaginationDto<List<ProductDto>>>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<PaginationDto<List<ProductDto>>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await productRepository.GetFullProductList(
                request.Page, 
                request.PageSize,
                request.SearchCriteria,
                request.CategoryIds
                );
            
            var totalRecords = await productRepository.GetProductCountAsync(
                request.SearchCriteria,
                request.CategoryIds
                );
            
            var totalPages = (int) Math.Ceiling(totalRecords / (double) request.PageSize);

            var paginationMetadata = new PaginationDto<List<ProductDto>>()
            {
                TotalRecords = totalRecords,
                PageSize = request.PageSize,
                CurrentPage = request.Page,
                TotalPages = totalPages,
                Data = Mapper.Map<List<ProductDto>>(products)
            };
            
            return Result<PaginationDto<List<ProductDto>>>.Success(paginationMetadata); 
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<PaginationDto<List<ProductDto>>>.Fail(ex.Message);
        }
    }
}