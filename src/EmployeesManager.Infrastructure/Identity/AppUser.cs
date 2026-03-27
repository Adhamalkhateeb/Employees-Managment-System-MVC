using EmployeesManager.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace EmployeesManager.Infrastructure.Identity;

public sealed class AppUser : IdentityUser<Guid>
{
    public DateTime CreatedAtUtc { get; set; }
}
