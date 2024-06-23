using PMS.Domain.Common.Interfaces;
using MediatR;

namespace PMS.Domain.Common;

public class DomainEventDispatcher(
    IPublisher mediator
    ) : IDomainEventDispatcher
{
    public async Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents)
    {
        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();

            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            { 
                await mediator.Publish(domainEvent);
            }
        }
    }
}