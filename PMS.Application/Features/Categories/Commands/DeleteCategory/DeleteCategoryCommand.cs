using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using PMS.Application.Common;
using PMS.Application.Dtos.Category;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Common;
using PMS.Domain.Entities;
using PMS.Domain.Exceptions;

namespace PMS.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : BaseRequest,IRequest<Result<CategoryDto>>
{
    [Required]
    public Guid Id { get; set; }
}

internal class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper)
    : BaseRequestHandler<DeleteCategoryCommand, Result<CategoryDto>>(unitOfWork, mediator, mapper)
{
    public override async Task<Result<CategoryDto>> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var category = await UnitOfWork.Repository<Category>()
                .GetByIdAsync(command.Id);
            
         
            if (category == null)
                return CategoriesExceptions.CategoryDoesNotExist;
            
            await UnitOfWork.Repository<Category>().DeleteAsync(command.Id);
            category.AddDomainEvent(new CategoryDeletedEvent(category));

            await UnitOfWork.CommitAsync(cancellationToken);
            return Result<CategoryDto>.Success(Mapper.Map<CategoryDto>(category));
        }
        catch (Exception ex)
        {
            await UnitOfWork.Rollback();

            return Result<CategoryDto>.Fail(ex.Message);
        }
    }
}