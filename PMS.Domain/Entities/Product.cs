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

    [StringLength(13, MinimumLength = 13)] public string Isbn { get; private set; } = null!;
    
    public decimal Price { get; private set; }
   
    private List<Category> _categories = [];
    public IReadOnlyCollection<Category> Categories => _categories;
    
    public Product() { }
    
    private Product(string name, string isbn, decimal price, List<Category> categories, IdentityUser? createdBy)
    {
        Name = name;
        Isbn = isbn;
        Price = price;
        _categories = categories;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;
    }

    public static Result<Product> Create(string name, string isbn, decimal defaultPrice, List<Category> categories, IdentityUser? createdBy)
    {
        var validationResult = ValidateDomainRules(name,isbn, defaultPrice);
        if (!validationResult.IsSuccess) return validationResult;

        var product = new Product(name, isbn, defaultPrice,categories, createdBy);

        return product;
    }
    
    public Result<Product> Update(string name, string isbn, decimal price, List<Category> categories, IdentityUser? updatedBy)
    {
        var validationResult = ValidateDomainRules(name, isbn, price);
        if (!validationResult.IsSuccess) return validationResult;
        
        Name = name;
        Isbn = isbn;
        Price = price;
        _categories = categories;
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
        
        return this;
    }
    private static Result<Product> ValidateDomainRules(string name,string isbn, decimal price)
    {
        List<ExceptionCode> exceptionCodes = [];
      
        return exceptionCodes.Count > 0 ? exceptionCodes : Result<Product>.Success(null!);
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