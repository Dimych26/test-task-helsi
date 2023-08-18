namespace Helsi.Test_Task.Infrastructure.Resources;

public static class ValidationErrorCodes
{
    public static string UserIdNotProvided { get; } = "UserIdNotProvided";

    public static string NotAllow { get; } = "NotAllow";

    public static string UserNotFound { get; } = "UserNotFound";

    public static string TaskListNotFound { get; } = "TaskListNotFound";

    public static string AlreadyObserver { get; } = "AlreadyObserver";

    public static string NotAnObserver { get; } = "NotAnObserver";

}