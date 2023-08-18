#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Helsi.Test_Task.Dtos.User;

public record UserDto
{
    public string Id { get; init; }

    public string Name { get; init; }
}
