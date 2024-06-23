using Newtonsoft.Json;

namespace PMS.Domain.Common;

public class ExceptionCode
{
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ExceptionCode()
    {
    }

    public ExceptionCode(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string ToJsonString()
    {
        return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }
}