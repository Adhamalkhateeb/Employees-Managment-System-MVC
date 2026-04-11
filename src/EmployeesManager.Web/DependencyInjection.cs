using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Identity;
using EmployeesManager.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesManager.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "RequireAdminRole",
                policy => policy.RequireRole(Role.Admin.ToString())
            );

            options.AddPolicy(
                "RequireUserRole",
                policy => policy.RequireRole(Role.User.ToString(), Role.Admin.ToString())
            );
        });
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
