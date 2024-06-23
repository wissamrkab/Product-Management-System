using FluentValidation;
using PMS.Application.Features.Categories.Commands.CreateCategory;
using PMS.Application.Helpers;
using PMS.Domain.Exceptions;

namespace PMS.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.NameEn)
            .NotEmpty()
            .Length(1,255)
            .WithExceptionCode(CategoriesExceptions.NameMustBeBetween0And255);
        
        RuleFor(x => x.NameAr)
            .NotEmpty()
            .Length(1,255)
            .WithExceptionCode(CategoriesExceptions.NameMustBeBetween0And255);
    }
}