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
            new PackageRelease("0.1.11", "Apartado de licencia retirado del README; ya lo publica nuget.org."),
            new PackageRelease("0.1.10", "Historial de versiones al día en el README."),
            new PackageRelease("0.1.9", "Insignia de licencia enlazada al texto real, no a la plantilla."),
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
            new PackageRelease("0.1.4", "La dependencia mínima de Abstractions baja de 0.1.8 a 0.1.0: instalar el paquete deja de forzar una actualización que nadie necesitaba."),
            new PackageRelease("0.1.3", "Insignia de licencia enlazada al texto real, no a la plantilla."),
            new PackageRelease("0.1.2", "Página del proyecto en el portafolio y metadata adaptada a un repositorio privado."),
            new PackageRelease("0.1.1", "Licencia publicada como archivo dentro del paquete."),
            new PackageRelease("0.1.0", "Primera publicación del adaptador HttpContextUserService.")
        ]);

    private static readonly NuGetPackage DomainValidation = new(
        Id: "Persiltech.DomainValidation",
        Route: "/DomainValidation",
        Summary: "Validación de reglas de negocio con el patrón Specification: reglas fluidas por propiedad, evaluación asíncrona y errores reunidos en un ValidationResult.",
        TargetFramework: "net10.0",
        IsPrerelease: false,
        Releases:
        [
            new PackageRelease("1.0.2", "La página del proyecto pasa a ser esta. El texto real de la licencia viaja dentro del .nupkg, el README documenta la superficie pública y el .nuspec deja de declarar el repositorio, que no es público."),
            new PackageRelease("1.0.1", "Primera versión disponible en nuget.org; reemplaza a la 1.0.0, retirada del listado.")
        ]);

    // El contrato va primero y su adaptador después: es el orden en que se leen.
    // DomainValidation cierra la lista: no depende de los otros dos.
    private static readonly IReadOnlyList<NuGetPackage> Packages =
        [UserServicesAbstractions, UserServices, DomainValidation];

    /// <inheritdoc />
    public IReadOnlyList<NuGetPackage> GetAll() => Packages;

    /// <inheritdoc />
    public NuGetPackage? Find(string packageId) =>
        Packages.FirstOrDefault(p => p.Id == packageId);
}
