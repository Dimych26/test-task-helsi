
using FluentValidation;
using Helsi.Test_Task.Contracts.Services;
using Helsi.Test_Task.Dtos.TaskList;
using Helsi.Test_Task.Infrastructure.Resources;

namespace Helsi.Test_Task.Services.Validators.TaskList;

public class TaskListUpdateDtoValidator : AbstractValidator<TaskListUpdateDto>
{
    public TaskListUpdateDtoValidator(ITaskListService taskListService)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (x, _) => await taskListService.ExistsAsync(x))
            .WithErrorCode(ValidationErrorCodes.TaskListNotFound);

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 255);
    }
}


