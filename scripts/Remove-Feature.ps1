# ============================================================
#  Remove-Feature.ps1  v2  —  Clean Architecture Feature Remover
#  Undoes everything New-Feature.ps1 created:
#    - Deletes Domain/Entities/FEATURE/ folder
#    - Deletes EF Configuration file
#    - Removes DbSet injection from IAppDbContext + AppDbContext
#    - Deletes Application/Features/FEATURE/ folder
#    - Deletes Web/Controllers/FEATURESController.cs
#    - Deletes Web/Views/FEATURES/ folder  (plural, matches New-Feature)
#    - Deletes Tests/Features/FEATURE/ folder
#    - Deletes Contracts/Requests+Responses/FEATURE/ folders
#    - Deletes Web/Mappers/FEATUREMappers.cs
#
#  Fixes (v2):
#    - $featurePlural defined (was missing, caused silent wrong paths)
#    - Views path corrected to plural (Views/${Feature}s) to match New-Feature
#    - Controller filename corrected to ${Feature}sController.cs
#    - Phantom snippet file removal removed (file was never created)
#    - Remove-DbSet guarded against missing AppProjectPath
#    - DryRun mode: preview all deletions without touching anything
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

    [switch]$Force,
    [switch]$DryRun,
    [switch]$Help
)

# ── Console helpers ────────────────────────────────────────────────────────
function Write-Removed ($msg) { Write-Host "  - $msg" -ForegroundColor Red }
function Write-Skipped ($msg) { Write-Host "  ~ $msg" -ForegroundColor Yellow }
function Write-Fixed   ($msg) { Write-Host "  * $msg" -ForegroundColor Cyan }
function Write-Section ($msg) {
    Write-Host ""
    Write-Host "  ── $msg " -NoNewline -ForegroundColor Cyan
    Write-Host ("─" * [Math]::Max(0, 46 - $msg.Length)) -ForegroundColor DarkGray
}

# ── Help ──────────────────────────────────────────────────────────────────
if ($Help) {
    Write-Host @"

  Clean Architecture Feature Remover
  ────────────────────────────────────────────────────────────

  -Namespace          Root namespace              (e.g. EmployeesManager)
  -Feature            Feature in PascalCase       (e.g. Employee)
  -AppProjectPath     Application project path
  -DomainProjectPath  Domain project path
  -InfraProjectPath   Infrastructure project path
  -WebProjectPath     Web project path
  -TestProjectPath    Test project path
  -Force              Skip confirmation prompt
  -DryRun             Preview what would be deleted without touching anything
  -Help               Show this message

  EXAMPLE
    .\Remove-Feature.ps1 ``
        -Namespace EmployeesManager -Feature Employee ``
        -AppProjectPath    src/EmployeesManager.Application ``
        -DomainProjectPath src/EmployeesManager.Domain ``
        -InfraProjectPath  src/EmployeesManager.Infrastructure ``
        -WebProjectPath    src/EmployeesManager.Web ``
        -TestProjectPath   tests/EmployeesManager.Tests

  NOTE
    Always run: dotnet ef migrations remove
    before running this script if you already ran migrations.

"@ -ForegroundColor Cyan
    exit 0
}

# ── Banner ─────────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║   Clean Architecture Feature Remover             ║" -ForegroundColor Red
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray

if ($DryRun) {
    Write-Host "  *** DRY-RUN MODE — no files will be deleted ***" -ForegroundColor DarkCyan
    Write-Host ""
}

# ── Interactive prompts ────────────────────────────────────────────────────
if (-not $Namespace)       { $Namespace       = Read-Host "  Root namespace (e.g. EmployeesManager)" }
if (-not $Feature)         { $Feature         = Read-Host "  Feature name to remove (e.g. Employee)" }
if (-not $AppProjectPath)  { $AppProjectPath  = Read-Host "  Application project path" }
if (-not $DomainProjectPath) {
    $DomainProjectPath = Read-Host "  Domain project path (Enter to skip)"
}
if (-not $InfraProjectPath) {
    $InfraProjectPath = Read-Host "  Infrastructure project path (Enter to skip)"
}
if (-not $WebProjectPath) {
    $WebProjectPath = Read-Host "  Web project path (Enter to skip)"
}
if (-not $TestProjectPath) {
    $TestProjectPath = Read-Host "  Test project path (Enter to skip)"
}
if (-not $ContractsProjectPath) {
    $ContractsProjectPath = Read-Host "  Contracts project path (Enter to skip)"
}

