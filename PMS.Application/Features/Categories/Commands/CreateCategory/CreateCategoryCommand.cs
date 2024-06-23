using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using PMS.Application.Common;
using PMS.Application.Dtos.Category;
using PMS.Application.Dtos.Proudct;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Common;
using PMS.Domain.Entities;
using PMS.Domain.Exceptions;

namespace PMS.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : BaseRequest,IRequest<Result<CategoryDto>>
{
    [Required] public string NameEn { get; set; } = null!;
    [Required] public string NameAr { get; set; } = null!;
    public Guid? ParentId { get; set; }

}

internal class CreateCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IMapper mapper)
    : BaseRequestHandler<CreateCategoryCommand, Result<CategoryDto>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<CategoryDto>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var categoryResult = Category.Create(
                command.NameEn,
                command.NameAr,
                command.ParentId,
                null
            );

            if (!categoryResult.IsSuccess || categoryResult.Data == null) return categoryResult.ToDtoResult<CategoryDto>(Mapper);
            
            var category = categoryResult.Data;
            category.AddDomainEvent(new CategoryCreatedEvent(category));
            
            await UnitOfWork.Repository<Category>().AddAsync(category);
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