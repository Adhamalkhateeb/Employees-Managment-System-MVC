# ============================================================
#  Feature-Auto.ps1  v2  —  Zero-config wrapper (auto-discovery)
#
#  Scans the folder tree from the current directory (or -Root)
#  to find project paths and namespace automatically.
#  No config file needed — just run it from anywhere in your solution.
#
#  USAGE (run from solution root or any subfolder)
#    .\Feature-Auto.ps1 new    Employee -All -WithAll
#    .\Feature-Auto.ps1 new    Employee -HasCreate -HasGetAll -WithEntity
#    .\Feature-Auto.ps1 remove Employee
#    .\Feature-Auto.ps1 new    Employee -All -WithAll -DryRun
#
#  HOW DISCOVERY WORKS
#    1. Walks up from $Root (default: current dir) to find the .sln / .slnx file
#    2. Reads the solution file to discover all .csproj paths (supports both formats)
#    3. Matches each project to a role by looking for known marker files:
#         Application  → IAppDbContext.cs  or  *Application*.csproj
#         Domain       → AuditableEntity.cs  or  *Domain*.csproj
#         Infra        → AppDbContext.cs  or  *Infrastructure*.csproj
#         Web          → *Web*.csproj  or  *Api*.csproj
#         Tests        → *Tests*.csproj  or  *Test*.csproj
#         Contracts    → *Contracts*.csproj  or  *Shared*.csproj
#    4. Namespace = root namespace from the Application .csproj
#       (strips the .Application suffix)
# ============================================================
param(
    [Parameter(Position = 0, Mandatory)]
    [ValidateSet("new", "remove", "n", "r")]
    [string]$Action,

    [Parameter(Position = 1, Mandatory)]
    [string]$Feature,

    # ── Slice flags ────────────────────────────────────────────────────────
    [switch]$HasCreate,
    [switch]$HasUpdate,
    [switch]$HasDelete,
    [switch]$HasGetById,
    [switch]$HasGetAll,
    [switch]$All,

    # ── Layer flags ────────────────────────────────────────────────────────
    [switch]$WithEntity,
    [switch]$WithMappings,
    [switch]$WithTests,
    [switch]$WithMvcController,
    [switch]$WithViews,
    [switch]$WithAll,

    # ── Shared flags ───────────────────────────────────────────────────────
    [switch]$Force,
    [switch]$DryRun,

    # ── Discovery override ─────────────────────────────────────────────────
    [string]$Root        # Override scan root (default: current directory)
)

# ── Helpers ────────────────────────────────────────────────────────────────
function Write-Info  ($msg) { Write-Host "  ◆ $msg" -ForegroundColor Cyan }
function Write-Warn  ($msg) { Write-Host "  ! $msg" -ForegroundColor DarkYellow }
function Write-Fail  ($msg) { Write-Host "  ✖ $msg" -ForegroundColor Red }
function Write-Found ($label, $val) {
    Write-Host ("  {0,-22} {1}" -f $label, $val) -ForegroundColor Gray
}

# ── Find solution root ─────────────────────────────────────────────────────
function Find-SolutionRoot([string]$startDir) {
    # 1. Walk UP from current working directory
    $dir = Get-Item $startDir
    while ($dir) {
        $sln = Get-ChildItem -Path $dir.FullName -ErrorAction SilentlyContinue |
               Where-Object { $_.Extension -eq ".sln" -or $_.Extension -eq ".slnx" } |
               Select-Object -First 1
        if ($sln) { return $dir.FullName }
        $dir = $dir.Parent
    }

    # 2. Walk UP from the script file location
    #    (covers: script is in a scripts/ subfolder inside the solution)
    $dir = Get-Item $PSScriptRoot
    while ($dir) {
        $sln = Get-ChildItem -Path $dir.FullName -ErrorAction SilentlyContinue |
               Where-Object { $_.Extension -eq ".sln" -or $_.Extension -eq ".slnx" } |
               Select-Object -First 1
        if ($sln) { return $dir.FullName }
        $dir = $dir.Parent
    }

    # 3. Search one level DOWN from startDir
    #    (covers: run from a parent folder containing the solution subfolder)
    $sln = Get-ChildItem -Path $startDir -Depth 1 -ErrorAction SilentlyContinue |
           Where-Object { $_.Extension -eq ".sln" -or $_.Extension -eq ".slnx" } |
           Select-Object -First 1
    if ($sln) { return (Split-Path $sln.FullName -Parent) }

    return $null
}

