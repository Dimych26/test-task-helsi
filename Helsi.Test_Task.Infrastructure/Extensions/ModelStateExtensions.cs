using Ardalis.Result;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using IResult = Ardalis.Result.IResult;

namespace Helsi.Test_Task.Infrastructure.Extensions;

public static class ModelStateExtensions
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, IResult result)
    {
        if (result.Status != ResultStatus.Invalid)
        {
            return;
        }

        foreach (var error in result.ValidationErrors)
        {
            modelState.AddModelError(error.Identifier, error.ErrorCode);
        }
    }
}
