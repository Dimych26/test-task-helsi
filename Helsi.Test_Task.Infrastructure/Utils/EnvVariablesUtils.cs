
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Helsi.Test_Task.Infrastructure.Utils;

public static class EnvVariablesUtils
{
    public static string GetRequiredVariable(string key)
    {
        return GetVariable(key)
            ?? throw new InvalidOperationException($"Environment variable with name {key} not found");
    }

    public static string? GetVariable(string key)
    {
        return Environment.GetEnvironmentVariable(key);
    }

    public static T GetRequiredVariable<T>(string key)
    {
        var variable = GetRequiredVariable(key);
        var obj = JsonConvert.DeserializeObject<T>(variable);
        return obj ?? throw new InvalidOperationException($"Environment variable with name {key} isn`t of type {typeof(T).FullName}");
    }

    public static void AddOptionsFromEnvironmentVariable<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        services.AddOptionsFromEnvironmentVariable<TOptions>(typeof(TOptions).Name);
    }

    public static void AddOptionsFromEnvironmentVariable<TOptions>(this IServiceCollection services, string envKey)
        where TOptions : class
    {
        services.AddOptions<TOptions>()
            .Configure((TOptions opt, IMapper mapper) =>
            {
                var envOpt = GetRequiredVariable<TOptions>(envKey);
                mapper.Map(envOpt, opt);
            });
    }
}
