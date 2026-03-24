using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesManager.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
        services.AddAuthentication();
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
