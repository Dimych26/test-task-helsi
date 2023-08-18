using Ardalis.Result;

namespace Helsi.Test_Task.Infrastructure.Utils;

public class InvalidResultBuilder
{
    private readonly List<ValidationError> _errors = new();

    public InvalidResultBuilder AddError(string identifier, string errorCode, string? errorMessage = null)
    {
        _errors.Add(new ValidationError()
        {
            Identifier = identifier,
            ErrorCode = errorCode,
            ErrorMessage = errorMessage ?? errorCode
        });

        return this;
    }

    public Result Build()
    {
        return Result.Invalid(_errors);
    }
}