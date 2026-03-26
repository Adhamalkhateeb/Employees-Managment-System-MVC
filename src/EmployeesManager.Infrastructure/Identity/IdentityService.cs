using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Infrastructure.Identity;

public sealed class IdentityService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    RoleManager<AppRole> roleManager
) : IIdentityService
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly RoleManager<AppRole> _roleManager = roleManager;

    public async Task<Result<AppUserDto>> RegisterAsync(
        string userName,
        string email,
        string phoneNumber,
        string password,
        CancellationToken cancellationToken = default
    )
    {
        if (await _userManager.FindByEmailAsync(email) is not null)
            return Error.Conflict(
                "Identity.EmailTaken",
                "An account with this email already exists."
            );

        if (await _userManager.FindByNameAsync(userName) is not null)
            return Error.Conflict("Identity.UserNameTaken", "This username is already taken.");

        var user = new AppUser
        {
            UserName = userName,
            Email = email,
            PhoneNumber = phoneNumber,
            CreatedAt = DateTime.UtcNow,
        };

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
            return MapIdentityErrors(createResult.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, Role.User.ToString());
        if (!roleResult.Succeeded)
            return MapIdentityErrors(roleResult.Errors);

        var roles = await _userManager.GetRolesAsync(user);

        return new AppUserDto(
            UserId: user.Id,
            UserName: user.UserName!,
            Email: user.Email!,
            PhoneNumber: user.PhoneNumber!,
            Roles: roles
        );
    }

    public async Task<Result<AppUserDto>> LoginAsync(
        string email,
        string password,
        bool rememberMe = false,
        CancellationToken cancellationToken = default
    )
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Error.Unauthorized("Identity.InvalidCredentials", "Invalid email or password.");

        var signInResult = await _signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: rememberMe,
            lockoutOnFailure: true
        );

        if (signInResult.IsLockedOut)
            return Error.Forbidden("Identity.LockedOut", "Account is locked. Try again later.");

        if (!signInResult.Succeeded)
            return Error.Unauthorized("Identity.InvalidCredentials", "Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);

        return new AppUserDto(
            UserId: user.Id,
            UserName: user.UserName!,
            Email: user.Email!,
            PhoneNumber: user.PhoneNumber!,
            Roles: roles
        );
    }

    public async Task LogoutAsync() => await _signInManager.SignOutAsync();

    public async Task<Result<Updated>> AssignRoleAsync(Guid userId, Role role)
    {
        var roleName = role.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
            return Error.Validation("Identity.RoleNotFound", $"Role '{roleName}' does not exist.");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Error.NotFound("Identity.UserNotFound", "User not found.");

        if (await _userManager.IsInRoleAsync(user, roleName))
            return Error.Conflict(
                "Identity.AlreadyInRole",
                $"User is already in role '{roleName}'."
            );

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
            return MapIdentityErrors(result.Errors);

        return Result.Updated;
    }

    public async Task<Result<Updated>> RemoveRoleAsync(Guid userId, Role role)
    {
        var roleName = role.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
            return Error.Validation("Identity.RoleNotFound", $"Role '{roleName}' does not exist.");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Error.NotFound("Identity.UserNotFound", "User not found.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Count == 1 && currentRoles[0] == roleName)
            return Error.Conflict("Identity.LastRole", "Cannot remove the user's only role.");

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        if (!result.Succeeded)
            return MapIdentityErrors(result.Errors);

        return Result.Updated;
    }

    public async Task<bool> IsEmailAvailableAsync(string email) =>
        await _userManager.FindByEmailAsync(email) is null;

    public async Task<bool> IsUserNameAvailableAsync(string userName) =>
        await _userManager.FindByNameAsync(userName) is null;

    public async Task<Result<string?>> GetUserNameByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user?.UserName ?? string.Empty;
    }

    private static List<Error> MapIdentityErrors(IEnumerable<IdentityError> errors) =>
        [.. errors.Select(e => Error.Conflict(e.Code, e.Description))];
}
