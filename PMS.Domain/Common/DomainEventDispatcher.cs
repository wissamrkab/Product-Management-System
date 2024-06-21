using PMS.Domain.Common.Interfaces;
using PMS.Domain.Interfaces;
using MediatR;

namespace PMS.Domain.Common;

public class DomainEventDispatcher(
    IPublisher mediator,
    ILoggingService loggingService
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
                try
                {
                    await mediator.Publish(domainEvent);
                }
                catch (Exception ex)
                {
                    loggingService.Error($"Error dispatching domain event: {ex.Message}");
                }
            }
        }
    }
}