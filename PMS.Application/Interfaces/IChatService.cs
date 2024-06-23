namespace PMS.Application.Interfaces;

public interface IChatService
{

    public string GenerateUserToken(string userId);
     Task<HttpResponseMessage> HttpRegisterAgoraUser( string body,string nickname ,
        CancellationToken cancellationToken);
}