using Microsoft.AspNetCore.Identity;
using PMS.Domain.ValueObjects;

namespace PMS.Application.Interfaces;

public interface IUserAuthService
{
    public Task<TokenDto> GetToken(IdentityUser user);

    public Task<IdentityUser?> GetAuthenticatedUser();
}