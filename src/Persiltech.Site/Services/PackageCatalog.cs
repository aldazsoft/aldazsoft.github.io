namespace Persiltech.Site.Services;

/// <inheritdoc cref="IPackageCatalog" />
/// <remarks>
/// El catálogo se declara en código porque el sitio es estático: no hay servidor
/// que consultar, y el número de paquetes es pequeño y cambia con cada publicación.
/// </remarks>
public sealed class PackageCatalog : IPackageCatalog
{
    private static readonly NuGetPackage UserServicesAbstractions = new(
        Id: "Persiltech.UserServices.Abstractions",
        Route: "/UserServices.Abstractions",
        Summary: "El Output Port IUserService: estado de autenticación e identidad del usuario actual para soluciones con Arquitectura Limpia.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.1.8", "Texto de la licencia empaquetado dentro del .nupkg."),
            new PackageRelease("0.1.7", "Metadata de empaquetado adaptada a un repositorio privado."),
            new PackageRelease("0.1.6", "Enlace absoluto al texto de la licencia en el README."),
            new PackageRelease("0.1.5", "Icono del paquete y documentación al día."),
            new PackageRelease("0.1.4", "Documentación y metadata de empaquetado al día."),
            new PackageRelease("0.1.0 – 0.1.3", "Primeras publicaciones de IUserService.")
        ]);

    private static readonly NuGetPackage UserServices = new(
        Id: "Persiltech.UserServices",
        Route: "/UserServices",
        Summary: "El adaptador de ASP.NET Core para IUserService: resuelve la identidad y el estado de autenticación desde HttpContext.User.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.1.2", "Página del proyecto en el portafolio, metadata adaptada a un repositorio privado y dependencia de Abstractions al día."),
            new PackageRelease("0.1.1", "Primera publicación del adaptador HttpContextUserService.")
        ]);

    // El contrato va primero y su adaptador después: es el orden en que se leen.
    private static readonly IReadOnlyList<NuGetPackage> Packages = [UserServicesAbstractions, UserServices];

    /// <inheritdoc />
    public IReadOnlyList<NuGetPackage> GetAll() => Packages;

    /// <inheritdoc />
    public NuGetPackage? Find(string packageId) =>
        Packages.FirstOrDefault(p => p.Id == packageId);
}
