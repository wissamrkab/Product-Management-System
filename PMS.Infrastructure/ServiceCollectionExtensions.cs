using PMS.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PMS.Application.Interfaces;

namespace PMS.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddSingleton<ILoggingService, SerilogLoggingService>();
    }
    
    
}