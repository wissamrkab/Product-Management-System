using Microsoft.AspNetCore.Identity;
using PMS.Application.Common.Mapping;

namespace PMS.Application.Dtos.UserDtos;

public class UserDto : IMapFrom<IdentityUser>
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    
}