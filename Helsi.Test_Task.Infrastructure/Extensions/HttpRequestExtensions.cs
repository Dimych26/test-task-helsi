using Microsoft.AspNetCore.Http;

namespace Helsi.Test_Task.Infrastructure.Extensions;
public static class HttpRequestExtensions
{
    public static bool TryGetUserId(this HttpRequest request, out string userId)
    {
        foreach (var header in request.Headers)
        {
            if (header.Key.Equals("userId", StringComparison.OrdinalIgnoreCase))
            {
                userId = header.Value.ToString();
                return true;
            }
        }

        userId = string.Empty;
        return false;
    }

    public static string GetUserId(this HttpRequest request)
    {
        return !request.TryGetUserId(out var userId)
            ? throw new InvalidOperationException("UserId wasn`t found in request headers")
            : userId;
    }
}
