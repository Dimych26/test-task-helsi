using Ardalis.Result;

namespace Helsi.Test_Task.Infrastructure.Utils;

public static class ResultExtensions
{
    public static Result ToResult<TIn>(this Result<TIn> result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => Result.Success(),
            ResultStatus.NotFound => Result.NotFound(),
            ResultStatus.Unauthorized => Result.Unauthorized(),
            ResultStatus.Forbidden => Result.Forbidden(),
            ResultStatus.Invalid => Result.Invalid(result.ValidationErrors),
            ResultStatus.Error => Result.Error(result.Errors.ToArray()),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }

    public static Result<TOut> ToResult<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> successConvert)
    {
        return result.Status switch
        {
            ResultStatus.Ok => Result.Success(successConvert(result.Value)),
            ResultStatus.NotFound => Result<TOut>.NotFound(),
            ResultStatus.Unauthorized => Result<TOut>.Unauthorized(),
            ResultStatus.Forbidden => Result<TOut>.Forbidden(),
            ResultStatus.Invalid => Result<TOut>.Invalid(result.ValidationErrors),
            ResultStatus.Error => Result<TOut>.Error(result.Errors.ToArray()),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }
}
