using PMS.Domain.Common;

namespace PMS.Domain.Exceptions;

public static class ProductsExceptions
{
    public static readonly ExceptionCode ProductDoesNotExist = new()
    {
        Code = "Product.ProductDoesNotExist",
        Description = "Product does not exist."
    };
    
    public static readonly ExceptionCode NameMustBeBetween0And255 = new ExceptionCode(
        "Product.NameMustBeBetween0and255",
        "Name must be between 0 and 255.");
    
    public static readonly ExceptionCode PriceMustBeMoreThanZero = new ExceptionCode(
        "Product.PriceMustBeMoreThanZero",
        "Price Must Be More Than Zero.");
    
    public static readonly ExceptionCode IsbnLengthMustBe13 = new ExceptionCode(
        "Product.IsbnLengthMustBe13",
        "Isbn Length Must Be 13.");
    
    public static readonly ExceptionCode IsbnMustBeUnique = new ExceptionCode(
        "Product.IsbnMustBeUnique",
        "There is a product with the same ISBN.");
}