# ============================================================
#  New-Identity.ps1  —  Clean Architecture Identity Scaffold
#  Generates full ASP.NET Identity setup for MVC projects:
#    Domain:      AppUser, AppRole, Role enum
#    Application: IIdentityService, AppUserDto,
#                 Register/Login Commands/Queries + Validators + Handlers
#    Infra:       IdentityService, RoleSeeder, DI registration update
#    Contracts:   RegisterRequest, LoginRequest
#    Web:         AccountController, Register/Login views
# ============================================================
param(
    [string]$Namespace,
    [string]$DomainProjectPath,
    [string]$AppProjectPath,
    [string]$InfraProjectPath,
    [string]$WebProjectPath,
    [string]$ContractsProjectPath,
    [switch]$Help
)

# ── Console helpers ────────────────────────────────────────────────────────
function Write-Created ($msg) { Write-Host "  + $msg" -ForegroundColor Green }
function Write-Skipped ($msg) { Write-Host "  ~ $msg" -ForegroundColor Yellow }
function Write-Warn    ($msg) { Write-Host "  ! $msg" -ForegroundColor DarkYellow }
function Write-Section ($msg) {
    Write-Host ""
    Write-Host "  ── $msg " -NoNewline -ForegroundColor Cyan
    Write-Host ("─" * [Math]::Max(0, 46 - $msg.Length)) -ForegroundColor DarkGray
}

if ($Help) {
    Write-Host @"

  Clean Architecture Identity Scaffold
  ─────────────────────────────────────────────────────

  -Namespace              Root namespace       (e.g. EmployeesManager)
  -DomainProjectPath      Domain project       (e.g. src/EmployeesManager.Domain)
  -AppProjectPath         Application project  (e.g. src/EmployeesManager.Application)
  -InfraProjectPath       Infrastructure project
  -WebProjectPath         Web project
  -ContractsProjectPath   Contracts project

  EXAMPLE
    .\New-Identity.ps1 ``
        -Namespace EmployeesManager ``
        -DomainProjectPath    src/EmployeesManager.Domain ``
        -AppProjectPath       src/EmployeesManager.Application ``
        -InfraProjectPath     src/EmployeesManager.Infrastructure ``
        -WebProjectPath       src/EmployeesManager.Web ``
        -ContractsProjectPath src/EmployeesManager.Contracts

"@ -ForegroundColor Cyan
    exit 0
}

# ── Banner ─────────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║   Clean Architecture Identity Scaffold           ║" -ForegroundColor Cyan
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray

# ── Interactive prompts ────────────────────────────────────────────────────
if (-not $Namespace)            { $Namespace            = Read-Host "  Root namespace (e.g. EmployeesManager)" }
if (-not $DomainProjectPath)    { $DomainProjectPath    = Read-Host "  Domain project path" }
if (-not $AppProjectPath)       { $AppProjectPath       = Read-Host "  Application project path" }
if (-not $InfraProjectPath)     { $InfraProjectPath     = Read-Host "  Infrastructure project path" }
if (-not $WebProjectPath)       { $WebProjectPath       = Read-Host "  Web project path" }
if (-not $ContractsProjectPath) { $ContractsProjectPath = Read-Host "  Contracts project path" }

# ── Validate paths ─────────────────────────────────────────────────────────
function Assert-Path($path, $label) {
    if (-not (Test-Path $path)) {
        Write-Host "  Path not found [$label]: $path" -ForegroundColor Red
        exit 1
    }
}
Assert-Path $DomainProjectPath    "Domain"
Assert-Path $AppProjectPath       "Application"
Assert-Path $InfraProjectPath     "Infrastructure"
Assert-Path $WebProjectPath       "Web"
Assert-Path $ContractsProjectPath "Contracts"

# ── Namespace shortcuts ────────────────────────────────────────────────────
$domNs       = "$Namespace.Domain"
$appNs       = "$Namespace.Application"
$infraNs     = "$Namespace.Infrastructure"
$webNs       = "$Namespace.Web"
$contractsNs = "$Namespace.Contracts"

# ── File writer ────────────────────────────────────────────────────────────
function Write-File($path, $content) {
    if (Test-Path $path) {
        Write-Skipped "exists   $(Split-Path $path -Leaf)"
    } else {
        New-Item -ItemType File -Path $path -Force | Out-Null
        Set-Content $path $content -Encoding UTF8
        Write-Created "$(Split-Path $path -Leaf)"
    }
}

