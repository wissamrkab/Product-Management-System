using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PMS.Domain.Common;
using PMS.Domain.Exceptions;

namespace PMS.Domain.Entities;

public class Category : AuditableEntity
{
    [MaxLength(255)]
    public string NameEn { get; private set; } = null!;
    
    [MaxLength(255)]
    public string NameAr { get; private set; } = null!;
    
    public Guid? ParentId { get; private set; }
    public Category? Parent { get; private set; }

    private readonly List<Product> _products = [];
    public IReadOnlyCollection<Product> Products => _products;
    
    private List<Category> _subCategories = new();
    public IReadOnlyCollection<Category> SubCategories => _subCategories;
    
    public Category()
    {
    }

    private Category(string nameEn, string nameAr,Guid? parentId, IdentityUser? createdBy)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        ParentId = parentId;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;
    }
    
    public static Result<Category> Create(string nameEn, string nameAr,Guid? parentId, IdentityUser? createdBy)
    {
        var validationResult = ValidateDomainRules(nameEn, nameAr);
        if (!validationResult.IsSuccess) return validationResult;

        var category = new Category(nameEn, nameAr, parentId, createdBy);

        return category;
    }
    
    public Result<Category> Update(string nameEn, string nameAr,Guid? parentId, IdentityUser? updatedBy)
    {
        var validationResult = ValidateDomainRules(nameEn, nameAr);
        if (!validationResult.IsSuccess) return validationResult;

        NameEn = nameEn;
        NameAr = nameAr;
        ParentId = parentId;

        return this;
    }

    public void SetSubCategories(ICollection<Category>? categories)
    {
        _subCategories = categories!.ToList();
    }
    
    private static Result<Category> ValidateDomainRules(string nameEn, string nameAr)
    {
        List<ExceptionCode> exceptionCodes = [];
      
        return exceptionCodes.Count > 0 ? exceptionCodes : Result<Category>.Success(null!);
    }
}