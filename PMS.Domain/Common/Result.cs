using AutoMapper;

namespace PMS.Domain.Common;
public class ResultError
{
    public string ErrorMessage { get; set; } = null!;
    public string ErrorCode { get; set; } = null!;
}

public class Result<T>
{
    
    
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public List<ResultError>? Errors { get; private set; }
  
    private Result(bool isSuccess, T? data, List<ResultError>? errors)
    {
        IsSuccess = isSuccess;
        Data = data;
        Errors = errors;
    }

    public static Result<T> Success(T data) => new(true, data, null);
    
    public static Result<T> Fail(string errorCode, string errorMessage) =>
        new(false, default, [new ResultError(){ErrorCode = errorCode, ErrorMessage = errorMessage}]);
    
    public static Result<T> Fail(string errorMessage) =>
        new(false, default, [new ResultError(){ErrorCode = errorMessage, ErrorMessage = errorMessage}]);
    
    public static Result<T> Fail(ExceptionCode exceptionCode) => 
        new(false, default, [new ResultError(){ErrorCode = exceptionCode.Code, ErrorMessage = exceptionCode.Description}]);
    
    public static Result<T> Fail(IEnumerable<ExceptionCode> exceptionCodes) => 
        new(false, default, exceptionCodes.Select(code => new ResultError(){ErrorCode = code.Code,ErrorMessage = code.Description}).ToList());

    
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(ExceptionCode value) => Fail(value);
    public static implicit operator Result<T>(List<ExceptionCode> value) => Fail(value);

    public Result<T1> ToDtoResult<T1>(IMapper mapper)
    {
        var result = new Result<T1>(IsSuccess, mapper.Map<T1>(Data), Errors);

        return result;
    }
}