# ── Confirm ────────────────────────────────────────────────────────────────
if (-not $Force -and -not $DryRun) {
    Write-Host ""
    Write-Host "  ⚠  This will permanently delete all files for '$Feature'." -ForegroundColor DarkYellow
    Write-Host "     Have you removed the EF migration? (dotnet ef migrations remove)" -ForegroundColor DarkYellow
    Write-Host ""
    $confirm = Read-Host "  Type the feature name to confirm"
    if ($confirm -ne $Feature) {
        Write-Host "  Cancelled — name did not match." -ForegroundColor Red
        exit 0
    }
}

# ── Namespace shortcuts ────────────────────────────────────────────────────
$domNs         = "$Namespace.Domain"
$featurePlural = "${Feature}s"

# ── Safe delete helpers ────────────────────────────────────────────────────
function Remove-Dir($path, $label) {
    if ($DryRun) {
        if (Test-Path $path) {
            Write-Host "  [DRY-RUN] would delete folder  $label" -ForegroundColor DarkCyan
        } else {
            Write-Skipped "not found       $label"
        }
        return
    }
    if (Test-Path $path) {
        Remove-Item -Recurse -Force $path
        Write-Removed "deleted folder  $label"
    } else {
        Write-Skipped "not found       $label"
    }
}

function Remove-File($path, $label) {
    if ($DryRun) {
        if (Test-Path $path) {
            Write-Host "  [DRY-RUN] would delete file    $label" -ForegroundColor DarkCyan
        } else {
            Write-Skipped "not found       $label"
        }
        return
    }
    if (Test-Path $path) {
        Remove-Item -Force $path
        Write-Removed "deleted file    $label"
    } else {
        Write-Skipped "not found       $label"
    }
}

# ── DbSet remover ──────────────────────────────────────────────────────────
function Remove-DbSet($feature, $appProjectPath, $infraProjectPath, $domNs) {

    if (-not $appProjectPath -or -not (Test-Path $appProjectPath)) {
        Write-Skipped "AppProjectPath not provided or not found — skipping DbSet removal"
        return
    }

    # ── IAppDbContext ──────────────────────────────────────────────────────
    $iface = Get-ChildItem -Path $appProjectPath -Recurse -Filter "IAppDbContext.cs" |
             Select-Object -First 1

    if ($iface) {
        $content = Get-Content $iface.FullName -Raw

        if ($content -match "DbSet<$feature>") {
            if ($DryRun) {
                Write-Host "  [DRY-RUN] would remove DbSet<$feature> from IAppDbContext.cs" -ForegroundColor DarkCyan
            } else {
                $updated = $content -replace "    DbSet<$feature> ${feature}s \{ get; \}\r?\n", ""
                $updated = $updated -replace "using $domNs\.Entities\.${feature}s;\r?\n", ""
                Set-Content $iface.FullName $updated -Encoding UTF8
                Write-Fixed  "IAppDbContext.cs  (DbSet<$feature> removed)"
            }
        } else {
            Write-Skipped "IAppDbContext.cs  (DbSet<$feature> not found)"
        }
    } else {
        Write-Skipped "IAppDbContext.cs not found"
    }

    # ── AppDbContext ───────────────────────────────────────────────────────
    $ctx = Get-ChildItem -Path $infraProjectPath -Recurse -Filter "AppDbContext.cs" |
           Select-Object -First 1

    if ($ctx) {
        $content = Get-Content $ctx.FullName -Raw

        if ($content -match "DbSet<$feature>") {
            if ($DryRun) {
                Write-Host "  [DRY-RUN] would remove DbSet<$feature> from AppDbContext.cs" -ForegroundColor DarkCyan
            } else {
                $updated = $content -replace "    public DbSet<$feature> ${feature}s => Set<$feature>\(\);\r?\n(\r?\n)?", ""
                $updated = $updated -replace "using $domNs\.Entities\.${feature}s;\r?\n", ""
                Set-Content $ctx.FullName $updated -Encoding UTF8
                Write-Fixed  "AppDbContext.cs   (DbSet<$feature> removed)"
            }
        } else {
            Write-Skipped "AppDbContext.cs   (DbSet<$feature> not found)"
        }
    } else {
        Write-Skipped "AppDbContext.cs not found"
    }
}

