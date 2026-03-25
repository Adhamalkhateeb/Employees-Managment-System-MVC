# ============================================================
#  New-Feature.ps1  v10  —  Full-Stack Clean Architecture Scaffold
#  All folders and namespaces use plural for features and entities
#  to avoid class/namespace name conflicts.
#    Domain:      Entities/${Feature}s/
#    Application: Features/${Feature}s/
#    Tests:       Features/${Feature}s/
#
#  Fixes (v10):
#    - && → -and for HasCreate/HasUpdate common block (PS 5.1 compat)
#    - Removed unnecessary cast in GetAll handler
#    - Added ⚠ warning comments to null-check guards in Update/Delete/GetById
#    - DryRun mode: preview all files that would be created without writing
# ============================================================
param(
    [string]$Namespace,
    [string]$Feature,
    [string]$AppProjectPath,
    [string]$DomainProjectPath,
    [string]$InfraProjectPath,
    [string]$WebProjectPath,
    [string]$TestProjectPath,
    [string]$ContractsProjectPath,

    [switch]$HasCreate,
    [switch]$HasUpdate,
    [switch]$HasDelete,
    [switch]$HasGetById,
    [switch]$HasGetAll,
    [switch]$All,

    [switch]$WithEntity,
    [switch]$WithMappings,
    [switch]$WithTests,
    [switch]$WithMvcController,
    [switch]$WithViews,
    [switch]$WithAll,

    [switch]$DryRun,
    [switch]$Help
)

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

  Clean Architecture Feature Scaffold  v10
  ─────────────────────────────────────────────────────

  -Namespace          Root namespace        (e.g. EmployeesManager)
  -Feature            PascalCase name       (e.g. Employee)
  -AppProjectPath     Application path
  -DomainProjectPath  Domain path
  -InfraProjectPath   Infrastructure path
  -WebProjectPath     Web path
  -ContractsProjectPath  Contracts path
  -TestProjectPath    Tests path

  SLICES:   -HasCreate -HasUpdate -HasDelete -HasGetById -HasGetAll -All
  LAYERS:   -WithEntity -WithMappings -WithTests -WithMvcController -WithViews -WithAll
  OTHER:    -DryRun  (preview what would be created without writing any files)

  EXAMPLE
    .\New-Feature.ps1 -Namespace EmployeesManager -Feature Employee ``
        -AppProjectPath src/EmployeesManager.Application ``
        -DomainProjectPath src/EmployeesManager.Domain ``
        -InfraProjectPath src/EmployeesManager.Infrastructure ``
        -WebProjectPath src/EmployeesManager.Web ``
        -TestProjectPath tests/EmployeesManager.Tests ``
        -All -WithAll

"@ -ForegroundColor Cyan
    exit 0
}

if ($All)     { $HasCreate = $HasUpdate = $HasDelete = $HasGetById = $HasGetAll = $true }
if ($WithAll) { $WithEntity = $WithMappings = $WithTests = $WithMvcController = $WithViews = $true }

Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║   Clean Architecture Feature Scaffold  v10       ║" -ForegroundColor Cyan
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray

if ($DryRun) {
    Write-Host "  *** DRY-RUN MODE — no files will be written ***" -ForegroundColor DarkCyan
    Write-Host ""
}

if (-not $Namespace) { $Namespace = Read-Host "  Root namespace (e.g. EmployeesManager)" }
if (-not $Feature)   { $Feature   = Read-Host "  Feature name in PascalCase (e.g. Employee)" }

if (-not ($HasCreate -or $HasUpdate -or $HasDelete -or $HasGetById -or $HasGetAll)) {
    Write-Host "  No slices selected. Use -All or individual flags." -ForegroundColor Red
    exit 1
}

if (-not $AppProjectPath) { $AppProjectPath = Read-Host "  Application project path" }
if ($WithEntity) {
    if (-not $DomainProjectPath) { $DomainProjectPath = Read-Host "  Domain project path" }
    if (-not $InfraProjectPath)  { $InfraProjectPath  = Read-Host "  Infrastructure project path" }
}
if (($WithMvcController -or $WithViews) -and -not $WebProjectPath) {
    $WebProjectPath = Read-Host "  Web (MVC) project path"
}
if ($WithTests -and -not $TestProjectPath) {
    $TestProjectPath = Read-Host "  Test project path"
}
if (($HasCreate -or $HasUpdate) -and -not $ContractsProjectPath) {
    $ContractsProjectPath = Read-Host "  Contracts project path (e.g. src/EmployeesManager.Contracts)"
}

