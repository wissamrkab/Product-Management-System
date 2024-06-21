namespace PMS.Infrastructure.Common;

public class CustomHttpClient
{
    private static HttpClient _customHttpClient = new HttpClient();
    

    public HttpClient GetHttpClient()
    {
        return _customHttpClient;
    }
}