# ══════════════════════════════════════════════════════════════════════════
#  DOMAIN — Role enum
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Domain — Role enum"

Write-File "$DomainProjectPath/Identity/Role.cs" @"
namespace $domNs.Identity;

public enum Role
{
    User,
    Admin,
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  INFRASTRUCTURE — AppUser + AppRole
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Infrastructure — AppUser + AppRole"

Write-File "$InfraProjectPath/Identity/AppUser.cs" @"
using Microsoft.AspNetCore.Identity;

namespace $infraNs.Identity;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName  { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
"@

Write-File "$InfraProjectPath/Identity/AppRole.cs" @"
using Microsoft.AspNetCore.Identity;

namespace $infraNs.Identity;

public class AppRole : IdentityRole<Guid>;
"@

# ══════════════════════════════════════════════════════════════════════════
#  INFRASTRUCTURE — RoleSeeder
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Infrastructure — RoleSeeder"

Write-File "$InfraProjectPath/Identity/RoleSeeder.cs" @"
using $domNs.Identity;
using Microsoft.AspNetCore.Identity;

namespace $infraNs.Identity;

public static class RoleSeeder
{
    private static readonly string[] Roles = Enum.GetNames<Role>();

    public static async Task SeedAsync(RoleManager<AppRole> roleManager)
    {
        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  APPLICATION — AppUserDto
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Application — AppUserDto"

Write-File "$AppProjectPath/Features/Identity/Dtos/AppUserDto.cs" @"
namespace $appNs.Features.Identity.Dtos;

public sealed record AppUserDto(
    Guid   UserId,
    string UserName,
    string Email,
    string PhoneNumber,
    IList<string> Roles
);
"@

# ══════════════════════════════════════════════════════════════════════════
#  APPLICATION — ICurrentUser
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Application — ICurrentUser"

Write-File "$AppProjectPath/Common/Interfaces/ICurrentUser.cs" @"
namespace $appNs.Common.Interfaces;

public interface ICurrentUser
{
    Guid?   Id              { get; }
    string? UserName        { get; }
    string? Email           { get; }
    bool    IsAuthenticated { get; }
    bool    IsInRole(string role);
    IEnumerable<string> Roles { get; }
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  APPLICATION — IIdentityService
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Application — IIdentityService"

Write-File "$AppProjectPath/Common/Interfaces/IIdentityService.cs" @"
using $appNs.Features.Identity.Dtos;
using $domNs.Common.Results;
using $domNs.Identity;

namespace $appNs.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<AppUserDto>> RegisterAsync(
        string userName,
        string email,
        string phoneNumber,
        string password,
        CancellationToken cancellationToken = default
    );

    Task<Result<AppUserDto>> LoginAsync(
        string email,
        string password,
        bool rememberMe = false,
        CancellationToken cancellationToken = default
    );

    Task LogoutAsync();

    Task<Result<Updated>> AssignRoleAsync(Guid userId, Role role);
    Task<Result<Updated>> RemoveRoleAsync(Guid userId, Role role);

    Task<bool> IsEmailAvailableAsync(string email);
    Task<bool> IsUserNameAvailableAsync(string userName);
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  APPLICATION — Register Command
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Application — Register Command"

Write-File "$AppProjectPath/Features/Identity/Commands/Register/RegisterCommand.cs" @"
using $appNs.Features.Identity.Dtos;
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.Identity.Commands.Register;

public sealed record RegisterCommand(
    string UserName,
    string Email,
    string PhoneNumber,
    string Password
) : IRequest<Result<AppUserDto>>;
"@

Write-File "$AppProjectPath/Features/Identity/Commands/Register/RegisterCommandValidator.cs" @"
using FluentValidation;

namespace $appNs.Features.Identity.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Enter a valid email address");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}
"@

Write-File "$AppProjectPath/Features/Identity/Commands/Register/RegisterCommandHandler.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.Identity.Dtos;
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.Identity.Commands.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<AppUserDto>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
        => _identityService = identityService;

    public Task<Result<AppUserDto>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
        => _identityService.RegisterAsync(
            command.UserName,
            command.Email,
            command.PhoneNumber,
            command.Password,
            cancellationToken
        );
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  APPLICATION — Login Query
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Application — Login Query"

Write-File "$AppProjectPath/Features/Identity/Queries/Login/LoginQuery.cs" @"
using $appNs.Features.Identity.Dtos;
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.Identity.Queries.Login;

public sealed record LoginQuery(
    string Email,
    string Password,
    bool   RememberMe
) : IRequest<Result<AppUserDto>>;
"@

Write-File "$AppProjectPath/Features/Identity/Queries/Login/LoginQueryValidator.cs" @"
using FluentValidation;

namespace $appNs.Features.Identity.Queries.Login;

public sealed class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Enter a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
"@

Write-File "$AppProjectPath/Features/Identity/Queries/Login/LoginQueryHandler.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.Identity.Dtos;
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.Identity.Queries.Login;

public sealed class LoginQueryHandler
    : IRequestHandler<LoginQuery, Result<AppUserDto>>
{
    private readonly IIdentityService _identityService;

    public LoginQueryHandler(IIdentityService identityService)
        => _identityService = identityService;

    public Task<Result<AppUserDto>> Handle(
        LoginQuery query,
        CancellationToken cancellationToken)
        => _identityService.LoginAsync(
            query.Email,
            query.Password,
            query.RememberMe,
            cancellationToken
        );
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  INFRASTRUCTURE — IdentityService
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Infrastructure — IdentityService"

Write-File "$InfraProjectPath/Identity/IdentityService.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.Identity.Dtos;
using $domNs.Common.Results;
using $domNs.Identity;
using Microsoft.AspNetCore.Identity;

namespace $infraNs.Identity;

public sealed class IdentityService(
    UserManager<AppUser>   userManager,
    SignInManager<AppUser> signInManager,
    RoleManager<AppRole>   roleManager
) : IIdentityService
{
    private readonly UserManager<AppUser>   _userManager   = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly RoleManager<AppRole>   _roleManager   = roleManager;

    public async Task<Result<AppUserDto>> RegisterAsync(
        string userName,
        string email,
        string phoneNumber,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(email) is not null)
            return Error.Conflict("Identity.EmailTaken", "An account with this email already exists.");

        if (await _userManager.FindByNameAsync(userName) is not null)
            return Error.Conflict("Identity.UserNameTaken", "This username is already taken.");

        var user = new AppUser
        {
            UserName    = userName,
            Email       = email,
            PhoneNumber = phoneNumber,
            CreatedAt   = DateTime.UtcNow,
        };

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
            return MapIdentityErrors(createResult.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, Role.User.ToString());
        if (!roleResult.Succeeded)
            return MapIdentityErrors(roleResult.Errors);

        var roles = await _userManager.GetRolesAsync(user);

        return new AppUserDto(
            UserId:      user.Id,
            UserName:    user.UserName!,
            Email:       user.Email!,
            PhoneNumber: user.PhoneNumber!,
            Roles:       roles
        );
    }

    public async Task<Result<AppUserDto>> LoginAsync(
        string email,
        string password,
        bool rememberMe = false,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Error.Unauthorized("Identity.InvalidCredentials", "Invalid email or password.");

        var signInResult = await _signInManager.PasswordSignInAsync(
            user, password,
            isPersistent:    rememberMe,
            lockoutOnFailure: true
        );

        if (signInResult.IsLockedOut)
            return Error.Forbidden("Identity.LockedOut", "Account is locked. Try again later.");

        if (!signInResult.Succeeded)
            return Error.Unauthorized("Identity.InvalidCredentials", "Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);

        return new AppUserDto(
            UserId:      user.Id,
            UserName:    user.UserName!,
            Email:       user.Email!,
            PhoneNumber: user.PhoneNumber!,
            Roles:       roles
        );
    }

    public async Task LogoutAsync()
        => await _signInManager.SignOutAsync();

    public async Task<Result<Updated>> AssignRoleAsync(Guid userId, Role role)
    {
        var roleName = role.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
            return Error.Validation("Identity.RoleNotFound", $"Role '{roleName}' does not exist.");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Error.NotFound("Identity.UserNotFound", "User not found.");

        if (await _userManager.IsInRoleAsync(user, roleName))
            return Error.Conflict("Identity.AlreadyInRole", $"User is already in role '{roleName}'.");

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

    public async Task<bool> IsEmailAvailableAsync(string email)
        => await _userManager.FindByEmailAsync(email) is null;

    public async Task<bool> IsUserNameAvailableAsync(string userName)
        => await _userManager.FindByNameAsync(userName) is null;

    private static List<Error> MapIdentityErrors(IEnumerable<IdentityError> errors) =>
        [.. errors.Select(e => Error.Conflict(e.Code, e.Description))];
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  INFRASTRUCTURE — DI update snippet
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Infrastructure — DI Identity Snippet"

Write-File "$InfraProjectPath/Identity/Identity_DI_Snippet.txt" @"
// ── ADD to Infrastructure/DependencyInjection.cs ──────────────────────────
//
// using $infraNs.Identity;
// using Microsoft.AspNetCore.Identity;
//
// services.AddScoped<IIdentityService, IdentityService>();
//
// services
//     .AddIdentity<AppUser, AppRole>(options =>
//     {
//         options.Password.RequiredLength      = 8;
//         options.Password.RequireDigit        = true;
//         options.Password.RequireUppercase    = true;
//         options.Password.RequireNonAlphanumeric = true;
//         options.User.RequireUniqueEmail      = true;
//         options.Lockout.MaxFailedAccessAttempts = 5;
//         options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(15);
//     })
//     .AddEntityFrameworkStores<AppDbContext>()
//     .AddDefaultTokenProviders();
//
// services.ConfigureApplicationCookie(options =>
// {
//     options.LoginPath        = "/Account/Login";
//     options.LogoutPath       = "/Account/Logout";
//     options.AccessDeniedPath = "/Account/AccessDenied";
//     options.SlidingExpiration = true;
//     options.ExpireTimeSpan   = TimeSpan.FromDays(7);
// });
//
// ── ADD to Web DependencyInjection / Program.cs ──────────────────────────
// services.AddHttpContextAccessor();
// services.AddScoped<ICurrentUser, CurrentUser>();
//
// ── ADD to Program.cs after app.Build() ───────────────────────────────────
//
// using (var scope = app.Services.CreateScope())
// {
//     var roleManager = scope.ServiceProvider
//         .GetRequiredService<RoleManager<AppRole>>();
//     await RoleSeeder.SeedAsync(roleManager);
// }
"@

Write-Warn "Open Identity_DI_Snippet.txt and add the DI registrations manually"

# ══════════════════════════════════════════════════════════════════════════
#  CONTRACTS — RegisterRequest + LoginRequest
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Contracts — RegisterRequest + LoginRequest"

Write-File "$ContractsProjectPath/Requests/Identity/RegisterRequest.cs" @"
using System.ComponentModel.DataAnnotations;

namespace $contractsNs.Requests.Identity;

public sealed class RegisterRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [Display(Name = "Email address")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format")]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = default!;
}
"@

Write-File "$ContractsProjectPath/Requests/Identity/LoginRequest.cs" @"
using System.ComponentModel.DataAnnotations;

namespace $contractsNs.Requests.Identity;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [Display(Name = "Email address")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; } = false;
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  WEB — AccountController
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Web — AccountController"

Write-File "$WebProjectPath/Controllers/AccountController.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.Identity.Commands.Register;
using $appNs.Features.Identity.Queries.Login;
using $contractsNs.Requests.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace $webNs.Controllers;

[AllowAnonymous]
[Route("[controller]/[action]")]
public sealed class AccountController : MvcController
{
    private readonly IMediator        _mediator;
    private readonly IIdentityService _identityService;

    public AccountController(IMediator mediator, IIdentityService identityService)
    {
        _mediator        = mediator;
        _identityService = identityService;
    }

    // ── Register ──────────────────────────────────────────────────────────

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var command = new RegisterCommand(
            request.UserName,
            request.Email,
            request.PhoneNumber,
            request.Password
        );

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return RedirectToAction(nameof(Login));

        return HandleError(result.Errors, request);
    }

    // ── Login ─────────────────────────────────────────────────────────────

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        var query = new LoginQuery(
            request.Email,
            request.Password,
            request.RememberMe
        );

        var result = await _mediator.Send(query);

        if (result.IsSuccess)
            return RedirectToAction("Index", "Home");

        return HandleError(result.Errors, request);
    }

    // ── Logout ────────────────────────────────────────────────────────────

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _identityService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }

    // ── Access Denied ──────────────────────────────────────────────────────

    [HttpGet]
    public IActionResult AccessDenied() => View();

    // ── Remote validation ─────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> IsEmailAvailable(string email)
    {
        var available = await _identityService.IsEmailAvailableAsync(email);
        return Json(available ? true : (object)"This email is already in use.");
    }

    [HttpGet]
    public async Task<IActionResult> IsUserNameAvailable(string userName)
    {
        var available = await _identityService.IsUserNameAvailableAsync(userName);
        return Json(available ? true : (object)"This username is already taken.");
    }
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  WEB — CurrentUser service
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Web — CurrentUser service"

Write-File "$WebProjectPath/Services/CurrentUser.cs" @"
using System.Security.Claims;
using $appNs.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace $webNs.Services;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal
        => accessor.HttpContext?.User;

    public Guid? Id =>
        Principal?.FindFirstValue(ClaimTypes.NameIdentifier) is string id
            ? Guid.Parse(id)
            : null;

    public string? UserName        => Principal?.FindFirstValue(ClaimTypes.Name);
    public string? Email           => Principal?.FindFirstValue(ClaimTypes.Email);
    public bool    IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role)
        => Principal?.IsInRole(role) ?? false;

    public IEnumerable<string> Roles =>
        Principal?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
        ?? Enumerable.Empty<string>();
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  INFRASTRUCTURE — Updated AuditableEntityInterceptor
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Infrastructure — AuditableEntityInterceptor (with ICurrentUser)"

Write-File "$InfraProjectPath/Data/Interceptors/AuditableEntityInterceptor.cs" @"
using $domNs.Common;
using $appNs.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace $infraNs.Data.Interceptors;

public sealed class AuditableEntityInterceptor(ICurrentUser currentUser)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData      eventData,
        InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData      eventData,
        InterceptionResult<int> result,
        CancellationToken       cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null) return;

        var utcNow  = DateTimeOffset.UtcNow;
        var actorBy = currentUser.UserName ?? "system";

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAtUtc  = utcNow;
                    entry.Entity.CreatedBy     = actorBy;
                }

