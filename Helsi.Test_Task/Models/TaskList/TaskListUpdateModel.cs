#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


namespace Helsi.Test_Task.WebApi.Models.TaskList;

public record TaskListUpdateModel
{
    public string Id { get; init; }

    public string Name { get; init; }
}
