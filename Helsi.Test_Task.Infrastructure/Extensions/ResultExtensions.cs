using System.Text;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using IResult = Ardalis.Result.IResult;

namespace Helsi.Test_Task.Infrastructure.Extensions;

public static class ResultExtensions
{
    public static IActionResult ConvertToActionResult<T>(this ActionResult<T> result)
    {
        var converableResult = (IConvertToActionResult)result;
        return converableResult.Convert();
    }

    public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        return controller.ToActionResult((IResult)result);
    }

    public static ActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        return controller.ToActionResult((IResult)result);
    }

    public static ActionResult<T> ToActionResult<T>(this ControllerBase controller,
      Result<T> result)
    {
        return controller.ToActionResult((IResult)result);
    }

    public static ActionResult ToActionResult(this ControllerBase controller,
      Result result)
    {
        return controller.ToActionResult((IResult)result);
    }

    private static ActionResult ToActionResult(this ControllerBase controller, IResult result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => typeof(Result).IsInstanceOfType(result)
                ? controller.Ok()
                : controller.Ok(result.GetValue()),
            ResultStatus.NotFound => controller.NotFound(),
            ResultStatus.Unauthorized => controller.Unauthorized(),
            ResultStatus.Forbidden => controller.Forbid(),
            ResultStatus.Invalid => ValidationProblem(controller, result),
            ResultStatus.Error => UnprocessableEntity(controller, result),
            _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
        };
    }

    private static ActionResult ValidationProblem(ControllerBase controller, IResult result)
    {
        controller.ModelState.AddValidationErrors(result);
        return controller.ValidationProblem();
    }

    private static ActionResult UnprocessableEntity(ControllerBase controller, IResult result)
    {
        var details = new StringBuilder("Next error(s) occured:");

        foreach (var error in result.Errors)
        {
            details.Append("* ").Append(error).AppendLine();
        }

        return controller.UnprocessableEntity(new ProblemDetails
        {
            Title = "Something went wrong.",
            Detail = details.ToString()
        });
    }
}
