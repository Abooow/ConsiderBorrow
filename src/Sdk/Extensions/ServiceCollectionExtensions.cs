using ConsiderBorrow.Sdk.Facades;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsiderBorrowSdk(this IServiceCollection services)
    {
        services.AddScoped<ICategoryFacade, CategoryFacade>();
        services.AddScoped<ILibraryItemFacade, LibraryItemFacade>();
        services.AddScoped<IEmployeeFacade, EmployeeFacade>();

        return services;
    }
}
