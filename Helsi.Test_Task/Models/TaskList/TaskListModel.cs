#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


using Helsi.Test_Task.WebApi.Models.User;

namespace Helsi.Test_Task.WebApi.Models.TaskList;

public record TaskListModel
{
    public string Id { get; init; }

    public string Name { get; init; }

    public DateTime Created { get; init; }

    public UserModel Creator { get; init; }

    public IReadOnlyList<UserModel> Observers { get; init; }
}
