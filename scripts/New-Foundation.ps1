# ============================================================
#  New-Foundation.ps1  —  Clean Architecture Base Infrastructure
#  Generates: Result<T>, Error, BaseEntity, IAppDbContext,
#             ValidationBehavior, AppDbContext, MvcController,
#             HomeController, ErrorViewModel, Program.cs
#  One command. Zero manual file creation.
# ============================================================
param(
    [string]$Namespace,
    [string]$DbName,
    [string]$SolutionRoot,
    [switch]$Help
)

# ── Console helpers ────────────────────────────────────────────────────────
function Write-Created ($msg) { Write-Host "  + $msg" -ForegroundColor Green }
function Write-Skipped ($msg) { Write-Host "  ~ $msg" -ForegroundColor Yellow }
function Write-Section ($msg) {
    Write-Host ""
    Write-Host "  ── $msg " -NoNewline -ForegroundColor Cyan
    Write-Host ("─" * [Math]::Max(0, 46 - $msg.Length)) -ForegroundColor DarkGray
}

# ── Help ──────────────────────────────────────────────────────────────────
if ($Help) {
    Write-Host @"

  Clean Architecture Foundation Generator
  ────────────────────────────────────────────────────

  -Namespace     Root namespace / solution name  (e.g. EmployeesManager)
  -DbName        Database name                   (e.g. EmployeesManagerDb)
  -SolutionRoot  Path to solution root           (default: current directory)
  -Help          Show this message

  EXAMPLE
    .\New-Foundation.ps1 -Namespace EmployeesManager -DbName EmployeesManagerDb

  WHAT GETS GENERATED
    Domain/Common/Results/   ErrorKind.cs · Error.cs · Result.cs
    Domain/Common/           BaseEntity.cs
    Domain/Common/Results/Abstractions/  IResult.cs
    Application/Common/       ValidationBehavior.cs UnhandledExceptionBehavior.cs
    Application/Interfaces/   IAppDbContext.cs
    Application/             DependencyInjection.cs
    Infrastructure/Persistence/  AppDbContext.cs
    Infrastructure/          DependencyInjection.cs
    Web/Controllers/         MvcController.cs · HomeController.cs
    Web/Models/              ErrorViewModel.cs
    Web/Views/Shared/        Error.cshtml
    Web/                     Program.cs · appsettings.json (updated)

"@ -ForegroundColor Cyan
    exit 0
}

# ── Interactive prompts ────────────────────────────────────────────────────
Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║   Clean Architecture Foundation Generator        ║" -ForegroundColor Cyan
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray
Write-Host ""

if (-not $Namespace)    { $Namespace    = Read-Host "  Namespace / solution name (e.g. EmployeesManager)" }
if (-not $DbName)       { $DbName       = Read-Host "  Database name (e.g. EmployeesManagerDb)" }
if (-not $SolutionRoot) { $SolutionRoot = Get-Location }

# ── Derive project paths ───────────────────────────────────────────────────
$domain  = "$SolutionRoot/src/$Namespace.Domain"
$app     = "$SolutionRoot/src/$Namespace.Application"
$infra   = "$SolutionRoot/src/$Namespace.Infrastructure"
$web     = "$SolutionRoot/src/$Namespace.Web"

foreach ($path in @($domain, $app, $infra, $web)) {
    if (-not (Test-Path $path)) {
        Write-Host "  Project not found: $path" -ForegroundColor Red
        Write-Host "  Run the solution scaffold commands first." -ForegroundColor DarkYellow
        exit 1
    }
}

# ── File writer ────────────────────────────────────────────────────────────
function Write-File($path, $content) {
    if (Test-Path $path) {
        Write-Skipped "exists   $(Split-Path $path -Leaf)"
    } else {
        New-Item -ItemType File -Path $path -Force | Out-Null
        Set-Content -Path $path -Value $content -Encoding UTF8
        Write-Created "$(Split-Path $path -Leaf)"
    }
}

# ══════════════════════════════════════════════════════════════════════════
#  DOMAIN
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Domain — Result pattern"

Write-File "$domain/Common/Results/ErrorKind.cs" @"
namespace $Namespace.Domain.Common.Results;

public enum ErrorKind
{
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    Failure,
    Unexpected,
}
"@

Write-File "$domain/Common/Results/Error.cs" @"
namespace $Namespace.Domain.Common.Results;

