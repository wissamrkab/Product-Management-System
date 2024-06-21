using PMS.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace PMS.Domain.Interfaces;

public interface IUserAuthService
{
    public Task<TokenDto> GetToken(IdentityUser user);

    public Task<IdentityUser?> GetAuthenticatedUser();
}