using PMS.Domain.Common;
using PMS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace PMS.Persistence.Contexts;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; } 
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        var domainEntities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            var eventType = domainEvent.GetType().FullName;
            if (eventType != null)
            {
                var outboxMessage =
                    new OutboxMessage(eventType, JsonConvert.SerializeObject(domainEvent,new JsonSerializerSettings()
                    { 
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                OutboxMessages.Add(outboxMessage);
            }
        }

        await base.SaveChangesAsync(cancellationToken);
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}