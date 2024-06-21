namespace PMS.Domain.Interfaces.Repositories;

public interface IChatService
{

    public string GenerateUserToken(string userId);
     Task<HttpResponseMessage> HttpRegisterAgoraUser( string body,string nickname ,
        CancellationToken cancellationToken);
}