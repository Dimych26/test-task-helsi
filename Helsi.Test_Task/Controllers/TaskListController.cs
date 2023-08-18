using AutoMapper;
using Helsi.Test_Task.Contracts.Services;
using Helsi.Test_Task.Dtos.TaskList;
using Helsi.Test_Task.WebApi.Models.TaskList;
using Microsoft.AspNetCore.Mvc;
using Helsi.Test_Task.Infrastructure.Extensions;
using Helsi.Test_Task.Infrastructure.Utils;
using Helsi.Test_Task.Infrastructure.Attributes;
using Helsi.Test_Task.Dtos.Options;
using Helsi.Test_Task.Infrastructure.Models;

namespace Helsi.Test_Task.WebApi.Controllers;

[ApiController]
[Route("api/task-lists")]
public class TaskListController : ControllerBase
{
    public ITaskListService _taskListService;
    public IMapper _mapper;

    public TaskListController(
        ITaskListService taskListService,
        IMapper mapper)
    {
        _taskListService = taskListService;
        _mapper = mapper;
    }

    [UserIdFilter]
    [HttpGet("page")]
    public async Task<ActionResult<PagedListModel<TaskListShortModel>>> GetByIdAsync([FromQuery] PaginationOptions options)
    {
        var result = await _taskListService.GetPageAsync(options);

        return result
            .ToResult(x => x.ToPagedListModel<TaskListShortDto, TaskListShortModel>(_mapper))
            .ToActionResult(this);
    }

    [UserIdFilter]
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskListModel>> GetByIdAsync(string id)
    {
        var result = await _taskListService.GetByIdAsync(id);

        return result
            .ToResult(_mapper.Map<TaskListModel>)
            .ToActionResult(this);
    }

    [UserIdFilter]
    [HttpPost("create")]
    public async Task<ActionResult<TaskListModel>> CreateAsync(TaskListCreateModel model)
    {
        var dto = _mapper.Map<TaskListCreateDto>(model);
        var result = await _taskListService.CreateAsync(dto);

        return result
            .ToResult(_mapper.Map<TaskListModel>)
            .ToActionResult(this);
    }

    [UserIdFilter]
    [HttpPut("update")]
    public async Task<ActionResult<TaskListModel>> UpdateAsync(TaskListUpdateModel model)
    {
        var dto = _mapper.Map<TaskListUpdateDto>(model);
        var result = await _taskListService.UpdateAsync(dto);

        return result
            .ToResult(_mapper.Map<TaskListModel>)
            .ToActionResult(this);
    }

    [UserIdFilter]
    [HttpPut("link-user")]
    public async Task<ActionResult<TaskListModel>> LinkUserAsync(LinkUserModel model)
    {
        var dto = _mapper.Map<LinkUserDto>(model);
        var result = await _taskListService.LinkUserAsync(dto);

        return result
            .ToResult(_mapper.Map<TaskListModel>)
            .ToActionResult(this);
    }

    [UserIdFilter]
    [HttpPut("unlink-user")]
    public async Task<ActionResult<TaskListModel>> UnlinkUserAsync(LinkUserModel model)
    {
        var dto = _mapper.Map<LinkUserDto>(model);
        var result = await _taskListService.UnlinkUserAsync(dto);

        return result
            .ToResult(_mapper.Map<TaskListModel>)
            .ToActionResult(this);
    }

}
