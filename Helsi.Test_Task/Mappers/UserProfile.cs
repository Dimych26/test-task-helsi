
using AutoMapper;
using Helsi.Test_Task.Dtos.User;
using Helsi.Test_Task.WebApi.Models.User;

namespace Helsi.Test_Task.WebApi.Mappers;

public class UserProfile : Profile
{
    public UserProfile() 
    {
        CreateMap<UserCreateModel, UserCreateDto>();

        CreateMap<UserDto, UserModel>();
    }
}
