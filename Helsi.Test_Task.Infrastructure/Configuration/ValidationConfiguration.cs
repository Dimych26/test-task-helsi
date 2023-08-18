using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Helsi.Test_Task.Infrastructure.Configuration;

public static class ValidationConfiguration
{
    public static void AddFluentValidation(this IServiceCollection services, params Assembly[] assemblies)
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        services.AddFluentValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
            config.AutomaticValidationEnabled = true;
            config.RegisterValidatorsFromAssemblies(assemblies);
        });

        services.AddSingleton<IValidatorInterceptor, ValidatorInterceptor>();
        services.AddSingleton<Contracts.IValidator, Validator>();
    }

    private class Validator : Contracts.IValidator
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public Validator(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<ValidationResult> ValidateAsync<T>(T obj)
        {
            var validator = _contextAccessor.HttpContext!.RequestServices.GetService<IValidator<T>>()!;
            return await validator.ValidateAsync(obj);
        }
    }

    private class ValidatorInterceptor : IValidatorInterceptor
    {
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }

        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            var projection = result.Errors
                .Select(x => new ValidationFailure(x.PropertyName, x.ErrorCode, x.AttemptedValue)
                {
                    ErrorCode = x.ErrorCode,
                    CustomState = x.CustomState,
                    FormattedMessagePlaceholderValues = x.FormattedMessagePlaceholderValues,
                    Severity = x.Severity,
                });

            return new ValidationResult(projection);
        }
    }
}
