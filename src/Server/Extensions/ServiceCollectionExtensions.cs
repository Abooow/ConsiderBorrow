using ConsiderBorrow.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection UseResultBasedValidationResponse(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(o =>
        {
            o.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = new List<string>();
                foreach (var modelState in actionContext.ModelState.Values)
                {
                    errors.AddRange(modelState.Errors.Select(x => x.ErrorMessage));
                }

                return new BadRequestObjectResult(Result.Fail(errors).WithDescription(StatusCodeDescriptions.ValidationError));
            };
        });

        return services;
    }
}
