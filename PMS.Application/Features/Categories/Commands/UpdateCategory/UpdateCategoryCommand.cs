using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using PMS.Application.Common;
using PMS.Application.Dtos.Category;
using PMS.Application.Features.Categories.Commands.CreateCategory;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Common;
using PMS.Domain.Entities;
using PMS.Domain.Exceptions;

namespace PMS.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : BaseRequest,IRequest<Result<CategoryDto>>
{
    [Required] public Guid Id { get; set; }
    [Required] public string NameEn { get; set; } = null!;
    [Required] public string NameAr { get; set; } = null!;
    public Guid? ParentId { get; set; }

}

internal class UpdateCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IMapper mapper)
    : BaseRequestHandler<UpdateCategoryCommand, Result<CategoryDto>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var category = await UnitOfWork.Repository<Category>().GetByIdAsync(command.Id);

            if (category == null) return CategoriesExceptions.CategoryDoesNotExist;
            
            var categoryResult = category.Update(
                command.NameEn,
                command.NameAr,
                command.ParentId,
                null
            );
            if (!categoryResult.IsSuccess) return categoryResult.ToDtoResult<CategoryDto>(Mapper);
            
            category.AddDomainEvent(new CategoryCreatedEvent(category));
            
            UnitOfWork.Repository<Category>().UpdateAsync(category);
            await UnitOfWork.CommitAsync(cancellationToken);
            
            return categoryResult.ToDtoResult<CategoryDto>(Mapper);
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<CategoryDto>.Fail(ex.Message);
        }
    }
}