
using FluentValidation;
using Helsi.Test_Task.Contracts.Services;
using Helsi.Test_Task.Dtos.TaskList;
using Helsi.Test_Task.Infrastructure.Resources;

namespace Helsi.Test_Task.Services.Validators.TaskList;

public class LinkUserDtoValidator : AbstractValidator<LinkUserDto>
{
    public LinkUserDtoValidator(ITaskListService taskListService, IUserService userService)
    {
        RuleFor(x => x.TaskListId)
            .NotEmpty()
            .MustAsync(async (x, _) => await taskListService.ExistsAsync(x))
            .WithErrorCode(ValidationErrorCodes.TaskListNotFound);

        RuleFor(x => x.ObserverId)
            .NotEmpty()
            .MustAsync(async (x, _) => await userService.ExistsAsync(x))
            .WithErrorCode(ValidationErrorCodes.UserNotFound);
    }
}


