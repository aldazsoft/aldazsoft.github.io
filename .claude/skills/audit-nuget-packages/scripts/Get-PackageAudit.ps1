#Requires -Version 7.0
<#
.SYNOPSIS
    Audita, sin escribir nada, el estado de homologación de todos los repositorios de paquetes.

.DESCRIPTION
    Este script NO decide qué significa "homologado". Recorre los repositorios que cuelgan de
    -packagesRoot e invoca sobre cada uno el plan de homologación
    (Get-HomologationPlan.ps1, del skill global homologate-nuget-package), que es la única
    definición del estándar. Lo que aporta aquí es lo que el plan no puede saber porque solo ve
    un repositorio: el descubrimiento de la flota, la ruta asignada a cada paquete y si ya está
    documentado en el portafolio.

    Ningún repositorio de paquete se modifica. El plan compila cada proyecto para medir los
    avisos, lo que genera 'obj/' y 'bin/'; con -fast se omite esa medición.

.PARAMETER packagesRoot
    Directorio que contiene los repositorios de los paquetes, uno por subdirectorio.

.PARAMETER siteUrl
    Dirección del sitio, sin barra final. De aquí sale la URL canónica esperada de cada
    paquete: {siteUrl}/{route}/.

.PARAMETER sitePath
    Raíz del repositorio del portafolio, de donde salen specs/Packages.md, las páginas .razor
    y la lista de rutas del generador.

.PARAMETER package
    Uno o más nombres de directorio a auditar. Si se omite, se auditan todos.

.PARAMETER planScript
    Ruta a Get-HomologationPlan.ps1. Por defecto, el del skill global.

.PARAMETER fast
    Omite la compilación de cada paquete. Mucho más rápido sobre una flota grande, pero
    entonces la etapa de calidad no se mide y ningún paquete puede declararse 'listo'.

.PARAMETER asText
    Emite un resumen legible en lugar del JSON.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $packagesRoot,

    [string] $siteUrl = 'https://aldazsoft.github.io',

    [string] $sitePath,

    [string[]] $package,

    [string] $planScript = (Join-Path $HOME '.claude/skills/homologate-nuget-package/scripts/Get-HomologationPlan.ps1'),

    [switch] $fast,

    [switch] $asText
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-SiteInventory {
    param([string] $sitePath)

    $inventory = @{ declared = @{}; byPath = @{}; routes = @(); pages = @() }

    if ([string]::IsNullOrWhiteSpace($sitePath) -or -not (Test-Path $sitePath)) {
        return $inventory
    }

    $specPath = Join-Path $sitePath 'specs/Packages.md'

    if (Test-Path $specPath) {
        $currentId = $null

        foreach ($line in (Get-Content -Path $specPath)) {
            if ($line -match '^\s*-\s+id:\s*(\S+)') {
                $currentId = $Matches[1]
                $inventory.declared[$currentId] = @{ route = $null; path = $null }
            }
            elseif ($currentId -and $line -match '^\s*route:\s*(\S+)') {
                $inventory.declared[$currentId].route = $Matches[1]
            }
            elseif ($currentId -and $line -match '^\s*path:\s*(\S+)') {
                $inventory.declared[$currentId].path = $Matches[1]
                $inventory.byPath[$Matches[1]] = $currentId
            }
        }
    }

    $buildPagesPath = Join-Path $sitePath '.github/scripts/build-pages.sh'

    if (Test-Path $buildPagesPath) {
        $inventory.routes = @(
            Select-String -Path $buildPagesPath -Pattern '^\s*"([^|"]+)\|' -AllMatches |
                ForEach-Object { $_.Matches[0].Groups[1].Value }
        )
    }

    $pagesDirectory = Join-Path $sitePath 'src/Persiltech.Site/Pages/Packages'

    if (Test-Path $pagesDirectory) {
        $inventory.pages = @(
            Select-String -Path (Join-Path $pagesDirectory '*.razor') -Pattern '^@page\s+"/([^"]+)"' |
                ForEach-Object { $_.Matches[0].Groups[1].Value }
        )
    }

    return $inventory
}

# --------------------------------------------------------------------------------------

if (-not (Test-Path $packagesRoot)) {
    Write-Output "CRITICAL_ERROR: No existe el directorio de paquetes '$packagesRoot'"
    exit 1
}

if (-not (Test-Path $planScript)) {
    Write-Output "CRITICAL_ERROR: No se encontró el plan de homologación en '$planScript'."
    Write-Output "Este skill delega en él y no comprueba el estándar por su cuenta. Instala o"
    Write-Output "localiza el skill global 'homologate-nuget-package', o indica la ruta con -planScript."
    exit 1
}

$siteInventory = Get-SiteInventory -sitePath $sitePath
$siteUrl = $siteUrl.TrimEnd('/')

$repositories = @(Get-ChildItem -Path $packagesRoot -Directory |
    Where-Object { $_.Name -notmatch '^\.' })

