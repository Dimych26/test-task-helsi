using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Helsi.Test_Task.Infrastructure.Extensions;
using Helsi.Test_Task.Infrastructure.Resources;
using Helsi.Test_Task.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Helsi.Test_Task.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class UserIdFilterAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controller = (ControllerBase)context.Controller;
        if (!controller.Request.TryGetUserId(out var userId))
        {
            controller.ModelState.AddModelError("userId", ValidationErrorCodes.UserIdNotProvided);
            context.Result = controller.ValidationProblem();
            return;
        }

        var services = controller.HttpContext.RequestServices;
        var userService = services.GetRequiredService<IUserService>();

        var isUserExists = await userService.ExistsAsync(userId);

        if (!isUserExists)
        {
            context.ModelState.AddModelError("userId", ValidationErrorCodes.UserNotFound);
            context.Result = controller.ValidationProblem();
            return;
        }

        await next();
    }
}
