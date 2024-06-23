using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PMS.Application.Interfaces;
using TokenDto = PMS.Domain.ValueObjects.TokenDto;

namespace PMS.Application.Helpers;

public class UserAuthService : IUserAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserAuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TokenDto> GetToken(IdentityUser user)
    {
        
        var userRoles = await _userManager.GetRolesAsync(user);
        
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenDto = new TokenDto()
        {
            Value = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireDate = token.ValidTo
        };

        return tokenDto;
    }

    public async Task<IdentityUser?> GetAuthenticatedUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user is not { Identity.IsAuthenticated: true }) return null;
        
        var email = user.FindFirst(ClaimTypes.Name)?.Value;
        if (email == null) return null;
        
        var appUser = await _userManager.FindByEmailAsync(email);
        return appUser ?? null;
    }
}