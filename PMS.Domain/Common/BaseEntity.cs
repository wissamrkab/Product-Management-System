using System.ComponentModel.DataAnnotations.Schema;
using PMS.Domain.Common.Interfaces;

namespace PMS.Domain.Common;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();
    private readonly List<BaseEvent> _domainEvents = new();

    public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(BaseEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public Result<BaseEntity> ToSuccessResult()
    {
        return Result<BaseEntity>.Success(this);
    }
}