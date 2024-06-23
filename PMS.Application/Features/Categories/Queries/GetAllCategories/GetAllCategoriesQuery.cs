using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common;
using PMS.Application.Dtos.Category;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Common;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQuery : BaseRequest,IRequest<Result<List<CategoryDto>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; }  = 10;
    public Guid? ParentId { get; set; }
    public bool AllCategories { get; set; } = false;
}

internal class GetAllCategoriesQueryHandler : BaseRequestHandler<GetAllCategoriesQuery, Result<List<CategoryDto>>>
{
    public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) : base(unitOfWork, mediator, mapper)
    {
    }

    public override async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allCategories  = await UnitOfWork.Repository<Category>().GetAllAsync(
                request.Page, 
                request.PageSize,
                category => request.AllCategories || category.ParentId == request.ParentId );
            
            return Result<List<CategoryDto>>.Success(Mapper.Map<List<CategoryDto>>(allCategories)); 
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<List<CategoryDto>>.Fail(ex.Message);
        }


    }
    
    private async Task<ICollection<Category>?> LoadSubcategoriesRecursively(Category? category)
    {
        if (category == null)
            return null;
        
        category = await UnitOfWork.Repository<Category>().GetByIdAsync(
            category.Id,
            queryable => queryable.Include(qr => qr.SubCategories)
            );
        
        foreach (var subCategory in category!.SubCategories.ToList())
        {
            subCategory.SetSubCategories(await LoadSubcategoriesRecursively(subCategory));
        }

        return category.SubCategories.ToList();
    }
}