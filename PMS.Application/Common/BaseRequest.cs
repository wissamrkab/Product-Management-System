namespace PMS.Application.Common;

public abstract class BaseRequest
{
    private string? UserEmail { get; set; }
    
    public void SetEmail(string? email)
    {
        UserEmail = email;
    }

    public string? GetEmail() => UserEmail;
}