public readonly record struct Error
{
    private Error(string code, string description, ErrorKind type)
    {
        Code        = code;
        Description = description;
        Type        = type;
    }

    public string    Code        { get; }
    public string    Description { get; }
    public ErrorKind Type        { get; }

    public static Error Validation  (string code = nameof(Validation),   string description = "Validation error.")  => new(code, description, ErrorKind.Validation);
    public static Error Conflict    (string code = nameof(Conflict),     string description = "Conflict error.")     => new(code, description, ErrorKind.Conflict);
    public static Error NotFound    (string code = nameof(NotFound),     string description = "Not found.")          => new(code, description, ErrorKind.NotFound);
    public static Error Unauthorized(string code = nameof(Unauthorized), string description = "Unauthorized.")       => new(code, description, ErrorKind.Unauthorized);
    public static Error Forbidden   (string code = nameof(Forbidden),    string description = "Forbidden.")          => new(code, description, ErrorKind.Forbidden);
    public static Error Failure     (string code = nameof(Failure),      string description = "General failure.")    => new(code, description, ErrorKind.Failure);
    public static Error Unexpected  (string code = nameof(Unexpected),   string description = "Unexpected error.")   => new(code, description, ErrorKind.Unexpected);

    public static Error Create(ErrorKind type, string code, string description)
        => new(code, description, type);
}
"@

Write-File "$domain/Common/Results/Abstractions/IResult.cs" @"
namespace $Namespace.Domain.Common.Results.Abstractions;

public interface IResult
{
    bool IsSuccess { get; }
    List<Error> Errors { get; }
}

public interface IResult<TValue> : IResult
{
    TValue Value { get; }
}


"@

Write-File "$domain/Common/Results/Result.cs" @"

using $Namespace.Domain.Common.Results.Abstractions;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace $Namespace.Domain.Common.Results;


public class Result
{
    public static Success Success => default;
    public static Created Created => default;
    public static Updated Updated => default;
    public static Deleted Deleted => default;
}

public sealed class Result<TValue> : IResult<TValue>
{
    private readonly TValue?       _value  = default;
    private readonly List<Error>?  _errors = null;

    public bool IsSuccess { get; }
    public bool IsError   => !IsSuccess;

    public List<Error> Errors =>
        IsError ? _errors! : [];

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access Value of a failed result.");

    public Error TopError =>
        _errors?.Count > 0 ? _errors[0] : default;

    private Result(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _value    = value;
        IsSuccess = true;
    }

    private Result(Error error)
    {
        _errors   = [error];
        IsSuccess = false;
    }

    private Result(List<Error> errors)
    {
        if (errors is null || errors.Count == 0)
            throw new ArgumentException(
                "Provide at least one error.", nameof(errors));

        _errors   = errors;
        IsSuccess = false;
    }

    
    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("For Serializer only", true)]
    public Result(TValue? value, List<Error>? errors, bool isSuccess)
    {
        if (isSuccess)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _errors = [];
            IsSuccess = true;
        }
        else
        {
            if (errors is null || errors.Count == 0)
            {
                throw new ArgumentException(
                    "Cannot create an Error or <TValue> from an empty collection of errors. Provide at least one error.",
                    nameof(errors)
                );
            }

            _errors = errors;
            _value = default;
            IsSuccess = false;
        }
    }

    public TNext Match<TNext>(
        Func<TValue,       TNext> onValue,
        Func<List<Error>,  TNext> onError)
        => IsSuccess ? onValue(Value) : onError(Errors);

    public static implicit operator Result<TValue>(TValue       value)  => new(value);
    public static implicit operator Result<TValue>(Error        error)  => new(error);
    public static implicit operator Result<TValue>(List<Error>  errors) => new(errors);
}

public readonly record struct Success;
public readonly record struct Created;
public readonly record struct Updated;
public readonly record struct Deleted;
"@

Write-Section "Domain — BaseEntity"

Write-File "$domain/Common/BaseEntity.cs" @"
namespace $Namespace.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; }

    protected BaseEntity() { }

    protected BaseEntity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }
}

"@


Write-Section "Domain — Auditable BaseEntity"

Write-File "$domain/Common/AuditableEntity.cs" @"
namespace $Namespace.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    protected AuditableEntity() { }

    protected AuditableEntity(Guid id)
        : base(id) { }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModifiedUtc { get; set; }
    public string? LastModifiedBy { get; set; }
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  APPLICATION
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Application — Interfaces + Behaviors"

Write-File "$app/Common/Interfaces/IAppDbContext.cs" @"
namespace $Namespace.Application.Common.Interfaces;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
"@

Write-File "$app/Common/Behaviors/ValidationBehavior.cs" @"
using $Namespace.Domain.Common.Results;
using FluentValidation;
using MediatR;

