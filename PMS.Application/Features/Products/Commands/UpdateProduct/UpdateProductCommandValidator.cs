using FluentValidation;
using PMS.Application.Helpers;
using PMS.Application.Interfaces.Repositories;
using PMS.Domain.Entities;
using PMS.Domain.Exceptions;

namespace PMS.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateProductCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithExceptionCode(ProductsExceptions.ProductDoesNotExist);
        
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
            .MustAsync((command, isbn, cancellationToken) => BeUniqueIsbn(isbn, command.Id, cancellationToken)).WithExceptionCode(ProductsExceptions.IsbnMustBeUnique);
        
        RuleFor(x => x.Categories)
            .NotEmpty().WithExceptionCode(CategoriesExceptions.AtLeastOneCategoryIsRequired);
    }

    private async Task<bool> BeUniqueIsbn(string isbn,Guid id, CancellationToken cancellationToken)
    {
        return !(await _unitOfWork.Repository<Product>().GetAllAsync(p => p.Isbn == isbn && p.Id != id)).Any();
    }
}