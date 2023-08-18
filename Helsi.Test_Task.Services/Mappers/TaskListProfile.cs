
using AutoMapper;
using Helsi.Test_Task.Dtos.TaskList;
using Helsi.Test_Task.Models;

namespace Helsi.Test_Task.Services.Mappers;

public class TaskListProfile : Profile
{
    public TaskListProfile()
    {
        CreateMap<TaskList, TaskListDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id.ToString()));

        CreateMap<TaskListCreateDto, TaskList>();

        CreateMap<TaskListUpdateDto, TaskList>();
    }
}