                entry.Entity.LastModifiedUtc = utcNow;
                entry.Entity.LastModifiedBy  = actorBy;
            }
        }
    }
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  WEB — Razor Views
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Web — Register View"

Write-File "$WebProjectPath/Views/Account/Register.cshtml" @"
@using $contractsNs.Requests.Identity
@model RegisterRequest
@{
    ViewBag.Title = "Register";
}

<div class="row justify-content-center">
    <div class="col-md-6">
        <h2 class="mb-4">Create your account</h2>

        @if (!ViewData.ModelState.IsValid)
        {
            <div class="alert alert-danger">
                <asp-validation-summary asp-validation-type="All" />
            </div>
        }

        <form asp-action="Register" method="post" novalidate>
            @Html.AntiForgeryToken()

            <div class="mb-3">
                <label asp-for="UserName" class="form-label">Username <span class="text-danger">*</span></label>
                <input asp-for="UserName" class="form-control" placeholder="john_doe" autocomplete="username" />
                <span asp-validation-for="UserName" class="text-danger small"></span>
            </div>

            <div class="mb-3">
                <label asp-for="Email" class="form-label">Email address <span class="text-danger">*</span></label>
                <input asp-for="Email" class="form-control" placeholder="john@example.com" autocomplete="email" />
                <span asp-validation-for="Email" class="text-danger small"></span>
            </div>

            <div class="mb-3">
                <label asp-for="PhoneNumber" class="form-label">Phone number <span class="text-danger">*</span></label>
                <input asp-for="PhoneNumber" class="form-control" placeholder="+1 555 000 0000" autocomplete="tel" />
                <span asp-validation-for="PhoneNumber" class="text-danger small"></span>
            </div>

            <div class="mb-3">
                <label asp-for="Password" class="form-label">Password <span class="text-danger">*</span></label>
                <input asp-for="Password" type="password" class="form-control" placeholder="Min. 8 characters" autocomplete="new-password" />
                <span asp-validation-for="Password" class="text-danger small"></span>
            </div>

            <div class="mb-3">
                <label asp-for="ConfirmPassword" class="form-label">Confirm password <span class="text-danger">*</span></label>
                <input asp-for="ConfirmPassword" type="password" class="form-control" placeholder="Repeat your password" autocomplete="new-password" />
                <span asp-validation-for="ConfirmPassword" class="text-danger small"></span>
            </div>

            <div class="d-grid mb-3">
                <button type="submit" class="btn btn-primary">Create account</button>
            </div>

            <p class="text-center">
                Already have an account?
                <a asp-action="Login">Sign in</a>
            </p>
        </form>
    </div>