# ══════════════════════════════════════════════════════════════════════════
#  DOMAIN
# ══════════════════════════════════════════════════════════════════════════
if ($DomainProjectPath -and (Test-Path $DomainProjectPath)) {
    Write-Section "Domain"
    Remove-Dir "$DomainProjectPath/Entities/${Feature}s" "Domain/Entities/${Feature}s/"
}

# ══════════════════════════════════════════════════════════════════════════
#  INFRASTRUCTURE
# ══════════════════════════════════════════════════════════════════════════
if ($InfraProjectPath -and (Test-Path $InfraProjectPath)) {
    Write-Section "Infrastructure"
    Remove-File "$InfraProjectPath/Data/Configurations/${Feature}Configuration.cs"  "${Feature}Configuration.cs"

    Write-Section "DbSet Removal"
    if ($AppProjectPath) {
        Remove-DbSet $Feature $AppProjectPath $InfraProjectPath $domNs
    } else {
        Write-Skipped "DbSet removal skipped — AppProjectPath not provided"
    }
}

# ══════════════════════════════════════════════════════════════════════════
#  APPLICATION
# ══════════════════════════════════════════════════════════════════════════
if ($AppProjectPath -and (Test-Path $AppProjectPath)) {
    Write-Section "Application"
    Remove-Dir "$AppProjectPath/Features/$featurePlural" "Application/Features/$featurePlural/"
}

# ══════════════════════════════════════════════════════════════════════════
#  WEB
# ══════════════════════════════════════════════════════════════════════════
if ($WebProjectPath -and (Test-Path $WebProjectPath)) {
    Write-Section "Web"
    Remove-File "$WebProjectPath/Controllers/${Feature}sController.cs" "${Feature}sController.cs"
    Remove-Dir  "$WebProjectPath/Views/${Feature}s" "Views/${Feature}s/"
}

# ══════════════════════════════════════════════════════════════════════════
#  TESTS
# ══════════════════════════════════════════════════════════════════════════
if ($TestProjectPath -and (Test-Path $TestProjectPath)) {
    Write-Section "Tests"
    Remove-Dir "$TestProjectPath/Features/$featurePlural" "Tests/Features/$featurePlural/"
}

# ══════════════════════════════════════════════════════════════════════════
#  CONTRACTS
# ══════════════════════════════════════════════════════════════════════════
if ($ContractsProjectPath -and (Test-Path $ContractsProjectPath)) {
    Write-Section "Contracts"
    Remove-Dir "$ContractsProjectPath/Requests/$featurePlural"  "Contracts/Requests/$featurePlural/"
    Remove-Dir "$ContractsProjectPath/Responses/$featurePlural" "Contracts/Responses/$featurePlural/"
}

if ($WebProjectPath -and (Test-Path $WebProjectPath)) {
    Write-Section "Web Mappers"
    Remove-File "$WebProjectPath/Mappers/${Feature}Mappers.cs" "${Feature}Mappers.cs"
}

# ══════════════════════════════════════════════════════════════════════════
#  SUMMARY
# ══════════════════════════════════════════════════════════════════════════
Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║  '$Feature' removed successfully                 " -NoNewline -ForegroundColor Red
Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ╠══════════════════════════════════════════════════╣" -ForegroundColor DarkGray
Write-Host "  ║  Don't forget:                                   ║" -ForegroundColor DarkYellow
Write-Host "  ║  [ ] dotnet ef migrations remove                 ║" -ForegroundColor White
Write-Host "  ║      --project src/$Namespace.Infrastructure     " -NoNewline -ForegroundColor White; Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ║      --startup-project src/$Namespace.Web       " -NoNewline -ForegroundColor White; Write-Host "║" -ForegroundColor DarkGray
Write-Host "  ║  [ ] dotnet build  (verify clean)                ║" -ForegroundColor White
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray
Write-Host ""