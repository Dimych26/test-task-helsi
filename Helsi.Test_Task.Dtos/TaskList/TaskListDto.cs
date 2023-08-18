using Helsi.Test_Task.Dtos.User;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Helsi.Test_Task.Dtos.TaskList;

public record TaskListDto
{
    public string Id { get; init; }

    public string Name { get; init; }

    public DateTime Created { get; init; }

    public UserDto Creator { get; init; }

    public IReadOnlyList<UserDto> Observers { get; init; }
}
