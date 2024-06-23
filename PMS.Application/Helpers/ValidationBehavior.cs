using FluentValidation;
using MediatR;
using PMS.Domain.Common;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace PMS.Application.Helpers;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(result => result.Errors).Where(f => f != null).ToList();

        if (failures.Count == 0) return await next();
        
        var resultType = typeof(TResponse);
            
        var result = CreateFailureResult<TResponse>(failures);
        return result!;

    }
    
    private static T? CreateFailureResult<T>(List<ValidationFailure> errors) where T : class
    {
        var resultType = typeof(TResponse).GetGenericArguments()[0];
        var method = typeof(Result<>).MakeGenericType(resultType).GetMethod("Fail", new Type[] { typeof(List<ExceptionCode>) });
        var result = method?.Invoke(null, new object[] { errors.Select(failure => new ExceptionCode()
        {
            Code = failure.ErrorCode,
            Description = failure.ErrorMessage
        }) });
        return result as T;
    }
}