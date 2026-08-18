namespace Persiltech.Site.Models;

/// <summary>
/// Paquete NuGet publicado, con todo lo que el sitio necesita para documentarlo.
/// </summary>
/// <param name="Id">Identificador en nuget.org (Ej. <c>Persiltech.UserServices.Abstractions</c>).</param>
/// <param name="Route">Ruta relativa de su página dentro del sitio.</param>
/// <param name="Summary">Descripción breve, la misma que publica el paquete.</param>
/// <param name="TargetFramework">Moniker del framework de destino (Ej. <c>net10.0</c>).</param>
/// <param name="IsPrerelease">Indica si la superficie pública todavía puede cambiar entre versiones menores.</param>
/// <param name="Releases">Historial de versiones publicadas, de la más reciente a la más antigua.</param>
public sealed record NuGetPackage(
    string Id,
    string Route,
    string Summary,
    string TargetFramework,
    bool IsPrerelease,
    IReadOnlyList<PackageRelease> Releases)
{
    /// <summary>
    /// Dirección de la página del paquete en nuget.org.
    /// </summary>
    public string NuGetUrl => $"https://www.nuget.org/packages/{Id}/";

    /// <summary>
    /// Dirección de la insignia que muestra la versión publicada más reciente.
    /// </summary>
    public string VersionBadgeUrl => $"https://img.shields.io/nuget/v/{Id}.svg";

    /// <summary>
    /// Comando que instala el paquete en un proyecto.
    /// </summary>
    public string InstallCommand => $"dotnet add package {Id}";
}
