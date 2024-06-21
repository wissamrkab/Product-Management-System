using PMS.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace PMS.Domain.Common;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public IdentityUser? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    
    public IdentityUser? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}