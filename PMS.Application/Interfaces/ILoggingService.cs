namespace PMS.Application.Interfaces;

public interface ILoggingService
{
    public void Information(string messageTemplate, params object[] propertyValues);
    public void Warning(string messageTemplate, params object[] propertyValues);
    public void Error(string messageTemplate, params object[] propertyValues);
    public void Debug(string messageTemplate, params object[] propertyValues);
    public void Dispose();
}