# ── Parse .sln / .slnx for project paths ─────────────────────────────────
function Get-SlnProjects([string]$slnRoot) {
    $projects = @()

    # Prefer .slnx (new XML format), fall back to .sln (legacy text format)
    $slnFile = Get-ChildItem -Path $slnRoot -ErrorAction SilentlyContinue |
               Where-Object { $_.Extension -eq ".slnx" -or $_.Extension -eq ".sln" } |
               Sort-Object { if ($_.Extension -eq ".slnx") { 0 } else { 1 } } |
               Select-Object -First 1

    if (-not $slnFile) { return $projects }

    if ($slnFile.Extension -eq ".slnx") {
        # .slnx is XML (VS 2022+):
        # <Solution>
        #   <Folder Name="/src/">
        #     <Project Path="src/Foo/Foo.csproj" />
        #   </Folder>
        # </Solution>
        [xml]$xml = Get-Content $slnFile.FullName -Raw

        # GetElementsByTagName catches <Project> at any nesting depth
        $nodes = $xml.GetElementsByTagName("Project")
        foreach ($node in $nodes) {
            $rel = $node.GetAttribute("Path")
            if ($rel -and $rel -match "\.csproj$") {
                # Normalize: replace forward slashes to backslashes for Windows Join-Path
                $rel  = $rel -replace "/", [System.IO.Path]::DirectorySeparatorChar
                $full = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($slnRoot, $rel))
                if (Test-Path $full) {
                    $projects += [PSCustomObject]@{
                        Path   = (Split-Path $full -Parent)
                        Csproj = $full
                        Name   = [System.IO.Path]::GetFileNameWithoutExtension($full)
                    }
                }
            }
        }
    } else {
        # .sln is text: Project("{...}") = "Name", "path\Foo.csproj", "{guid}"
        Select-String -Path $slnFile.FullName -Pattern 'Project\(' |
            ForEach-Object {
                if ($_.Line -match '"([^"]+\.csproj)"') {
                    $rel  = $Matches[1] -replace "\\", "/"
                    $full = Join-Path $slnRoot $rel
                    if (Test-Path $full) {
                        $projects += [PSCustomObject]@{
                            Path   = (Split-Path $full -Parent)
                            Csproj = $full
                            Name   = [System.IO.Path]::GetFileNameWithoutExtension($full)
                        }
                    }
                }
            }
    }

    return $projects
}
# ── Role detection ─────────────────────────────────────────────────────────
function Get-ProjectRole([PSCustomObject]$proj) {
    $name = $proj.Name
    $path = $proj.Path

    # Marker-file based (most reliable)
    if (Get-ChildItem $path -Recurse -Filter "IAppDbContext.cs"  -ErrorAction SilentlyContinue | Select-Object -First 1) { return "Application" }
    if (Get-ChildItem $path -Recurse -Filter "AppDbContext.cs"   -ErrorAction SilentlyContinue | Select-Object -First 1) { return "Infrastructure" }
    if (Get-ChildItem $path -Recurse -Filter "AuditableEntity.cs" -ErrorAction SilentlyContinue | Select-Object -First 1) { return "Domain" }

    # Name-pattern based (fallback)
    if ($name -match "Application")  { return "Application"    }
    if ($name -match "Domain")        { return "Domain"         }
    if ($name -match "Infrastructur") { return "Infrastructure" }
    if ($name -match "Contracts|Shared") { return "Contracts"  }
    if ($name -match "Tests?$")       { return "Tests"         }
    if ($name -match "Web|Api")       { return "Web"           }

    return $null
}

# ── Read root namespace from .csproj ──────────────────────────────────────
function Get-RootNamespace([string]$csprojPath) {
    try {
        [xml]$xml = Get-Content $csprojPath -Raw
        $rns = $xml.Project.PropertyGroup.RootNamespace | Select-Object -First 1
        if ($rns) { return $rns }
        $an = $xml.Project.PropertyGroup.AssemblyName | Select-Object -First 1
        if ($an) { return $an }
    } catch { }
    # Fall back to filename (most reliable — always matches the actual project name)
    return [System.IO.Path]::GetFileNameWithoutExtension($csprojPath)
}

# ══════════════════════════════════════════════════════════════════════════
#  MAIN — Discovery
# ══════════════════════════════════════════════════════════════════════════
$scanRoot = if ($Root) { $Root } else { (Get-Location).Path }

Write-Host ""
Write-Host "  ╔══════════════════════════════════════════════════╗" -ForegroundColor DarkGray
Write-Host "  ║   Feature-Auto  —  Auto-Discovery Wrapper  v2   ║" -ForegroundColor Cyan
Write-Host "  ╚══════════════════════════════════════════════════╝" -ForegroundColor DarkGray
Write-Host ""
Write-Info "Scanning from: $scanRoot"

$slnRoot = Find-SolutionRoot $scanRoot
if (-not $slnRoot) {
    Write-Fail "No .sln file found. Searched:"
    Write-Warn "  Up from working dir : $scanRoot"
    Write-Warn "  Up from script dir  : $PSScriptRoot"
    Write-Warn "  One level down from : $scanRoot"
    Write-Host ""
    Write-Host "  Tips:" -ForegroundColor DarkYellow
    Write-Host "    - Make sure your .sln file exists in the solution folder" -ForegroundColor Gray
    Write-Host "    - Pass -Root explicitly:  .\Feature-Auto.ps1 new Employee -All -WithAll -Root 'C:\path	o\solution'" -ForegroundColor Gray
    exit 1
}
Write-Info "Solution root: $slnRoot"