</div>

@section scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
    <script>
        // Remote validation fires only on blur, not every keystroke
        `$(function () {
            var form = `$('form');
            form.validate().settings.onkeyup = false;
            `$('#UserName, #Email').on('blur', function () {
                form.validate().element(this);
            });
        });
    </script>
}
"@

Write-Section "Web — Login View"

Write-File "$WebProjectPath/Views/Account/Login.cshtml" @"
@using $contractsNs.Requests.Identity
@model LoginRequest
@{
    ViewBag.Title = "Sign in";
}

<div class="row justify-content-center">
    <div class="col-md-5">
        <h2 class="mb-4">Welcome back</h2>

        @if (!ViewData.ModelState.IsValid)
        {
            <div class="alert alert-danger">
                <asp-validation-summary asp-validation-type="All" />
            </div>
        }

        <form asp-action="Login" method="post" novalidate>
            @Html.AntiForgeryToken()

            <div class="mb-3">
                <label asp-for="Email" class="form-label">Email address <span class="text-danger">*</span></label>
                <input asp-for="Email" class="form-control" placeholder="john@example.com" autocomplete="email" />
                <span asp-validation-for="Email" class="text-danger small"></span>
            </div>

            <div class="mb-3">
                <label asp-for="Password" class="form-label">Password <span class="text-danger">*</span></label>
                <input asp-for="Password" type="password" class="form-control" placeholder="Your password" autocomplete="current-password" />
                <span asp-validation-for="Password" class="text-danger small"></span>
            </div>

            <div class="mb-3 form-check">
                <input asp-for="RememberMe" class="form-check-input" type="checkbox" />
                <label asp-for="RememberMe" class="form-check-label">Remember me</label>
            </div>

            <div class="d-grid mb-3">
                <button type="submit" class="btn btn-primary">Sign in</button>
            </div>

            <p class="text-center">
                Don't have an account?
                <a asp-action="Register">Create one</a>
            </p>
        </form>
    </div>
</div>

@section scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
"@

Write-Section "Web — AccessDenied View"

Write-File "$WebProjectPath/Views/Account/AccessDenied.cshtml" @"
@{
    ViewBag.Title = "Access Denied";
}

<div class="row justify-content-center mt-5">
    <div class="col-md-6 text-center">
        <h1 class="display-4 text-danger">403</h1>
        <h2>Access Denied</h2>
        <p class="text-muted">You don't have permission to view this page.</p>
        <a asp-controller="Home" asp-action="Index" class="btn btn-primary mt-3">
            Go Home
        </a>
    </div>
</div>
"@

# ══════════════════════════════════════════════════════════════════════════
#  SUMMARY
# ══════════════════════════════════════════════════════════════════════════
Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║  Identity scaffolded successfully                ║" -ForegroundColor Cyan
Write-Host "  ╠══════════════════════════════════════════════════╣" -ForegroundColor DarkGray
Write-Host "  ║  Manual steps required:                          ║" -ForegroundColor DarkYellow
Write-Host "  ║  [ ] Install NuGet packages:                     ║" -ForegroundColor White
Write-Host "  ║      Microsoft.AspNetCore.Identity.EF            ║" -ForegroundColor White
Write-Host "  ║  [ ] Open Identity_DI_Snippet.txt                ║" -ForegroundColor White
Write-Host "  ║      Add Identity + Cookie DI registrations      ║" -ForegroundColor White
Write-Host "  ║  [ ] Add AppUser/AppRole to AppDbContext          ║" -ForegroundColor White
Write-Host "  ║      (AppDbContext : IdentityDbContext<AppUser,   ║" -ForegroundColor White
Write-Host "  ║       AppRole, Guid>)                            ║" -ForegroundColor White
Write-Host "  ║  [ ] Add RoleSeeder.SeedAsync() to Program.cs    ║" -ForegroundColor White
Write-Host "  ║  [ ] Add [AllowAnonymous] to AccountController   ║" -ForegroundColor White
Write-Host "  ║  [ ] Register ICurrentUser in DI:               ║" -ForegroundColor White
Write-Host "  ║      services.AddHttpContextAccessor()           ║" -ForegroundColor White
Write-Host "  ║      services.AddScoped<ICurrentUser,CurrentUser>║" -ForegroundColor White
Write-Host "  ║  [ ] dotnet ef migrations add AddIdentity        ║" -ForegroundColor White
Write-Host "  ║  [ ] dotnet ef database update                   ║" -ForegroundColor White
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray
Write-Host ""