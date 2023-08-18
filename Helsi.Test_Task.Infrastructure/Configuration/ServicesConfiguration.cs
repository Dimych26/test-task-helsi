
using AdSaver.Persistens.EF;
using Helsi.Test_Task.Contracts;
using Helsi.Test_Task.Contracts.Services;
using Helsi.Test_Task.Core;
using Helsi.Test_Task.Infrastructure.Extensions;
using Helsi.Test_Task.Infrastructure.Utils;
using Helsi.Test_Task.MongoDb.Settings;
using Helsi.Test_Task.Services;
using Helsi.Test_Task.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Helsi.Test_Task.Infrastructure.Configuration;
public static class ServicesConfiguration
{
    public static void AddHelsiCommonServices(this IServiceCollection services/*, Assembly[] autoMapperAssemblies, Assembly[] validationAssemblies*/)
    {
        services.AddOptionsFromEnvironmentVariable<MongoDbSettings>();

        var assemblies = new Assembly[] 
        {
            Assembly.GetEntryAssembly()!,
            typeof(ServicesConfiguration).Assembly,
            typeof(IServiceMarker).Assembly
        };

        services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();

        services.AddFluentValidation(assemblies);

        services.AddSingleton<IRequestContext, RequestContext>();

        services.AddAutoMapper(assemblies);

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskListService, TaskListService>();
    }

    private class RequestContext : IRequestContext
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string UserId => _contextAccessor.HttpContext!.Request.GetUserId();

        public bool TryGetUserId(out string userId)
        {
            return _contextAccessor.HttpContext!.Request.TryGetUserId(out userId);
        }
    }
}
