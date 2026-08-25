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
/// <param name="IsPublished">
/// Indica si el paquete ya está en nuget.org. La página se despliega <em>antes</em> que el
/// paquete —su <c>PackageProjectUrl</c> la declara como sitio oficial, y un enlace muerto solo
/// se corrige publicando otra versión—, así que hay una ventana en la que la página existe y
/// el paquete no. Mientras dura, la insignia de versión no tiene nada que mostrar.
/// </param>
public sealed record NuGetPackage(
    string Id,
    string Route,
    string Summary,
    string TargetFramework,
    bool IsPrerelease,
    IReadOnlyList<PackageRelease> Releases,
    bool IsPublished = true)
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
