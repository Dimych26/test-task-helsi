
using AutoMapper;
using Helsi.Test_Task.Dtos.TaskList;
using Helsi.Test_Task.WebApi.Models.TaskList;

namespace Helsi.Test_Task.WebApi.Mappers;

public class TaskListProfile : Profile
{
    public TaskListProfile()
    {
        CreateMap<TaskListDto, TaskListModel>();

        CreateMap<TaskListCreateModel, TaskListCreateDto>();

        CreateMap<TaskListUpdateModel, TaskListUpdateDto>();

        CreateMap<LinkUserModel, LinkUserDto>();

        CreateMap<TaskListShortDto, TaskListShortModel>();
    }
}
