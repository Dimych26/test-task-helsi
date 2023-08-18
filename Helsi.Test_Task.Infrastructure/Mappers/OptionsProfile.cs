
using AutoMapper;
using Helsi.Test_Task.MongoDb.Settings;

namespace Helsi.Test_Task.Infrastructure.Mappers;

public class OptionsProfile : Profile
{
    public OptionsProfile()
    {
        CreateMap<MongoDbSettings, MongoDbSettings>();
    }
}
