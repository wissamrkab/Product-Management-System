using PMS.Application.Dtos.UserDtos;

namespace PMS.Application.Dtos.Audit;

public abstract class Auditable : Base
{
    public UserDto? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    
    public UserDto? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}