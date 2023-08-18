
using FluentValidation;
using Helsi.Test_Task.Dtos.User;

namespace Helsi.Test_Task.Services.Validators.User;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
