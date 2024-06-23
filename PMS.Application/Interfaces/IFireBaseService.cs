namespace PMS.Application.Interfaces;

public interface IFireBaseService
{
    public Task<bool> CheckFireBaseUser(string fireBaseUserToken);
    public Task<bool> CheckIfUpdatedPhoneNumber(string fireBaseUserToken, string phoneNumber);
    public Task<string> SendNotificationAsync(string title, string body, string token);

}