namespace $Namespace.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest  : IRequest<TResponse>
    where TResponse : class
{
    public async Task<TResponse> Handle(
        TRequest                          request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken                 cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var failures = (await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(r => r.Errors)
            .Where(e => e is not null)
            .ToList();

        if (failures.Count == 0)
            return await next(cancellationToken);

        var errors = failures
            .ConvertAll(f => Error.Validation(f.PropertyName, f.ErrorMessage));

        return (dynamic)errors;
    }
}
"@


Write-File "$app/Common/Behaviors/UnhandledExceptionBehavior.cs" @"
using MediatR;
using Microsoft.Extensions.Logging;


namespace $Namespace.Application.Common.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Request was cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred while processing request.");
            throw;
        }
    }
}
"@

Write-Section "Application — DependencyInjection"

Write-File "$app/DependencyInjection.cs" @"
using System.Reflection;
using $Namespace.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace $Namespace.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
        });

        return services;
    }
}
"@

# ══════════════════════════════════════════════════════════════════════════
#  INFRASTRUCTURE
# ══════════════════════════════════════════════════════════════════════════
Write-Section "Infrastructure — AppDbContext"

Write-File "$infra/Data/AppDbContext.cs" @"
using System.Reflection;
using $Namespace.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace $Namespace.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options), IAppDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
"@

Write-Section "Infrastructure — DependencyInjection"

Write-File "$infra/DependencyInjection.cs" @"
using $Namespace.Application.Common.Interfaces;
using $Namespace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace $Namespace.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IHostEnvironment        environment,
        IConfiguration          configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        if (!environment.IsEnvironment("Test"))
        {
            services.AddDbContext<AppDbContext>(
                (sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    options.UseSqlServer(connectionString);
                }
            );
        }

        services.AddScoped<IAppDbContext>(sp =>
            sp.GetRequiredService<AppDbContext>());

        return services;
    }
}
"@


Write-Section "Infrastructure — AuditableEntityInterceptor"

Write-File "$infra/Data/Interceptors/AuditableEntityInterceptor.cs" @"
using $Namespace.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace $Namespace.Infrastructure.Data;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                var utcNow = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAtUtc = utcNow;
                }

                entry.Entity.LastModifiedUtc = utcNow;
            }
        }
    }
}
"@


# ══════════════════════════════════════════════════════════════════════════
#  WEB
# ════════════════════════════════════════
Write-Section "Web — MvcController base"

Write-File "$web/Controllers/MvcController.cs" @"
using $Namespace.Domain.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace $Namespace.Web.Controllers;

public class MvcController : Controller
{
    protected IActionResult HandleError<TModel>(List<Error> errors, TModel model)
        => HandleErrorInternal(errors, () => View(model));

    protected IActionResult HandleError(List<Error> errors)
        => HandleErrorInternal(errors, () => View());

    private IActionResult HandleErrorInternal(
        List<Error>          errors,
        Func<IActionResult>  onValidation)
    {
        if (errors is null || errors.Count == 0)
            return RedirectToAction("Error", "Home",
                new { statusCode = StatusCodes.Status500InternalServerError });

        if (errors.All(e =>
                e.Type == ErrorKind.Validation ||
                e.Type == ErrorKind.Conflict))
        {
            foreach (var error in errors)
                ModelState.AddModelError(error.Code, error.Description);

            return onValidation();
        }

        var primary    = errors[0];
        var statusCode = MapToStatusCode(primary.Type);

        TempData["ResultErrorCode"]    = primary.Code;
        TempData["ResultErrorMessage"] = primary.Description;
        TempData["ResultErrorDetails"] = string.Join(" | ",
            errors.Select(e => $"{e.Code}: {e.Description}").Distinct());

        return RedirectToAction("Error", "Home", new { statusCode });
    }

    private static int MapToStatusCode(ErrorKind type) =>
        type switch
        {
            ErrorKind.Validation   => StatusCodes.Status400BadRequest,
            ErrorKind.Conflict     => StatusCodes.Status409Conflict,
            ErrorKind.NotFound     => StatusCodes.Status404NotFound,
            ErrorKind.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorKind.Forbidden    => StatusCodes.Status403Forbidden,
            _                      => StatusCodes.Status500InternalServerError,
        };
}
"@

Write-Section "Web — HomeController + ErrorViewModel"

Write-File "$web/Controllers/HomeController.cs" @"
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using $Namespace.Web.Models;

