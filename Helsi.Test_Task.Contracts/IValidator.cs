using FluentValidation.Results;

namespace Helsi.Test_Task.Contracts;

public interface IValidator
{
    Task<ValidationResult> ValidateAsync<T>(T obj);
}