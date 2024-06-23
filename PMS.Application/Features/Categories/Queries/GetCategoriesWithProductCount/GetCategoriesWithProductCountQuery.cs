using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common;
using PMS.Application.Dtos.Category;
using PMS.Application.Features.Categories.Queries.GetAllCategories;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Common;
using PMS.Domain.Entities;

namespace PMS.Application.Features.Categories.Queries.GetCategoriesWithProductCount;

public class GetCategoriesWithProductCountQuery : BaseRequest,IRequest<Result<List<CategoryWithProductCountDto>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; }  = 10;
    public Guid? ParentId { get; set; }
    public bool AllCategories { get; set; } = false;
}

internal class GetCategoriesWithProductCountQueryHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper, ICategoryRepository categoryRepository)
    : BaseRequestHandler<GetCategoriesWithProductCountQuery, Result<List<CategoryWithProductCountDto>>>(unitOfWork,
        mediator, mapper)
{
    public override async Task<Result<List<CategoryWithProductCountDto>>> Handle(GetCategoriesWithProductCountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allCategories  = await categoryRepository.GetCategoriesWithProductCountAsync();
            
            return Result<List<CategoryWithProductCountDto>>.Success(Mapper.Map<List<CategoryWithProductCountDto>>(allCategories)); 
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<List<CategoryWithProductCountDto>>.Fail(ex.Message);
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