namespace $Namespace.Web.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => View();

    [Route("Error")]
    public IActionResult Error(int? statusCode = null)
    {
        var effective = statusCode ?? StatusCodes.Status500InternalServerError;
        Response.StatusCode = effective;

        var model = new ErrorViewModel
        {
            StatusCode = effective,
            TraceId    = HttpContext.TraceIdentifier,
            Details    = TempData["ResultErrorDetails"]?.ToString(),
            ErrorCode  = TempData["ResultErrorCode"]?.ToString(),
            Message    = TempData["ResultErrorMessage"]?.ToString()
                         ?? effective switch
                         {
                             400 => "Bad Request",
                             401 => "Unauthorized",
                             403 => "Forbidden",
                             404 => "Page Not Found",
                             409 => "Conflict",
                             _   => "An unexpected error occurred."
                         }
        };

        var ex = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (ex?.Error is not null)
            model = model with
            {
                Details = string.IsNullOrWhiteSpace(model.Details)
                    ? ex.Error.Message
                    : $"{model.Details} | {ex.Error.Message}"
            };

        return View("~/Views/Shared/Error.cshtml", model);
    }
}
"@

Write-File "$web/Models/ErrorViewModel.cs" @"
namespace $Namespace.Web.Models;

public record ErrorViewModel
{
    public int     StatusCode { get; init; }
    public string? TraceId    { get; init; }
    public string? ErrorCode  { get; init; }
    public string? Message    { get; init; }
    public string? Details    { get; init; }
}
"@

Write-Section "Web — Error.cshtml"

New-Item -ItemType Directory -Path "$web/Views/Shared" -Force | Out-Null
Write-File "$web/Views/Shared/Error.cshtml" @"
@model $Namespace.Web.Models.ErrorViewModel
@{
    ViewBag.Title = `$"Error {Model.StatusCode}";
    Layout = "_Layout";
}
<main>
    <div style="max-width:480px;margin:4rem auto;text-align:center">
        <h1>@Model.StatusCode</h1>
        <p>@Model.Message</p>
        @if (!string.IsNullOrEmpty(Model.Details))
        {
            <p><small>@Model.Details</small></p>
        }
        @if (!string.IsNullOrEmpty(Model.TraceId))
        {
            <p><small>Trace: <code>@Model.TraceId</code></small></p>
        }
        <a asp-controller="Home" asp-action="Index">Go Home</a>
    </div>
</main>
"@

Write-Section "Web — Program.cs"

# Only overwrite Program.cs if it still has the default MVC boilerplate
$programPath = "$web/Program.cs"
$existing    = if (Test-Path $programPath) { Get-Content $programPath -Raw } else { "" }

if ($existing -match "builder\.Services\.AddControllersWithViews\(\)" -and
    $existing -notmatch "AddApplication") {

    Set-Content -Path $programPath -Encoding UTF8 -Value @"
using $Namespace.Application;
using $Namespace.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Environment, builder.Configuration);

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.UseHsts();
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name:    "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();

public partial class Program { }
"@
    Write-Created "Program.cs (replaced boilerplate)"
} else {
    Write-Skipped "Program.cs (already customized — skipped)"
}

Write-Section "Web — appsettings.json"

$settingsPath = "$web/appsettings.json"
$settings     = if (Test-Path $settingsPath) {
    Get-Content $settingsPath -Raw | ConvertFrom-Json
} else {
    [PSCustomObject]@{}
}

if (-not $settings.ConnectionStrings) {
    $settings | Add-Member -NotePropertyName "ConnectionStrings" -NotePropertyValue @{
        DefaultConnection = "Server=(localdb)\mssqllocaldb;Database=$DbName;Trusted_Connection=True;MultipleActiveResultSets=true"
    } -Force
    $settings | ConvertTo-Json -Depth 5 | Set-Content $settingsPath -Encoding UTF8
    Write-Created "appsettings.json (ConnectionString added)"
} else {
    Write-Skipped "appsettings.json (ConnectionStrings already exists)"
}

# ══════════════════════════════════════════════════════════════════════════
#  SUMMARY
# ══════════════════════════════════════════════════════════════════════════
Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║  Foundation ready for '$Namespace'" -NoNewline -ForegroundColor Cyan
Write-Host "$((' ' * [Math]::Max(0, 21 - $Namespace.Length)))║" -ForegroundColor DarkGray
Write-Host "  ╠══════════════════════════════════════════════════╣" -ForegroundColor DarkGray
Write-Host "  ║  Next steps:                                     ║" -ForegroundColor DarkYellow
Write-Host "  ║  1. dotnet build                                 ║" -ForegroundColor White
Write-Host "  ║  2. dotnet ef migrations add InitialCreate       ║" -ForegroundColor White
Write-Host "  ║     --project src/$Namespace.Infrastructure      " -NoNewline -ForegroundColor White; Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ║     --startup-project src/$Namespace.Web        " -NoNewline -ForegroundColor White; Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ║  3. dotnet ef database update                    ║" -ForegroundColor White
Write-Host "  ║  4. .\scripts\New-Feature.ps1 -All -WithAll      ║" -ForegroundColor White
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray
Write-Host ""