function Assert-Path($path, $label) {
    if ($path -and -not (Test-Path $path)) {
        Write-Host "  Path not found [$label]: $path" -ForegroundColor Red
        exit 1
    }
}
Assert-Path $AppProjectPath    "Application"
Assert-Path $DomainProjectPath "Domain"
Assert-Path $InfraProjectPath  "Infrastructure"
Assert-Path $WebProjectPath    "Web"
Assert-Path $TestProjectPath        "Tests"
Assert-Path $ContractsProjectPath   "Contracts"

# ── Namespace shortcuts ────────────────────────────────────────────────────
$appNs   = "$Namespace.Application"
$domNs   = "$Namespace.Domain"
$webNs   = "$Namespace.Web"
$infraNs = "$Namespace.Infrastructure"
$testNs       = "$Namespace.Tests"
$contractsNs  = "$Namespace.Contracts"

# ── Plural shortcuts ───────────────────────────────────────────────────────
# Entity folder:      Domain/Entities/Employees/         namespace: Domain.Entities.Employees
# Application folder: Application/Features/Employees/    namespace: Application.Features.Employees
# Test folder:        Tests/Features/Employees/           namespace: Tests.Features.Employees
$featurePlural = "${Feature}s"

function Write-File($path, $content) {
    if ($DryRun) {
        Write-Host "  [DRY-RUN] would create  $(Split-Path $path -Leaf)  →  $path" -ForegroundColor DarkCyan
        return
    }
    if (Test-Path $path) {
        Write-Skipped "exists   $(Split-Path $path -Leaf)"
    } else {
        New-Item -ItemType File -Path $path -Force | Out-Null
        Set-Content $path $content -Encoding UTF8
        Write-Created "$(Split-Path $path -Leaf)"
    }
}

function Inject-DbSet($feature, $featurePlural, $appProjectPath, $infraProjectPath, $domNs) {
    $iface = Get-ChildItem -Path $appProjectPath -Recurse -Filter "IAppDbContext.cs" | Select-Object -First 1
    if ($iface) {
        $content = Get-Content $iface.FullName -Raw
        if ($content -notmatch "DbSet<$feature>") {
            if ($DryRun) {
                Write-Host "  [DRY-RUN] would inject DbSet<$feature> into IAppDbContext.cs" -ForegroundColor DarkCyan
            } else {
                $updated = $content -replace '(\})\s*$', "    DbSet<$feature> ${feature}s { get; }`n`$1"
                if ($updated -notmatch "using Microsoft.EntityFrameworkCore") {
                    $updated = "using Microsoft.EntityFrameworkCore;`n" + $updated
                }
                if ($updated -notmatch "using $domNs.Entities.$featurePlural") {
                    $updated = "using $domNs.Entities.$featurePlural;`n" + $updated
                }
                Set-Content $iface.FullName $updated -Encoding UTF8
                Write-Created "IAppDbContext.cs  (DbSet<$feature> injected)"
            }
        } else {
            Write-Skipped "IAppDbContext.cs  (DbSet<$feature> already exists)"
        }
    } else {
        Write-Warn "IAppDbContext.cs not found"
    }

    $ctx = Get-ChildItem -Path $infraProjectPath -Recurse -Filter "AppDbContext.cs" | Select-Object -First 1
    if ($ctx) {
        $content = Get-Content $ctx.FullName -Raw
        if ($content -notmatch "DbSet<$feature>") {
            if ($DryRun) {
                Write-Host "  [DRY-RUN] would inject DbSet<$feature> into AppDbContext.cs" -ForegroundColor DarkCyan
            } else {
                $updated = $content -replace '(protected override void OnModelCreating)', "    public DbSet<$feature> ${feature}s => Set<$feature>();`n`n`$1"
                if ($updated -notmatch "using $domNs.Entities.$featurePlural") {
                    $updated = "using $domNs.Entities.$featurePlural;`n" + $updated
                }
                Set-Content $ctx.FullName $updated -Encoding UTF8
                Write-Created "AppDbContext.cs   (DbSet<$feature> injected)"
            }
        } else {
            Write-Skipped "AppDbContext.cs   (DbSet<$feature> already exists)"
        }
    } else {
        Write-Warn "AppDbContext.cs not found"
    }
}

