using Microsoft.AspNetCore.Identity;

namespace PMS.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FirebaseToken { get; set; } = null!;
}