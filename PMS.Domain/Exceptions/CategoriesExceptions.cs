using PMS.Domain.Common;

namespace PMS.Domain.Exceptions;

public static class CategoriesExceptions
{
    public static readonly ExceptionCode CategoryDoesNotExist = new()
    {
        Code = "Category.CategoryDoesNotExist",
        Description = "Category does not exist."
    };
    
    public static readonly ExceptionCode NameMustBeBetween0And255 = new ExceptionCode(
        "Category.NameMustBeBetween0and255",
        "Name must be between 0 and 255.");

    public static ExceptionCode SpecificCategoryDoesNotExist(Guid id)
    {
        var error = CategoryDoesNotExist;
        error.Description = $"Category with {id} does not exist.";
        
        return error;
    }
}