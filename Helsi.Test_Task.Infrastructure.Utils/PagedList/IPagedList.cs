namespace Helsi.Test_Task.Infrastructure.Utils.PagedList;

public interface IPagedList<T> : IReadOnlyList<T>
{
    public int PageCount { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalItemsCount { get; }
}
