namespace Helsi.Test_Task.Infrastructure.Models;

public record PagedListModel<T>
{
    public int PageCount { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalItemsCount { get; init; }

    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
}
