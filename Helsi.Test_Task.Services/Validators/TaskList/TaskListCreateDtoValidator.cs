
using FluentValidation;
using Helsi.Test_Task.Dtos.TaskList;

namespace Helsi.Test_Task.Services.Validators.TaskList;

public class TaskListCreateDtoValidator : AbstractValidator<TaskListCreateDto>
{
    public TaskListCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 255);
    }
}