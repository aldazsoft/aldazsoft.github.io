namespace Persiltech.Site.Services;

/// <summary>
/// Catálogo de los paquetes que el sitio documenta.
/// </summary>
public interface IPackageCatalog
{
    /// <summary>
    /// Obtiene todos los paquetes publicados, en el orden en que se listan en la portada.
    /// </summary>
    /// <returns>Los paquetes del catálogo.</returns>
    IReadOnlyList<NuGetPackage> GetAll();

    /// <summary>
    /// Obtiene un paquete por su identificador de nuget.org.
    /// </summary>
    /// <param name="packageId">Identificador del paquete.</param>
    /// <returns>El paquete, o <see langword="null"/> si el catálogo no lo contiene.</returns>
    NuGetPackage? Find(string packageId);
}
