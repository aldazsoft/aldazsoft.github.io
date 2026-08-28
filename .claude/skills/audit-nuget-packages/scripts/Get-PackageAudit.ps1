#Requires -Version 7.0
<#
.SYNOPSIS
    Audita, sin escribir nada, el estado de homologación de todos los repositorios de paquetes.

.DESCRIPTION
    Este script NO decide qué significa "homologado". Descubre la flota e invoca sobre cada
    paquete el plan de homologación (Get-HomologationPlan.ps1, del skill global
    homologate-nuget-package), que es la única definición del estándar. Lo que aporta aquí es lo
    que el plan no puede saber porque solo ve un paquete: el descubrimiento de la flota, la ruta
    asignada a cada uno y si ya está documentado en el portafolio.

    La flota vive hoy en un monorepo —un repositorio, un .slnx, un proyecto empaquetable por
    paquete bajo src/—, y de ahí sale la mayoría. Los paquetes que todavía tienen repositorio
    propio se auditan igual, pasando sus directorios en -legacyPackagesRoot.

    Lo que es del repositorio y no del paquete —la solución, la gestión centralizada, la
    licencia, los workflows— el plan lo emite aparte, en 'repositoryStructure'. El auditor lo
    reporta UNA vez por repositorio en lugar de repetirlo en los diez paquetes, que es lo que
    haría un recuento ingenuo y lo que convertiría un pendiente en diez.

    Nada se modifica. El plan compila cada proyecto para medir los avisos, lo que genera 'obj/'
    y 'bin/'; con -fast se omite esa medición.

.PARAMETER monorepoRoot
    Raíz del monorepo que publica los paquetes. Se audita cada proyecto empaquetable de su src/.

.PARAMETER legacyPackagesRoot
    Directorio que contiene los repositorios de un solo paquete que aún no se han trasladado al
    monorepo, uno por subdirectorio. Opcional.

.PARAMETER siteUrl
    Dirección del sitio, sin barra final. De aquí sale la URL canónica esperada de cada
    paquete: {siteUrl}/{route}/.

.PARAMETER sitePath
    Raíz del repositorio del portafolio, de donde salen specs/Packages.md, las páginas .razor
    y la lista de rutas del generador.

