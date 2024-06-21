using PMS.Domain.Common;

namespace PMS.Domain.Exceptions;

public static class UserExceptions
{
    public static readonly ExceptionCode UserUnauthorized = new()
    {
        Code = "User.Unauthorized",
        Description = "You do not have permission to access this resource."
    };
}