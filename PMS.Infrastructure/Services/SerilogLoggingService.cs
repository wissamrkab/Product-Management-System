using Microsoft.Extensions.Configuration;
using PMS.Application.Interfaces;
using Serilog;
using Serilog.Core;

namespace PMS.Infrastructure.Services;

public class SerilogLoggingService(IConfiguration configuration) : ILoggingService
{
    private readonly Logger _logger = new LoggerConfiguration()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    public void Information(string messageTemplate, params object[] propertyValues)
    {
        _logger.Information(messageTemplate, propertyValues);
    }

    public void Warning(string messageTemplate, params object[] propertyValues)
    {
        _logger.Warning(messageTemplate, propertyValues);
    }

    public void Error(string messageTemplate, params object[] propertyValues)
    {
        _logger.Error(messageTemplate, propertyValues);
    }

    public void Debug(string messageTemplate, params object[] propertyValues)
    {
        _logger.Debug(messageTemplate, propertyValues);
    }

    public void Dispose()
    {
       _logger.Dispose();
    }
}