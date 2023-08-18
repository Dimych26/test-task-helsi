using System.Collections;

namespace Helsi.Test_Task.Infrastructure.Utils.PagedList;

public class PagedList<T> : IPagedList<T>
{
    private readonly IReadOnlyList<T> _items;

    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "PageNumber cannot be below 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "PageSize cannot be less than 1.");
        }

        var totalItemsCount = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToArray();

        _items = items;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalItemsCount = totalItemsCount;
        PageCount = (int)Math.Ceiling(TotalItemsCount / (double)pageSize);
    }

    public PagedList(IReadOnlyList<T> items, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "PageNumber cannot be below 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "PageSize cannot be less than 1.");
        }

        var totalItemsCount = items.Count;

        _items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItemsCount = totalItemsCount;
        PageCount = (int)Math.Ceiling(TotalItemsCount / (double)pageSize);
    }

    public int PageCount { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalItemsCount { get; }

    public int Count => _items.Count;

    public T this[int index] => _items[index];

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
