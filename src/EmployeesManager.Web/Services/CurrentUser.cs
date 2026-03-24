using System.Security.Claims;
using EmployeesManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EmployeesManager.Web.Services;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public Guid? Id =>
        Principal?.FindFirstValue(ClaimTypes.NameIdentifier) is string id ? Guid.Parse(id) : null;

    public string? UserName => Principal?.FindFirstValue(ClaimTypes.Name);
    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email);
    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => Principal?.IsInRole(role) ?? false;

    public IEnumerable<string> Roles =>
        Principal?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
        ?? Enumerable.Empty<string>();
}
