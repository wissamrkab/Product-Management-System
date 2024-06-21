using Microsoft.AspNetCore.Identity;

namespace PMS.Domain.Common.Interfaces;

public interface IAuditableEntity : IBaseEntity
{
    IdentityUser? CreatedBy { get; set; }
    DateTime? CreatedDate { get; set; }
    
    IdentityUser? UpdatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
}