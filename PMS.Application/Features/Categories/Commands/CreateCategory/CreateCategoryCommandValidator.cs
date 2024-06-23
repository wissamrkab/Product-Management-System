using FluentValidation;
using PMS.Application.Features.Products.Commands.CreateProduct;
using PMS.Application.Helpers;
using PMS.Domain.Entities;
using PMS.Domain.Exceptions;

namespace PMS.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
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