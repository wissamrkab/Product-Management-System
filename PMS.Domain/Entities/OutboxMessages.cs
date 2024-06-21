using PMS.Domain.Common;

namespace PMS.Domain.Entities;

public class OutboxMessage(string eventType, string eventData) : AuditableEntity
{
    public string EventType { get; private set; } = eventType;
    public string EventData { get; private set; } = eventData;
    public DateTime? ProcessedAt  { get; private set; }

    public void SetProcessed()
    {
        ProcessedAt = DateTime.UtcNow;
    }
}