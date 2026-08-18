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
            new PackageRelease("0.1.7", "Metadata de empaquetado adaptada a un repositorio privado."),
            new PackageRelease("0.1.6", "Enlace absoluto al texto de la licencia en el README."),
            new PackageRelease("0.1.5", "Icono del paquete y documentación al día."),
            new PackageRelease("0.1.4", "Documentación y metadata de empaquetado al día."),
            new PackageRelease("0.1.0 – 0.1.3", "Primeras publicaciones de IUserService.")
        ]);

    private static readonly IReadOnlyList<NuGetPackage> Packages = [UserServicesAbstractions];

    /// <inheritdoc />
    public IReadOnlyList<NuGetPackage> GetAll() => Packages;

    /// <inheritdoc />
    public NuGetPackage? Find(string packageId) =>
        Packages.FirstOrDefault(p => p.Id == packageId);
}
