#Requires -Version 7.0
<#
.SYNOPSIS
    Ordena los paquetes que quedan en el monorepo por relación entre lo que aportan y lo que
    cuesta extraerlos. No modifica nada.

.DESCRIPTION
    Recorre los proyectos de Src/ y mide, para cada uno:

      - dependientes : cuántos proyectos del monorepo lo consumen por PackageReference
      - dependencias : cuántos paquetes consume él
      - internas     : cuáles de esas son de la casa y siguen sin extraer
      - archivos     : archivos .cs, sin obj/ ni bin/
      - avisos       : CS1591 al compilar con Nullable y GenerateDocumentationFile forzados,
                       que es el coste real de la etapa de documentación

    El orden por defecto es por 'score' = dependientes / (1 + avisos/10): favorece lo que más
    se usa y menos cuesta documentar. Es una guía, no un veredicto: la cola acordada vive en
    specs/Extraction.md.

    Compilar cada proyecto lleva tiempo. Con -skipBuild se omite y la columna de avisos sale
    vacía, pero entonces el orden pierde su mitad más cara.

.PARAMETER monorepoRoot
    Raíz del monorepo, el directorio que contiene Src/.

.PARAMETER top
    Cuántos candidatos mostrar. Por defecto 10.

.PARAMETER package
    Uno o más nombres de proyecto concretos. Si se indica, se miden solo esos.

.PARAMETER skipBuild
    Omite la compilación y, con ella, el recuento de avisos.

.PARAMETER asJson
    Emite el resultado como JSON en lugar de la tabla.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $monorepoRoot,

    [int] $top = 10,

    [string[]] $package,

    [switch] $skipBuild,

    [switch] $asJson
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$sourceRoot = Join-Path $monorepoRoot 'Src'

if (-not (Test-Path $sourceRoot)) {
    Write-Output "CRITICAL_ERROR: No existe '$sourceRoot'. ¿Es '$monorepoRoot' la raíz del monorepo?"
    exit 1
}

# El grafo se construye SIEMPRE sobre todos los proyectos: quién consume a quién no depende
# de lo que se pida medir. Filtrar aquí daría recuentos de dependientes falsos.
$allProjects = @(Get-ChildItem -Path $sourceRoot -Directory | Sort-Object Name)

$projects = $package ? @($allProjects | Where-Object { $package -contains $_.Name }) : $allProjects

if ($projects.Count -eq 0) {
    Write-Output 'No quedan proyectos que medir.'
    return
}

# Un solo recorrido de todos los .csproj: de aquí salen tanto las dependencias de cada uno
# como el recuento de quién consume a quién.
$references = @{}

foreach ($project in $allProjects) {
    $projectFile = Join-Path $project.FullName "$($project.Name).csproj"

    if (-not (Test-Path $projectFile)) {
        $references[$project.Name] = @()
        continue
    }

    $references[$project.Name] = @(
        [regex]::Matches(
            (Get-Content -Path $projectFile -Raw),
            '<PackageReference\s+Include="([^"]+)"') |
            ForEach-Object { $_.Groups[1].Value })
}

$existing = @($allProjects | ForEach-Object { $_.Name })
$results = @()

foreach ($project in $projects) {
    $name = $project.Name
    $projectFile = Join-Path $project.FullName "$name.csproj"

    if (-not (Test-Path $projectFile)) {
        continue
    }

    $dependents = @($references.Keys | Where-Object { $references[$_] -contains $name }).Count
    $dependencies = @($references[$name])

    # Las internas que siguen en el monorepo importan: al homologar este, querrá fijar la
    # versión nueva de aquellas cuando les llegue el turno.
    $internalPending = @(
        $dependencies | Where-Object { $_ -like 'Persiltech.*' -and $existing -contains $_ })

    $files = @(Get-ChildItem -Path $project.FullName -Filter '*.cs' -Recurse -File -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' }).Count

    $warnings = $null

    if (-not $skipBuild) {
        $output = & dotnet build $projectFile --no-incremental `
            -p:Nullable=enable -p:GenerateDocumentationFile=true 2>&1 | Out-String

        $warnings = $LASTEXITCODE -eq 0 ? @([regex]::Matches($output, 'warning CS1591')).Count : -1
    }

    # Cuanto más se use y menos cueste documentar, mejor. El -1 marca 'no compila', que hunde
    # el candidato a propósito: hay que arreglarlo antes de plantearse extraerlo.
    $score = if ($null -eq $warnings) { $dependents }
             elseif ($warnings -lt 0) { -1 }
             else { [math]::Round($dependents / (1 + $warnings / 10), 2) }

    $results += [pscustomobject] [ordered] @{
        package        = $name
        dependents     = $dependents
        dependencies   = $dependencies.Count
        internalPending = $internalPending
        files          = $files
        warnings       = $warnings
        # El monorepo nombra a los acompañantes en plural y en singular: HttpDelegatingHandlers
        # tiene un '.Test'. Buscar solo '.Tests' los daba por inexistentes.
        hasTests       = @('Tests', 'Test') | Where-Object {
            Test-Path (Join-Path $monorepoRoot "Tests/$name.$_") } | Select-Object -First 1 | ForEach-Object { $true }
        score          = $score
    }
}

$ranked = @($results | Sort-Object -Property @{ Expression = 'score'; Descending = $true },
                                             @{ Expression = 'dependents'; Descending = $true },
                                             @{ Expression = 'warnings'; Descending = $false })

if ($asJson) {
    $ranked | ConvertTo-Json -Depth 5
    return
}

if ($package) {
    Write-Output "Medidos $($results.Count) de los $($allProjects.Count) proyectos que quedan en $sourceRoot"
}
else {
    Write-Output "Quedan $($allProjects.Count) proyectos en $sourceRoot"
}

if ($skipBuild) {
    Write-Output 'Modo -skipBuild: los avisos no se midieron, así que el orden ignora el coste de documentación.'
}

Write-Output ''

$ranked | Select-Object -First $top | ForEach-Object {
    if ($null -eq $_.warnings) { $warningText = '   —' }
    elseif ($_.warnings -lt 0) { $warningText = 'ROTO' }
    else { $warningText = '{0,4}' -f $_.warnings }

    $pending = $_.internalPending.Count -gt 0 ? "  <- espera a: $($_.internalPending -join ', ')" : ''

    '{0,-42} usan:{1,-3} archivos:{2,-4} avisos:{3}  score:{4,-6}{5}' -f `
        $_.package, $_.dependents, $_.files, $warningText, $_.score, $pending
}

Write-Output ''
Write-Output "La cola acordada vive en specs/Extraction.md. Esto es una guía, no un veredicto."
