#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Helsi.Test_Task.MongoDb.Settings;

public class MongoDbSettings
{
    public string DatabaseName { get; set; }

    public string ConnectionString { get; set; }
}