# ══════════════════════════════════════════════════════════════════════════
#  DOMAIN
# ══════════════════════════════════════════════════════════════════════════
if ($WithEntity) {
    Write-Section "Domain Entity"
    $entityDir = "$DomainProjectPath/Entities/$featurePlural"

    Write-File "$entityDir/$Feature.cs" @"
using $domNs.Common;
using $domNs.Common.Results;

namespace $domNs.Entities.$featurePlural;

public sealed class $Feature : AuditableEntity
{
    // TODO: add properties
    // public string Name { get; private set; } = default!;

    private $Feature() { }

    private $Feature(Guid id) : base(id) { }

    public static Result<$Feature> Create(
        // TODO: add parameters
    )
    {
        return new $Feature(Guid.NewGuid())
        {
            // TODO: assign properties
        };
    }

    public Result<Updated> Update(
        // TODO: add parameters
    )
    {
        // TODO: assign updated properties
        return Result.Updated;
    }
}
"@

    Write-File "$entityDir/${Feature}Errors.cs" @"
using $domNs.Common.Results;

namespace $domNs.Entities.$featurePlural;

public static class ${Feature}Errors
{
    // public static Error NotFound(Guid id)
    //     => Error.NotFound("$Feature.NotFound", `$"$Feature '{id}' was not found.");

    // public static Error AlreadyExists(string name)
    //     => Error.Conflict("$Feature.AlreadyExists", `$"A $Feature named '{name}' already exists.");

    // public static readonly Error NameRequired =
    //     Error.Validation("$Feature.NameRequired", "Name is required.");
}
"@

    Write-Section "EF Configuration"

    Write-File "$InfraProjectPath/Data/Configurations/${Feature}Configuration.cs" @"
using $domNs.Entities.$featurePlural;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace $infraNs.Data.Configurations;

public sealed class ${Feature}Configuration : IEntityTypeConfiguration<$Feature>
{
    public void Configure(EntityTypeBuilder<$Feature> builder)
    {
        builder.ToTable("$featurePlural");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();


        // TODO: configure properties
        // builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
    }
}
"@

    Write-Section "DbSet Auto-Injection"
    Inject-DbSet $Feature $featurePlural $AppProjectPath $InfraProjectPath $domNs
}

# ── Base paths using plural ────────────────────────────────────────────────
$appBase  = "$AppProjectPath/Features/$featurePlural"
$testBase = "$TestProjectPath/Features/$featurePlural"

# ══════════════════════════════════════════════════════════════════════════
#  DTO
# ══════════════════════════════════════════════════════════════════════════
Write-Section "DTO"
Write-File "$appBase/Dtos/${Feature}Dto.cs" @"
namespace $appNs.Features.$featurePlural.Dtos;

public sealed record ${Feature}Dto(
    Guid Id
    // TODO: add properties
);
"@

