namespace PMS.Domain.ValueObjects;

public class TokenDto
{
    public string Value { get; set; } = null!;
    public DateTime ExpireDate { get; set; }
}