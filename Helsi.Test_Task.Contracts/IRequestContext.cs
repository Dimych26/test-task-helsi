
namespace Helsi.Test_Task.Contracts;

public interface IRequestContext
{
    /// <exception cref="InvalidOperationException">if userId is null</exception>
    string UserId { get; }

    bool TryGetUserId(out string userId);
}
