using AutoMapper;
using Helsi.Test_Task.Infrastructure.Models;
using Helsi.Test_Task.Infrastructure.Utils.PagedList;

namespace Helsi.Test_Task.Infrastructure.Extensions;

public static class PagedListExtensions
{
    public static PagedListModel<TOut> ToPagedListModel<TIn, TOut>(this IPagedList<TIn> pagedList, IMapper mapper)
    {
        return new PagedListModel<TOut>
        {
            Items = mapper.Map<IReadOnlyList<TIn>, IReadOnlyList<TOut>>(pagedList),
            PageCount = pagedList.PageCount,
            PageNumber = pagedList.PageNumber,
            PageSize = pagedList.PageSize,
            TotalItemsCount = pagedList.TotalItemsCount
        };
    }
}
