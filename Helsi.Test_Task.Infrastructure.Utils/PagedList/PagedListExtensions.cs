using Helsi.Test_Task.Infrastructure.Utils.PagedList;

namespace AdSaver.Infrastructure.Utils.PagedList;

public static class PagedListExtensions
{
    public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        return new PagedList<T>(source, pageNumber, pageSize);
    }

    public static PagedList<T> ToPagedList<T>(this List<T> source, int pageNumber, int pageSize)
    {
        return new PagedList<T>(source, pageNumber, pageSize);
    }
}