if ($package) {
    $repositories = @($repositories | Where-Object { $package -contains $_.Name })
}

$results = @()

foreach ($repository in $repositories) {

    # La ruta del paquete en el sitio la conoce el portafolio, no el repositorio del paquete.
    # Sin ella no hay URL esperada que comprobar, y así se le dice al plan.
    $declaredId = $siteInventory.byPath[$repository.Name]
    $route = $declaredId ? $siteInventory.declared[$declaredId].route : $null

    $arguments = @(
        '-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', $planScript,
        '-packagePath', $repository.FullName, '-asJson'
    )

    if ($route) { $arguments += @('-expectedProjectUrl', "$siteUrl/$route/") }
    if ($fast) { $arguments += '-skipBuild' }

    $output = & pwsh @arguments 2>&1 | Out-String

    if ($output -match 'CRITICAL_ERROR') {
        $results += [pscustomobject] [ordered] @{
            directory = $repository.Name
            id        = $declaredId
            state     = 'no-legible'
            reason    = ($output -split "`n" | Where-Object { $_ -match 'CRITICAL_ERROR' } | Select-Object -First 1).Trim()
            questions = @()
            stages    = $null
            site      = $null
        }

        continue
    }

    try {
        $plan = $output | ConvertFrom-Json
    }
    catch {
        $results += [pscustomobject] [ordered] @{
            directory = $repository.Name
            id        = $declaredId
            state     = 'no-legible'
            reason    = 'El plan no devolvió JSON válido.'
            questions = @()
            stages    = $null
            site      = $null
        }

        continue
    }

    $siteRoute = $route ?? ($plan.packageId -replace '^[^.]+\.', '')

    $siteBlockers = @()

    if (-not $declaredId) { $siteBlockers += 'No está declarado en specs/Packages.md.' }
    if ($siteInventory.pages -notcontains $siteRoute) { $siteBlockers += "Sin página /$siteRoute en el sitio." }
    if ($siteInventory.routes -notcontains $siteRoute) { $siteBlockers += "Sin la ruta '$siteRoute' en build-pages.sh." }

    if ($declaredId -and $declaredId -ne $plan.packageId) {
        $siteBlockers += "specs/Packages.md lo declara como '$declaredId' y el .csproj como '$($plan.packageId)'."
    }

    # El estado es la primera etapa sin terminar. Las cuatro primeras las decide el plan; la
    # quinta es cosa del portafolio.
    $state = 'listo'

    foreach ($stage in @('estructura', 'metadata', 'documentacion', 'calidad')) {
        if (@($plan.stages.$stage).Count -gt 0) { $state = $stage; break }
    }

    if ($state -eq 'listo' -and $siteBlockers.Count -gt 0) { $state = 'sin-página' }

    # Sin medir la calidad, 'listo' prometería más de lo que se comprobó.
    if ($state -eq 'listo' -and -not $plan.warnings.ran) { $state = 'sin-medir' }

    $results += [pscustomobject] [ordered] @{
        directory = $repository.Name
        id        = $plan.packageId
        state     = $state
        version   = $plan.version
        route     = $siteRoute
        questions = @($plan.questions)
        stages    = $plan.stages
        warnings  = $plan.warnings
        site      = [ordered] @{ route = $siteRoute; blockers = $siteBlockers }
    }
}

if (-not $asText) {
    $results | ConvertTo-Json -Depth 8
    return
}

$order = @{
    'no-legible' = 0; 'estructura' = 1; 'metadata' = 2; 'documentacion' = 3
    'calidad' = 4; 'sin-página' = 5; 'sin-medir' = 6; 'listo' = 7
}

Write-Output "Auditados $($results.Count) repositorios en $packagesRoot"

if ($fast) {
    Write-Output "Modo -fast: la etapa de calidad no se midió, así que ningún paquete puede salir 'listo'."
}

Write-Output ''

foreach ($group in ($results | Group-Object state | Sort-Object { $order[$_.Name] })) {
    Write-Output "[$($group.Name)] $($group.Count)"

    foreach ($item in $group.Group) {
        $pending = 0

        if ($item.stages) {
            foreach ($stage in @('estructura', 'metadata', 'documentacion', 'calidad')) {
                $pending += @($item.stages.$stage).Count
            }
        }

        $pending += @($item.site.blockers).Count

        $suffix = $pending -gt 0 ? " ($pending pendientes)" : ''
        Write-Output "  - $($item.id ?? $item.directory)$suffix"
    }

    Write-Output ''
}

$questions = @($results | Where-Object { @($_.questions).Count -gt 0 })

if ($questions.Count -gt 0) {
    Write-Output "PREGUNTAS BLOQUEANTES, agrupadas ($($questions.Count) paquetes):"

    foreach ($item in $questions) {
        Write-Output "  $($item.id):"
        foreach ($question in $item.questions) { Write-Output "    ? $question" }
    }
}
