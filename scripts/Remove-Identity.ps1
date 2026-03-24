# ============================================================
#  Remove-Identity.ps1  —  Clean Architecture Identity Remover
#  Undoes everything New-Identity.ps1 created
# ============================================================
param(
    [string]$Namespace,
    [string]$DomainProjectPath,
    [string]$AppProjectPath,
    [string]$InfraProjectPath,
    [string]$WebProjectPath,
    [string]$ContractsProjectPath,
    [switch]$Force,
    [switch]$Help
)

function Write-Removed ($msg) { Write-Host "  - $msg" -ForegroundColor Red }
function Write-Skipped ($msg) { Write-Host "  ~ $msg" -ForegroundColor Yellow }
function Write-Fixed   ($msg) { Write-Host "  * $msg" -ForegroundColor Cyan }
function Write-Section ($msg) {
    Write-Host ""
    Write-Host "  ── $msg " -NoNewline -ForegroundColor Cyan
    Write-Host ("─" * [Math]::Max(0, 46 - $msg.Length)) -ForegroundColor DarkGray
}

if ($Help) {
    Write-Host @"

  Clean Architecture Identity Remover
  ─────────────────────────────────────────────────────

  -Namespace              Root namespace
  -DomainProjectPath      Domain project path
  -AppProjectPath         Application project path
  -InfraProjectPath       Infrastructure project path
  -WebProjectPath         Web project path
  -ContractsProjectPath   Contracts project path
  -Force                  Skip confirmation prompt

  NOTE: Run dotnet ef migrations remove before this script
        if you already ran AddIdentity migration.

"@ -ForegroundColor Cyan
    exit 0
}

Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║   Clean Architecture Identity Remover            ║" -ForegroundColor Red
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray

if (-not $Namespace)            { $Namespace            = Read-Host "  Root namespace" }
if (-not $DomainProjectPath)    { $DomainProjectPath    = Read-Host "  Domain project path" }
if (-not $AppProjectPath)       { $AppProjectPath       = Read-Host "  Application project path" }
if (-not $InfraProjectPath)     { $InfraProjectPath     = Read-Host "  Infrastructure project path" }
if (-not $WebProjectPath)       { $WebProjectPath       = Read-Host "  Web project path" }
if (-not $ContractsProjectPath) { $ContractsProjectPath = Read-Host "  Contracts project path" }

if (-not $Force) {
    Write-Host ""
    Write-Host "  ⚠  This will permanently delete all Identity files." -ForegroundColor DarkYellow
    Write-Host "     Have you removed the migration? (dotnet ef migrations remove)" -ForegroundColor DarkYellow
    Write-Host ""
    $confirm = Read-Host "  Type 'identity' to confirm"
    if ($confirm -ne 'identity') {
        Write-Host "  Cancelled." -ForegroundColor Red
        exit 0
    }
}

function Remove-Dir($path, $label) {
    if (Test-Path $path) {
        Remove-Item -Recurse -Force $path
        Write-Removed "deleted folder  $label"
    } else {
        Write-Skipped "not found       $label"
    }
}

function Remove-File($path) {
    if (Test-Path $path) {
        Remove-Item -Force $path
        Write-Removed "deleted file    $(Split-Path $path -Leaf)"
    } else {
        Write-Skipped "not found       $(Split-Path $path -Leaf)"
    }
}

# ── Domain ─────────────────────────────────────────────────────────────────
Write-Section "Domain"
Remove-File "$DomainProjectPath/Identity/Role.cs"
$identityDir = "$DomainProjectPath/Identity"
if ((Test-Path $identityDir) -and -not (Get-ChildItem $identityDir)) {
    Remove-Item $identityDir
    Write-Removed "deleted empty folder  Domain/Identity/"
}

# ── Application ────────────────────────────────────────────────────────────
Write-Section "Application"
Remove-Dir  "$AppProjectPath/Features/Identity"             "Application/Features/Identity/"
Remove-File "$AppProjectPath/Common/Interfaces/IIdentityService.cs"
Remove-File "$AppProjectPath/Common/Interfaces/ICurrentUser.cs"

# ── Infrastructure ─────────────────────────────────────────────────────────
Write-Section "Infrastructure"
Remove-File "$InfraProjectPath/Identity/AppUser.cs"
Remove-File "$InfraProjectPath/Identity/AppRole.cs"
Remove-File "$InfraProjectPath/Identity/RoleSeeder.cs"
Remove-File "$InfraProjectPath/Identity/IdentityService.cs"
Remove-File "$InfraProjectPath/Identity/Identity_DI_Snippet.txt"
Remove-File "$InfraProjectPath/Data/Interceptors/AuditableEntityInterceptor.cs"
$infraIdentityDir = "$InfraProjectPath/Identity"
if ((Test-Path $infraIdentityDir) -and -not (Get-ChildItem $infraIdentityDir)) {
    Remove-Item $infraIdentityDir
    Write-Removed "deleted empty folder  Infrastructure/Identity/"
}

# ── Contracts ──────────────────────────────────────────────────────────────
Write-Section "Contracts"
Remove-Dir "$ContractsProjectPath/Requests/Identity" "Contracts/Requests/Identity/"

# ── Web ────────────────────────────────────────────────────────────────────
Write-Section "Web"
Remove-File "$WebProjectPath/Controllers/AccountController.cs"
Remove-File "$WebProjectPath/Services/CurrentUser.cs"
Remove-Dir  "$WebProjectPath/Views/Account" "Web/Views/Account/"

$servicesDir = "$WebProjectPath/Services"
if ((Test-Path $servicesDir) -and -not (Get-ChildItem $servicesDir)) {
    Remove-Item $servicesDir
    Write-Removed "deleted empty folder  Web/Services/"
}

# ── Summary ────────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║  Identity files removed                          ║" -ForegroundColor Red
Write-Host "  ╠══════════════════════════════════════════════════╣" -ForegroundColor DarkGray
Write-Host "  ║  Manual cleanup needed:                          ║" -ForegroundColor DarkYellow
Write-Host "  ║  [ ] Revert AppDbContext to DbContext base       ║" -ForegroundColor White
Write-Host "  ║  [ ] Remove Identity DI from DependencyInjection ║" -ForegroundColor White
Write-Host "  ║  [ ] Remove RoleSeeder.SeedAsync from Program.cs ║" -ForegroundColor White
Write-Host "  ║  [ ] Remove AddHttpContextAccessor from DI       ║" -ForegroundColor White
Write-Host "  ║  [ ] dotnet ef migrations remove (if needed)     ║" -ForegroundColor White
Write-Host "  ║  [ ] dotnet build to verify clean                ║" -ForegroundColor White
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray
Write-Host ""