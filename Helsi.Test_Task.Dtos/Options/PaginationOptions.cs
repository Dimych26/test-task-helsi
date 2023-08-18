
namespace Helsi.Test_Task.Dtos.Options;

public record PaginationOptions
{
    public static PaginationOptions Default { get; } = new()
    {
        PageNumber = 1,
        PageSize = 10
    };

    public int PageNumber { get; init; }

    public int PageSize { get; init; }
}
