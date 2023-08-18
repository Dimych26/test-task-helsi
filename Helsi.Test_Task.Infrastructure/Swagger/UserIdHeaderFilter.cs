using System.Reflection;
using Helsi.Test_Task.Infrastructure.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Helsi.Test_Task.Infrastructure.Swagger;

public class UserIdHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.TryGetMethodInfo(out var methodInfo);

        if (methodInfo == null || methodInfo.MemberType != MemberTypes.Method)
        {
            return;
        }

        var filter = methodInfo.GetCustomAttribute<UserIdFilterAttribute>();
        if (filter == null)
        {
            return;
        }

        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "userId",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}

