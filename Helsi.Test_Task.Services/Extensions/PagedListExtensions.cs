
using AdSaver.Infrastructure.Utils.PagedList;
using Helsi.Test_Task.Infrastructure.Utils.PagedList;
using MongoDB.Driver;

namespace Helsi.Test_Task.Services.Extensions;

public static class PagedListExtensions
{
    public static async Task<PagedList<TDto>> GetPagedAsync<T, TDto>(
        this IMongoCollection<T> collection,       
        PipelineDefinition<T, TDto> pipeline,
        int pageNumber,
        int pageSize)
        where T : class
        where TDto : class
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 1 : pageSize;

        pipeline = pipeline
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize);

        var cursor = await collection.AggregateAsync(pipeline);
        var data = await cursor.ToListAsync();

        return data.ToPagedList(pageNumber, pageSize);
    }
}
