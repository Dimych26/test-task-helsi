using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Helsi.Test_Task.Infrastructure.Configuration;

public static class EnvironmentVariablesConfiguration
{
    public static void AddEnvironmentVariablesFromConfig()
    {
        var assemblyPath = Assembly.GetEntryAssembly()!.Location;
        var baseDir = Path.GetDirectoryName(assemblyPath)!;
        var configFilePath = Path.Combine(baseDir, "appsettings.json");
        var configJson = File.ReadAllText(configFilePath);
        var configObj = (JObject)JsonConvert.DeserializeObject(configJson)!;
        foreach (var item in configObj)
        {
            if (Environment.GetEnvironmentVariable(item.Key) == null)
            {
                Console.WriteLine(string.Format("Added environment variable {0} from config with value {1}", item.Key, item.Value?.ToString()));
                Environment.SetEnvironmentVariable(item.Key, item.Value?.ToString());
            }
        }
    }
}