$projects = Get-SlnProjects $slnRoot
if ($projects.Count -eq 0) {
    $slnFile = Get-ChildItem -Path $slnRoot -ErrorAction SilentlyContinue | Where-Object { $_.Extension -eq ".slnx" -or $_.Extension -eq ".sln" } | Select-Object -First 1
    Write-Fail "No .csproj files found in: $($slnFile.Name)"
    Write-Warn "Solution file path: $($slnFile.FullName)"
    Write-Warn "Please paste the first 10 lines of your .slnx so the parser can be verified."
    exit 1
}

# Assign roles
$roleMap = @{}
foreach ($p in $projects) {
    $role = Get-ProjectRole $p
    if ($role -and -not $roleMap.ContainsKey($role)) {
        $roleMap[$role] = $p
    }
}

# Namespace — strip role suffix from Application project name
$appProj   = $roleMap["Application"]
$namespace = $null
if ($appProj) {
    $namespace = Get-RootNamespace $appProj.Csproj
    # Strip .Application suffix if present to get the root namespace
    $namespace = $namespace -replace "\.(Application|Domain|Infrastructure|Web|Api|Tests?|Contracts|Shared)$", ""
}

if (-not $namespace) {
    Write-Fail "Could not determine root namespace. Add RootNamespace to your Application .csproj, or use Feature.ps1 with a config file."
    exit 1
}

# ── Print discovered layout ────────────────────────────────────────────────
Write-Host ""
Write-Host "  Discovered:" -ForegroundColor DarkGray
Write-Found "Namespace"    $namespace
Write-Found "Application"  ($appProj?.Path  ?? "(not found)")
Write-Found "Domain"       ($roleMap["Domain"]?.Path         ?? "(not found)")
Write-Found "Infrastructure" ($roleMap["Infrastructure"]?.Path ?? "(not found)")
Write-Found "Web"          ($roleMap["Web"]?.Path            ?? "(not found)")
Write-Found "Tests"        ($roleMap["Tests"]?.Path          ?? "(not found)")
Write-Found "Contracts"    ($roleMap["Contracts"]?.Path      ?? "(not found)")
Write-Host ""

# ── Resolve script paths ───────────────────────────────────────────────────
$newScript    = Join-Path $PSScriptRoot "New-Feature.ps1"
$removeScript = Join-Path $PSScriptRoot "Remove-Feature.ps1"

foreach ($s in @($newScript, $removeScript)) {
    if (-not (Test-Path $s)) {
        Write-Fail "Script not found: $s"
        exit 1
    }
}

# ── Build splat ────────────────────────────────────────────────────────────
$splat = @{ Namespace = $namespace; Feature = $Feature }

if ($appProj)                    { $splat.AppProjectPath       = $appProj.Path }
if ($roleMap["Domain"])          { $splat.DomainProjectPath    = $roleMap["Domain"].Path }
if ($roleMap["Infrastructure"])  { $splat.InfraProjectPath     = $roleMap["Infrastructure"].Path }
if ($roleMap["Web"])             { $splat.WebProjectPath       = $roleMap["Web"].Path }
if ($roleMap["Tests"])           { $splat.TestProjectPath      = $roleMap["Tests"].Path }
if ($roleMap["Contracts"])       { $splat.ContractsProjectPath = $roleMap["Contracts"].Path }

# ── Dispatch ───────────────────────────────────────────────────────────────
if ($Action -in @("new", "n")) {
    if ($HasCreate)         { $splat.HasCreate         = $true }
    if ($HasUpdate)         { $splat.HasUpdate         = $true }
    if ($HasDelete)         { $splat.HasDelete         = $true }
    if ($HasGetById)        { $splat.HasGetById        = $true }
    if ($HasGetAll)         { $splat.HasGetAll         = $true }
    if ($All)               { $splat.All               = $true }
    if ($WithEntity)        { $splat.WithEntity        = $true }
    if ($WithMappings)      { $splat.WithMappings      = $true }
    if ($WithTests)         { $splat.WithTests         = $true }
    if ($WithMvcController) { $splat.WithMvcController = $true }
    if ($WithViews)         { $splat.WithViews         = $true }
    if ($WithAll)           { $splat.WithAll           = $true }
    if ($DryRun)            { $splat.DryRun            = $true }

    & $newScript @splat

} elseif ($Action -in @("remove", "r")) {
    if ($Force)  { $splat.Force  = $true }
    if ($DryRun) { $splat.DryRun = $true }

    & $removeScript @splat
}