namespace PMS.Domain.Interfaces;

public interface IVerifyService
{
    public bool SendVerificationCode(string phoneNumber);
    public bool CheckVerificationCode(string phoneNumber, string code);
    public bool IsPhoneNumberValid(string phoneNumber);
}