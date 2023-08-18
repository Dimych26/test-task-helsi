
using Ardalis.Result;
using Helsi.Test_Task.Dtos.Options;
using Helsi.Test_Task.Dtos.TaskList;
using Helsi.Test_Task.Infrastructure.Utils.PagedList;

namespace Helsi.Test_Task.Contracts.Services;

public interface ITaskListService
{
    Task<Result<PagedList<TaskListShortDto>>> GetPageAsync(PaginationOptions options);

    Task<Result<TaskListDto>> GetByIdAsync(string id);

    Task<Result<TaskListDto>> CreateAsync(TaskListCreateDto dto);

    Task<Result<TaskListDto>> UpdateAsync(TaskListUpdateDto dto);

    Task<Result> DeleteAsync(string id);

    Task<Result<TaskListDto>> LinkUserAsync(LinkUserDto dto);

    Task<Result<TaskListDto>> UnlinkUserAsync(LinkUserDto dto);

    Task<bool> ExistsAsync(string id);
}