# ══════════════════════════════════════════════════════════════════════════
#  MAPPINGS
# ══════════════════════════════════════════════════════════════════════════
if ($WithMappings) {
    Write-Section "Mappings"
    Write-File "$appBase/Mappings/${Feature}Mappings.cs" @"
using $appNs.Features.$featurePlural.Dtos;
using $domNs.Entities.$featurePlural;

namespace $appNs.Features.$featurePlural.Mappings;

public static class ${Feature}Mappings
{
    public static ${Feature}Dto ToDto(this $Feature entity)
        => new(
            Id: entity.Id
            // TODO: map properties
        );

    public static List<${Feature}Dto> ToDtos(this IEnumerable<$Feature> entities)
        => [.. entities.Select(x => x.ToDto())];
}
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  Common
# ══════════════════════════════════════════════════════════════════════════

if ($HasCreate -and $HasUpdate) {

     Write-Section "Common Contracts"
    $dir = "$appBase/Common/I${Feature}Command"
    Write-File "$dir/I${Feature}Command.cs" @"
    

namespace $appNs.Features.$featurePlural.Common;

public interface I${Feature}Command
{
    // TODO: add common command properties
}
"@

    Write-File "$dir/${Feature}CommandValidator.cs" @"

using FluentValidation;

namespace $appNs.Features.$featurePlural.Common;

public abstract class ${Feature}CommandValidatorBase<TCommand> : AbstractValidator<TCommand>
where TCommand : I${Feature}Command
{
    protected void CommonRules()
    {
        //TODO: add common validation rules for Create and Update commands
    }
}


"@
}

# ══════════════════════════════════════════════════════════════════════════
#  CREATE
# ══════════════════════════════════════════════════════════════════════════
if ($HasCreate) {
    Write-Section "Create Request + Response (Contracts)"

    if ($ContractsProjectPath) {
        Write-File "$ContractsProjectPath/Requests/$featurePlural/Create${Feature}Request.cs" @"
using System.ComponentModel.DataAnnotations;

namespace $contractsNs.Requests.$featurePlural;

public sealed class Create${Feature}Request
{
    // TODO: add properties with validation attributes
    // [Required(ErrorMessage = "Name is required")]
    // [StringLength(200, MinimumLength = 1)]
    // public string Name { get; set; } = default!;
}
"@

        Write-File "$ContractsProjectPath/Responses/$featurePlural/${Feature}Response.cs" @"
namespace $contractsNs.Responses.$featurePlural;

public sealed record ${Feature}Response(
    Guid Id
    // TODO: add properties
    // string Name
);
"@
    }

    Write-Section "Create Command"
    $dir = "$appBase/Commands/Create$Feature"
    $mappingUsing = if ($WithMappings) { "using $appNs.Features.$featurePlural.Mappings;" } else { "" }

    Write-File "$dir/Create${Feature}Command.cs" @"
using $appNs.Features.$featurePlural.Dtos;
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.$featurePlural.Commands.Create$Feature;

public sealed record Create${Feature}Command(
    // TODO: add properties
) : IRequest<Result<Created>>;
"@

    Write-File "$dir/Create${Feature}CommandValidator.cs" @"
using FluentValidation;

namespace $appNs.Features.$featurePlural.Commands.Create$Feature;

public sealed class Create${Feature}CommandValidator : AbstractValidator<Create${Feature}Command>
{
    public Create${Feature}CommandValidator()
    {
        // TODO: add rules
    }
}
"@

    Write-File "$dir/Create${Feature}CommandHandler.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.$featurePlural.Dtos;
$mappingUsing
using $domNs.Common.Results;
using $domNs.Entities.$featurePlural;
using MediatR;

namespace $appNs.Features.$featurePlural.Commands.Create$Feature;

public sealed class Create${Feature}CommandHandler
    : IRequestHandler<Create${Feature}Command, Result<Created>>
{
    private readonly IAppDbContext _context;

    public Create${Feature}CommandHandler(IAppDbContext context)
        => _context = context;

    public async Task<Result<Created>> Handle(
        Create${Feature}Command command,
        CancellationToken cancellationToken)
    {
        var createResult = $Feature.Create(
            // TODO: pass command properties
        );

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.${Feature}s.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  UPDATE
# ══════════════════════════════════════════════════════════════════════════
if ($HasUpdate) {
    Write-Section "Update Command"
    $dir = "$appBase/Commands/Update$Feature"

    if ($ContractsProjectPath) {
        Write-File "$ContractsProjectPath/Requests/$featurePlural/Update${Feature}Request.cs" @"
using System.ComponentModel.DataAnnotations;

namespace $contractsNs.Requests.$featurePlural;

public sealed class Update${Feature}Request
{
    // TODO: add properties with validation attributes
    // [Required(ErrorMessage = "Name is required")]
    // [StringLength(200, MinimumLength = 1)]
    // public string Name { get; set; } = default!;
}
"@
    }

    Write-File "$dir/Update${Feature}Command.cs" @"
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.$featurePlural.Commands.Update$Feature;

public sealed record Update${Feature}Command(
    Guid Id
    // TODO: add properties
) : IRequest<Result<Updated>>;
"@

    Write-File "$dir/Update${Feature}CommandValidator.cs" @"
using FluentValidation;

namespace $appNs.Features.$featurePlural.Commands.Update$Feature;

public sealed class Update${Feature}CommandValidator : AbstractValidator<Update${Feature}Command>
{
    public Update${Feature}CommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        // TODO: add rules
    }
}
"@

    Write-File "$dir/Update${Feature}CommandHandler.cs" @"
using $appNs.Common.Interfaces;
using $domNs.Common.Results;
using $domNs.Entities.$featurePlural;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace $appNs.Features.$featurePlural.Commands.Update$Feature;

public sealed class Update${Feature}CommandHandler
    : IRequestHandler<Update${Feature}Command, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public Update${Feature}CommandHandler(IAppDbContext context)
        => _context = context;

    public async Task<Result<Updated>> Handle(
        Update${Feature}Command command,
        CancellationToken cancellationToken)
    {
        var entity = await _context.${Feature}s
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        // ⚠ Uncomment before using in production — without this, a missing entity will throw NullReferenceException
        // if (entity is null)
        //    return ${Feature}Errors.NotFound(command.Id);

        var updateResult = entity.Update(
            // TODO: pass command properties
        );

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  DELETE
# ══════════════════════════════════════════════════════════════════════════
if ($HasDelete) {
    Write-Section "Delete Command"
    $dir = "$appBase/Commands/Delete$Feature"

    Write-File "$dir/Delete${Feature}Command.cs" @"
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.$featurePlural.Commands.Delete$Feature;

public sealed record Delete${Feature}Command(Guid Id) : IRequest<Result<Deleted>>;
"@

    Write-File "$dir/Delete${Feature}CommandValidator.cs" @"
using FluentValidation;

namespace $appNs.Features.$featurePlural.Commands.Delete$Feature;

public sealed class Delete${Feature}CommandValidator : AbstractValidator<Delete${Feature}Command>
{
    public Delete${Feature}CommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
"@

    Write-File "$dir/Delete${Feature}CommandHandler.cs" @"
using $appNs.Common.Interfaces;
using $domNs.Common.Results;
using $domNs.Entities.$featurePlural;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace $appNs.Features.$featurePlural.Commands.Delete$Feature;

public sealed class Delete${Feature}CommandHandler
    : IRequestHandler<Delete${Feature}Command, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public Delete${Feature}CommandHandler(IAppDbContext context)
        => _context = context;

    public async Task<Result<Deleted>> Handle(
        Delete${Feature}Command command,
        CancellationToken cancellationToken)
    {
        var entity = await _context.${Feature}s
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        // ⚠ Uncomment before using in production — without this, a missing entity will throw NullReferenceException
        // if (entity is null)
        //    return ${Feature}Errors.NotFound(command.Id);

        _context.${Feature}s.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  GET BY ID
# ══════════════════════════════════════════════════════════════════════════
if ($HasGetById) {
    Write-Section "GetById Query"
    $dir = "$appBase/Queries/Get${Feature}ById"
    $mappingUsing = if ($WithMappings) { "using $appNs.Features.$featurePlural.Mappings;" } else { "" }

    Write-File "$dir/Get${Feature}ByIdQuery.cs" @"
using $appNs.Features.$featurePlural.Dtos;
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.$featurePlural.Queries.Get${Feature}ById;

public sealed record Get${Feature}ByIdQuery(Guid Id) : IRequest<Result<${Feature}Dto>>;
"@

    Write-File "$dir/Get${Feature}ByIdQueryValidator.cs" @"
using FluentValidation;

namespace $appNs.Features.$featurePlural.Queries.Get${Feature}ById;

public sealed class Get${Feature}ByIdQueryValidator : AbstractValidator<Get${Feature}ByIdQuery>
{
    public Get${Feature}ByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
"@

    Write-File "$dir/Get${Feature}ByIdQueryHandler.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.$featurePlural.Dtos;
$mappingUsing
using $domNs.Common.Results;
using $domNs.Entities.$featurePlural;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace $appNs.Features.$featurePlural.Queries.Get${Feature}ById;

public sealed class Get${Feature}ByIdQueryHandler
    : IRequestHandler<Get${Feature}ByIdQuery, Result<${Feature}Dto>>
{
    private readonly IAppDbContext _context;

    public Get${Feature}ByIdQueryHandler(IAppDbContext context)
        => _context = context;

    public async Task<Result<${Feature}Dto>> Handle(
        Get${Feature}ByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _context.${Feature}s
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        // ⚠ Uncomment before using in production — without this, a missing entity will throw NullReferenceException
        // if (entity is null)
        //    return ${Feature}Errors.NotFound(query.Id);

        return entity.ToDto();
    }
}
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  GET ALL
# ══════════════════════════════════════════════════════════════════════════
if ($HasGetAll) {
    Write-Section "GetAll Query"
    $dir = "$appBase/Queries/GetAll${Feature}s"
    $mappingUsing = if ($WithMappings) { "using $appNs.Features.$featurePlural.Mappings;" } else { "" }

    Write-File "$dir/GetAll${Feature}sQuery.cs" @"
using $appNs.Features.$featurePlural.Dtos;
using $domNs.Common.Results;
using MediatR;

namespace $appNs.Features.$featurePlural.Queries.GetAll${Feature}s;

public sealed record GetAll${Feature}sQuery() : IRequest<Result<List<${Feature}Dto>>>;
"@

    Write-File "$dir/GetAll${Feature}sQueryHandler.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.$featurePlural.Dtos;
$mappingUsing
using $domNs.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace $appNs.Features.$featurePlural.Queries.GetAll${Feature}s;

public sealed class GetAll${Feature}sQueryHandler
    : IRequestHandler<GetAll${Feature}sQuery, Result<List<${Feature}Dto>>>
{
    private readonly IAppDbContext _context;

    public GetAll${Feature}sQueryHandler(IAppDbContext context)
        => _context = context;

    public async Task<Result<List<${Feature}Dto>>> Handle(
        GetAll${Feature}sQuery query,
        CancellationToken cancellationToken)
    {
        var entities = await _context.${Feature}s
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.ToDtos();
    }
}
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  TESTS
# ══════════════════════════════════════════════════════════════════════════
if ($WithTests) {
    Write-Section "Test Skeletons"

    if ($HasCreate) {
        Write-File "$testBase/Commands/Create${Feature}Tests.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.$featurePlural.Commands.Create$Feature;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace $testNs.Features.$featurePlural.Commands;

public sealed class Create${Feature}Tests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly Create${Feature}CommandHandler _handler;

    public Create${Feature}Tests()
        => _handler = new Create${Feature}CommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new Create${Feature}Command(/* TODO: valid properties */);
        var result  = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new Create${Feature}CommandValidator();
        var command   = new Create${Feature}Command(/* TODO: invalid properties */);
        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }
}
"@
    }

    if ($HasUpdate) {
        Write-File "$testBase/Commands/Update${Feature}Tests.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.$featurePlural.Commands.Update$Feature;
using $domNs.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace $testNs.Features.$featurePlural.Commands;

public sealed class Update${Feature}Tests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly Update${Feature}CommandHandler _handler;

    public Update${Feature}Tests()
        => _handler = new Update${Feature}CommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.${Feature}s to return null
        var result = await _handler.Handle(new Update${Feature}Command(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
"@
    }

    if ($HasDelete) {
        Write-File "$testBase/Commands/Delete${Feature}Tests.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.$featurePlural.Commands.Delete$Feature;
using $domNs.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace $testNs.Features.$featurePlural.Commands;

public sealed class Delete${Feature}Tests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly Delete${Feature}CommandHandler _handler;

    public Delete${Feature}Tests()
        => _handler = new Delete${Feature}CommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.${Feature}s to return null
        var result = await _handler.Handle(new Delete${Feature}Command(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.${Feature}s to return a valid entity
        var result = await _handler.Handle(new Delete${Feature}Command(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
"@
    }

    if ($HasGetById) {
        Write-File "$testBase/Queries/Get${Feature}ByIdTests.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.$featurePlural.Queries.Get${Feature}ById;
using $domNs.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace $testNs.Features.$featurePlural.Queries;

public sealed class Get${Feature}ByIdTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly Get${Feature}ByIdQueryHandler _handler;

    public Get${Feature}ByIdTests()
        => _handler = new Get${Feature}ByIdQueryHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.${Feature}s to return null
        var result = await _handler.Handle(new Get${Feature}ByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        var id = Guid.NewGuid();
        // TODO: setup _context.${Feature}s to return entity with this id

        var result = await _handler.Handle(new Get${Feature}ByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
    }
}
"@
    }

    if ($HasGetAll) {
        Write-File "$testBase/Queries/GetAll${Feature}sTests.cs" @"
using $appNs.Common.Interfaces;
using $appNs.Features.$featurePlural.Queries.GetAll${Feature}s;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace $testNs.Features.$featurePlural.Queries;

public sealed class GetAll${Feature}sTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAll${Feature}sQueryHandler _handler;

    public GetAll${Feature}sTests()
        => _handler = new GetAll${Feature}sQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.${Feature}s to return 2 entities
        var result = await _handler.Handle(new GetAll${Feature}sQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.${Feature}s to return empty list
        var result = await _handler.Handle(new GetAll${Feature}sQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
"@
    }
}

# ══════════════════════════════════════════════════════════════════════════
#  WEB MAPPERS
# ══════════════════════════════════════════════════════════════════════════
if ($WithMvcController -and $WebProjectPath -and $ContractsProjectPath) {
    Write-Section "Web Mappers"

    Write-File "$WebProjectPath/Mappers/${Feature}Mappers.cs" @"
using $appNs.Features.$featurePlural.Dtos;
using $contractsNs.Responses.$featurePlural;

namespace $webNs.Mappers;

public static class ${Feature}Mappers
{
    public static ${Feature}Response ToResponse(this ${Feature}Dto dto)
        => new(
            Id: dto.Id
            // TODO: map properties
            // Name: dto.Name
        );

    public static List<${Feature}Response> ToResponses(this IEnumerable<${Feature}Dto> dtos)
        => [.. dtos.Select(x => x.ToResponse())];
}
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  MVC CONTROLLER
# ══════════════════════════════════════════════════════════════════════════
if ($WithMvcController) {
    Write-Section "MVC Controller"

    Write-File "$WebProjectPath/Controllers/${Feature}sController.cs" @"
using $appNs.Features.$featurePlural.Commands.Create$Feature;
using $appNs.Features.$featurePlural.Commands.Delete$Feature;
using $appNs.Features.$featurePlural.Commands.Update$Feature;
using $appNs.Features.$featurePlural.Queries.GetAll${Feature}s;
using $appNs.Features.$featurePlural.Queries.Get${Feature}ById;
using $contractsNs.Requests.$featurePlural;
using $contractsNs.Responses.$featurePlural;
using $webNs.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace $webNs.Controllers;

[Authorize]
[Route("[controller]/[action]")]
public sealed class ${Feature}sController : MvcController
{
    private readonly IMediator _mediator;

    public ${Feature}sController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    [Route("/[controller]")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAll${Feature}sQuery(), cancellationToken);
        return result.Match(
            items  => View(items.ToResponses()),
            errors => HandleError(errors)
        );
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Create${Feature}Request request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(request);

        var command = new Create${Feature}Command(
            // TODO: map request properties to command
            // request.Name
        );

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _      => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new Get${Feature}ByIdQuery(id), cancellationToken);
        return result.Match(
            item =>
            {
                ViewBag.Id = item.Id;
                var request = new Update${Feature}Request
                {
                    // TODO: populate request from item
                    // Name = item.Name
                };
                return View(request);
            },
            errors => HandleError(errors)
        );
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id, Update${Feature}Request request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(request);

        var command = new Update${Feature}Command(
            Id: id
            // TODO: map request properties to command
            // request.Name
        );

        var result = await _mediator.Send(command, cancellationToken);
        return result.Match(
            _      => RedirectToAction(nameof(Index)),
            errors => HandleError(errors, request)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new Get${Feature}ByIdQuery(id), cancellationToken);
        return result.Match(
            item   => View(item.ToResponse()),
            errors => HandleError(errors)
        );
    }

    [HttpPost("{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new Delete${Feature}Command(id), cancellationToken);
        return result.Match(
            _      => RedirectToAction(nameof(Index)),
            errors => HandleError(errors)
        );
    }
}
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  RAZOR VIEWS
# ══════════════════════════════════════════════════════════════════════════
if ($WithViews) {
    Write-Section "Razor Views"
    $viewDir = "$WebProjectPath/Views/${Feature}s"

    Write-File "$viewDir/Index.cshtml" @"
@using $contractsNs.Responses.$featurePlural
@model List<${Feature}Response>
@{ ViewBag.Title = "$Feature List"; }

<div class="d-flex justify-content-between align-items-center mb-3">
    <h2>$Feature List</h2>
    <a asp-action="Create" class="btn btn-primary">New $Feature</a>
</div>

@if (!Model.Any())
{
    <div class="alert alert-info">No ${Feature}s found.</div>
}
else
{
    <table class="table table-hover">
        <thead><tr>
            <th>Id</th>
            @* TODO: add column headers *@
            <th>Actions</th>
        </tr></thead>
        <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>@item.Id</td>
                @* TODO: add columns *@
                <td>
                    <a asp-action="Edit"   asp-route-id="@item.Id" class="btn btn-sm btn-warning">Edit</a>
                    <a asp-action="Delete" asp-route-id="@item.Id" class="btn btn-sm btn-danger">Delete</a>
                </td>
            </tr>
        }
        </tbody>
    </table>
}
"@

    Write-File "$viewDir/Create.cshtml" @"
@using $contractsNs.Requests.$featurePlural
@model Create${Feature}Request
@{ ViewBag.Title = "Create $Feature"; }

<h2>Create $Feature</h2>

@if (!ViewData.ModelState.IsValid)
{
    <div class="alert alert-danger">
        <asp-validation-summary asp-validation-type="All" />
    </div>
}

<form asp-action="Create" method="post">
    @Html.AntiForgeryToken()
    @* TODO: add form fields *@
    <div class="mt-3">
        <button type="submit" class="btn btn-primary">Create</button>
        <a asp-action="Index" class="btn btn-secondary ms-2">Cancel</a>
    </div>
</form>

@section scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
"@

    Write-File "$viewDir/Edit.cshtml" @"
@using $contractsNs.Requests.$featurePlural
@model Update${Feature}Request
@{ ViewBag.Title = "Edit $Feature"; }

<h2>Edit $Feature</h2>

@if (!ViewData.ModelState.IsValid)
{
    <div class="alert alert-danger">
        <asp-validation-summary asp-validation-type="All" />
    </div>
}

<form asp-action="Edit" asp-route-id="@ViewBag.Id" method="post">
    @Html.AntiForgeryToken()
    @* TODO: add form fields *@
    <div class="mt-3">
        <button type="submit" class="btn btn-warning">Save</button>
        <a asp-action="Index" class="btn btn-secondary ms-2">Cancel</a>
    </div>
</form>

@section scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
"@

    Write-File "$viewDir/Delete.cshtml" @"
@using $contractsNs.Responses.$featurePlural
@model ${Feature}Response
@{ ViewBag.Title = "Delete $Feature"; }

<h2>Delete $Feature</h2>

<div class="alert alert-warning">Are you sure you want to delete this $Feature?</div>

<dl class="row">
    <dt class="col-sm-2">Id</dt>
    <dd class="col-sm-10">@Model.Id</dd>
    @* TODO: add display fields *@
</dl>

<form asp-action="Delete" asp-route-id="@Model.Id" method="post">
    @Html.AntiForgeryToken()
    <button type="submit" class="btn btn-danger">Confirm Delete</button>
    <a asp-action="Index" class="btn btn-secondary ms-2">Cancel</a>
</form>
"@
}

# ══════════════════════════════════════════════════════════════════════════
#  SUMMARY
# ══════════════════════════════════════════════════════════════════════════
Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║  '$Feature' scaffolded successfully              " -NoNewline -ForegroundColor Cyan
Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ╠══════════════════════════════════════════════════╣" -ForegroundColor DarkGray
Write-Host "  ║  TODO checklist:                                 ║" -ForegroundColor DarkYellow
Write-Host "  ║  [ ] Add properties to $Feature.cs              " -NoNewline -ForegroundColor White; Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ║  [ ] Define errors in ${Feature}Errors.cs        " -NoNewline -ForegroundColor White; Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ║  [ ] Add properties to commands + DTO            ║" -ForegroundColor White
Write-Host "  ║  [ ] Fill in ToDto mapping                       ║" -ForegroundColor White
Write-Host "  ║  [ ] Add FluentValidation rules                  ║" -ForegroundColor White
Write-Host "  ║  [ ] Fill in View form fields                    ║" -ForegroundColor White
Write-Host "  ║  [ ] Fill in test mock setups                    ║" -ForegroundColor White
Write-Host "  ║  [ ] dotnet ef migrations add Add${Feature}Table " -NoNewline -ForegroundColor White; Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ║  [ ] dotnet ef database update                   ║" -ForegroundColor White
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray
Write-Host ""