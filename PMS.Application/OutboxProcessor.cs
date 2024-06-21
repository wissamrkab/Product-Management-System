using PMS.Domain.Entities;
using PMS.Domain.Interfaces;
using PMS.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace PMS.Application;

public class OutboxProcessor(
    IServiceProvider serviceProvider)
    : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessOutboxMessagesAsync(stoppingToken);
            await Task.Delay(10000, stoppingToken); // Delay between polls
        }
    }
    private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
    
        var messages = await unitOfWork.Repository<OutboxMessage>()
            .GetAllAsync(message => message.ProcessedAt == null);

        foreach (var message in messages)
        {
            try
            {
                var eventType = Type.GetType(message.EventType);
                if (eventType != null)
                {
                    if (JsonConvert.DeserializeObject(message.EventData, eventType) is INotification domainEvent)
                    {
                        await mediator.Publish(domainEvent, cancellationToken);
                    }
                }

                // Mark the message as processed
                message.SetProcessed();
                await unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                loggingService.Error($"Error processing outbox message {message.Id}: {ex.Message}");
            }
        }
    }
}