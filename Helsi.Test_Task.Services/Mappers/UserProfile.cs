
using AutoMapper;
using Helsi.Test_Task.Dtos.User;
using Helsi.Test_Task.Models;

namespace Helsi.Test_Task.Services.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreateDto, User>();

        CreateMap<User, UserDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id.ToString()));
    }
}
