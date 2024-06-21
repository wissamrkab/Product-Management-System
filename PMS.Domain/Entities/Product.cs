using System.ComponentModel.DataAnnotations;
using PMS.Domain.Common;
using PMS.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PMS.Domain.Entities;

public class Product : AuditableEntity
{
    [MaxLength(255)]
    public string Name { get; private set; } = null!;
    
    [StringLength(13, MinimumLength = 13)]
    public string Isbn { get; private set; }
    
    public decimal Price { get; private set; }
   
    private readonly List<Category> _categories = [];
    public IReadOnlyCollection<Category> Categories => _categories;
    
    public Product() { }
    
    private Product(string name, string isbn, decimal price, IdentityUser? createdBy)
    {
        Name = name;
        Isbn = isbn;
        Price = price;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;
    }

    public static Result<Product> Create(string name, string isbn, decimal defaultPrice, IdentityUser? createdBy)
    {
        var validationResult = Validate(name,isbn, defaultPrice);
        if (validationResult != null) return validationResult;

        var product = new Product(name, isbn, defaultPrice, createdBy);

        return product;
    }
    
    public Result<Product> Update(string name, string isbn, decimal price, IdentityUser updatedBy)
    {
        var validationResult = Validate(name, isbn, price);
        if (validationResult != null) return validationResult;
        
        Name = name;
        Isbn = isbn;
        Price = price;
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
        
        return this;
    }
    private static Result<Product>? Validate(string name,string isbn, decimal price)
    {
        List<ExceptionCode> exceptionCodes = [];
        if (name.Length is not(> 0 and < 255)) exceptionCodes.Add(ProductsExceptions.NameMustBeBetween0And255);
        if (isbn.Length is not 13) exceptionCodes.Add(ProductsExceptions.IsbnLengthMustBe13);
        if (price is <= 0) exceptionCodes.Add(ProductsExceptions.PriceMustBeMoreThanZero);

        return (exceptionCodes.Count > 0 ? exceptionCodes : null)!;
    }
    
    public class ProductConfiguration: IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Isbn)
                .HasMaxLength(13)
                .IsRequired();
            
            builder.HasIndex(p => p.Isbn)
                .IsUnique();
        }
    }
}