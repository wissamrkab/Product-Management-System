using FluentValidation;
using PMS.Application.Helpers;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Entities;
using PMS.Domain.Exceptions;

namespace PMS.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateProductCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1,255)
            .WithExceptionCode(ProductsExceptions.NameMustBeBetween0And255);
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithExceptionCode(ProductsExceptions.PriceMustBeMoreThanZero);
        
        RuleFor(x => x.Isbn)
            .NotEmpty()
            .Length(13).WithExceptionCode(ProductsExceptions.IsbnLengthMustBe13)
            .MustAsync(BeUniqueIsbn).WithExceptionCode(ProductsExceptions.IsbnMustBeUnique);
        
        RuleFor(x => x.Categories)
            .NotEmpty().WithExceptionCode(CategoriesExceptions.AtLeastOneCategoryIsRequired);
    }

    private async Task<bool> BeUniqueIsbn(string isbn, CancellationToken cancellationToken)
    {
        return !(await _unitOfWork.Repository<Product>().GetAllAsync(p => p.Isbn == isbn)).Any();
    }
}