.PARAMETER package
    Uno o más paquetes a auditar, por su id o por el nombre de su directorio. Si se omite, se
    auditan todos.

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
    [string] $monorepoRoot,

    [string] $legacyPackagesRoot,

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

    $inventory = @{ declared = @{}; routes = @(); pages = @() }

    if ([string]::IsNullOrWhiteSpace($sitePath) -or -not (Test-Path $sitePath)) {
        return $inventory
    }

    $specPath = Join-Path $sitePath 'specs/Packages.md'

    if (Test-Path $specPath) {
        $currentId = $null

        foreach ($line in (Get-Content -Path $specPath)) {
            if ($line -match '^\s*-\s+id:\s*(\S+)') {
                $currentId = $Matches[1]
                $inventory.declared[$currentId] = @{ route = $null; project = $null; path = $null }
            }
            elseif ($currentId -and $line -match '^\s*route:\s*(\S+)') {
                $inventory.declared[$currentId].route = $Matches[1]
            }
            elseif ($currentId -and $line -match '^\s*project:\s*(\S+)') {
                $inventory.declared[$currentId].project = $Matches[1]
            }
            elseif ($currentId -and $line -match '^\s*path:\s*(\S+)') {
                $inventory.declared[$currentId].path = $Matches[1]
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

if (-not (Test-Path $monorepoRoot)) {
    Write-Output "CRITICAL_ERROR: No existe el monorepo '$monorepoRoot'"
    exit 1
}

if ($legacyPackagesRoot -and -not (Test-Path $legacyPackagesRoot)) {
    Write-Output "CRITICAL_ERROR: No existe el directorio de repositorios sueltos '$legacyPackagesRoot'"
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

# ---------------------------------------------------------------------------- Descubrimiento

# Cada elemento es un paquete a auditar: el repositorio en el que vive y su id dentro de él.
# Del monorepo se descubren preguntándole al propio plan, que es quien sabe qué .csproj son
# empaquetables; de los repositorios sueltos, uno por subdirectorio, como siempre.
$targets = [System.Collections.Generic.List[pscustomobject]]::new()

$listing = & pwsh -NoProfile -ExecutionPolicy Bypass -File $planScript `
    -packagePath $monorepoRoot -listPackages -asJson 2>&1 | Out-String

if ($listing -match 'CRITICAL_ERROR') {
    Write-Output ($listing -split "`n" | Where-Object { $_ -match 'CRITICAL_ERROR' } | Select-Object -First 1).Trim()
    Write-Output "El monorepo '$monorepoRoot' no expone ningún paquete legible. Sin eso no hay flota que auditar."
    exit 1
}

try {
    $monorepoPackages = ($listing | ConvertFrom-Json).packages
}
catch {
    Write-Output "CRITICAL_ERROR: El plan no devolvió una lista de paquetes legible para '$monorepoRoot'."
    Write-Output "Comprueba que Get-HomologationPlan.ps1 admite -listPackages: es la versión que este auditor necesita."
    exit 1
}

foreach ($entry in $monorepoPackages) {
    $targets.Add([pscustomobject] @{
            id            = $entry.id
            directory     = $entry.directory
            repositoryRoot = $monorepoRoot
            repository    = [System.IO.Path]::GetFileName($monorepoRoot.TrimEnd('\', '/'))
            inMonorepo    = $true
        })
}

# De los repositorios sueltos NO se audita todo lo que haya en el directorio: al trasladar la
# flota al monorepo, el repositorio de origen se queda ahí con una copia del paquete, y
# recorrerlo a ciegas auditaría cada paquete dos veces —dando por pendiente en la copia vieja lo
# que ya se arregló en el monorepo—. La lista válida es la de specs/Packages.md: solo las
# entradas que declaran 'path' siguen viviendo fuera.
$abandonedRepositories = @()

if ($legacyPackagesRoot) {
    $declaredPaths = @(
        $siteInventory.declared.Keys |
            Where-Object { $siteInventory.declared[$_].path } |
            ForEach-Object { $siteInventory.declared[$_].path })

    foreach ($directory in @(Get-ChildItem -Path $legacyPackagesRoot -Directory | Where-Object { $_.Name -notmatch '^\.' })) {
        if ($declaredPaths -notcontains $directory.Name) {
            $abandonedRepositories += $directory.Name
            continue
        }

        $targets.Add([pscustomobject] @{
                id            = $null
                directory     = $directory.Name
                repositoryRoot = $directory.FullName
                repository    = $directory.Name
                inMonorepo    = $false
            })
    }
}

if ($package) {
    $targets = [System.Collections.Generic.List[pscustomobject]] @(
        $targets | Where-Object { $package -contains $_.id -or $package -contains $_.directory })
}

$results = @()

# Los pendientes del repositorio se reportan una vez por repositorio, no una por paquete: en un
# monorepo de diez, repetirlos convertiría un trabajo en diez.
$repositoryFindings = [ordered] @{}

foreach ($target in $targets) {

    # La ruta del paquete en el sitio la conoce el portafolio, no el repositorio. En el monorepo
    # el id se conoce de antemano; en un repositorio suelto lo dice el plan, así que hasta
    # entonces se busca por el directorio que specs/Packages.md le asigna.
    $declaredId = $target.id

    if (-not $declaredId) {
        $declaredId = @(
            $siteInventory.declared.Keys |
                Where-Object { $siteInventory.declared[$_].path -eq $target.directory }) | Select-Object -First 1
    }
    elseif (-not $siteInventory.declared.ContainsKey($declaredId)) {
        $declaredId = $null
    }

    $route = $declaredId ? $siteInventory.declared[$declaredId].route : $null

    $arguments = @(
        '-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', $planScript,
        '-packagePath', $target.repositoryRoot, '-asJson'
    )

    if ($target.inMonorepo) { $arguments += @('-packageId', $target.id) }
    if ($route) { $arguments += @('-expectedProjectUrl', "$siteUrl/$route/") }
    if ($fast) { $arguments += '-skipBuild' }

    $output = & pwsh @arguments 2>&1 | Out-String

    if ($output -match 'CRITICAL_ERROR') {
        $results += [pscustomobject] [ordered] @{
            directory  = $target.directory
            repository = $target.repository
            id         = $target.id ?? $declaredId
            state      = 'no-legible'
            reason     = ($output -split "`n" | Where-Object { $_ -match 'CRITICAL_ERROR' } | Select-Object -First 1).Trim()
            questions  = @()
            stages     = $null
            site       = $null
        }

        continue
    }

    try {
        $plan = $output | ConvertFrom-Json
    }
    catch {
        $results += [pscustomobject] [ordered] @{
            directory  = $target.directory
            repository = $target.repository
            id         = $target.id ?? $declaredId
            state      = 'no-legible'
            reason     = 'El plan no devolvió JSON válido.'
            questions  = @()
            stages     = $null
            site       = $null
        }

        continue
    }

    # Los pendientes del repositorio son los mismos para todos sus paquetes: se guardan una vez.
    if (-not $repositoryFindings.Contains($target.repository)) {
        $repositoryFindings[$target.repository] = [ordered] @{
            root     = $target.repositoryRoot
            layout   = $plan.layout
            packages = 0
            pending  = @($plan.repositoryStructure)
        }
    }

    $repositoryFindings[$target.repository].packages++

    $siteRoute = $route ?? ($plan.packageId -replace '^[^.]+\.', '')

    $siteBlockers = @()

    if (-not $declaredId) { $siteBlockers += 'No está declarado en specs/Packages.md.' }
    if ($siteInventory.pages -notcontains $siteRoute) { $siteBlockers += "Sin página /$siteRoute en el sitio." }
    if ($siteInventory.routes -notcontains $siteRoute) { $siteBlockers += "Sin la ruta '$siteRoute' en build-pages.sh." }

    if ($declaredId -and $declaredId -ne $plan.packageId) {
        $siteBlockers += "specs/Packages.md lo declara como '$declaredId' y el .csproj como '$($plan.packageId)'."
    }

    # El estado es la primera etapa sin terminar. Las cuatro primeras las decide el plan; la
    # quinta es cosa del portafolio. 'repositoryStructure' NO entra: es del repositorio, y
    # dejaría los diez paquetes de un monorepo en 'estructura' por un solo workflow que falta.
    $state = 'listo'

    foreach ($stage in @('estructura', 'metadata', 'documentacion', 'calidad')) {
        if (@($plan.stages.$stage).Count -gt 0) { $state = $stage; break }
    }

    if ($state -eq 'listo' -and $siteBlockers.Count -gt 0) { $state = 'sin-página' }

    # Sin medir la calidad, 'listo' prometería más de lo que se comprobó.
    if ($state -eq 'listo' -and -not $plan.warnings.ran) { $state = 'sin-medir' }

    $results += [pscustomobject] [ordered] @{
        directory  = $target.directory
        repository = $target.repository
        id         = $plan.packageId
        state      = $state
        version    = $plan.version
        route      = $siteRoute
        readme     = $plan.readme
        specs      = $plan.specs
        questions  = @($plan.questions)
        stages     = $plan.stages
        warnings   = $plan.warnings
        site       = [ordered] @{ route = $siteRoute; blockers = $siteBlockers }
    }
}

if (-not $asText) {
    [ordered] @{
        repositories = $repositoryFindings
        packages     = $results
    } | ConvertTo-Json -Depth 8

    return
}

$order = @{
    'no-legible' = 0; 'estructura' = 1; 'metadata' = 2; 'documentacion' = 3
    'calidad' = 4; 'sin-página' = 5; 'sin-medir' = 6; 'listo' = 7
}

Write-Output "Auditados $($results.Count) paquetes en $($repositoryFindings.Count) repositorio(s)"

if ($fast) {
    Write-Output "Modo -fast: la etapa de calidad no se midió, así que ningún paquete puede salir 'listo'."
}

if ($abandonedRepositories.Count -gt 0) {
    Write-Output ''
    Write-Output "No auditados ($($abandonedRepositories.Count)): están en '$legacyPackagesRoot' pero specs/Packages.md no los declara con 'path',"
    Write-Output "así que su paquete ya vive en el monorepo y esto es la copia que quedó atrás:"
    Write-Output "  $($abandonedRepositories -join ', ')"
}

Write-Output ''

# Primero lo del repositorio, porque se arregla una vez y desbloquea a todos sus paquetes.
foreach ($name in $repositoryFindings.Keys) {
    $repository = $repositoryFindings[$name]
    $count = @($repository.pending).Count

    Write-Output "REPOSITORIO $name ($($repository.layout), $($repository.packages) paquete$($repository.packages -eq 1 ? '' : 's')): $count pendiente$($count -eq 1 ? '' : 's')"

    foreach ($item in $repository.pending) { Write-Output "  - $item" }

    Write-Output ''
}

foreach ($group in ($results | Group-Object state | Sort-Object { $order[$_.Name] })) {
    Write-Output "[$($group.Name)] $($group.Count)"

    foreach ($item in $group.Group) {
        $pending = 0

        if ($item.stages) {
            foreach ($stage in @('estructura', 'metadata', 'documentacion', 'calidad')) {
                $pending += @($item.stages.$stage).Count
            }
        }

        # 'site' es nulo en los 'no-legible': el plan no llegó a decir nada de ellos.
        if ($item.site) { $pending += @($item.site.blockers).Count }

        $suffix = $pending -gt 0 ? " ($pending pendientes)" : ''
        Write-Output "  - $($item.id ?? $item.directory)$suffix"

        if ($item.state -eq 'no-legible') { Write-Output "      $($item.reason)" }
    }

    Write-Output ''
}

$questions = @($results | Where-Object { @($_.questions).Count -gt 0 })

if ($questions.Count -gt 0) {
    # En un monorepo hay preguntas que son del repositorio —si el código es público, a qué
    # remoto apunta— y el plan las emite en cada paquete porque solo ve uno. Agrupar por el
    # texto las deja como lo que son: una pregunta, no diez.
    Write-Output "PREGUNTAS BLOQUEANTES, agrupadas:"

    foreach ($group in ($questions | ForEach-Object {
                $item = $_
                $item.questions | ForEach-Object { [pscustomobject] @{ question = $_; id = $item.id } }
            } | Group-Object question)) {

        Write-Output "  ? $($group.Name)"
        Write-Output "      afecta a: $((($group.Group | ForEach-Object { $_.id }) | Sort-Object -Unique) -join ', ')"
    }
}
