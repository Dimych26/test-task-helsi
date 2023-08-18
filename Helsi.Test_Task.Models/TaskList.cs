#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Helsi.Test_Task.Models;

public class TaskList : Entity
{
    public string Name { get; set; }

    public DateTime Created { get; set; }

    public User Creator { get; set; }

    public List<User> Observers { get; set; }
}
