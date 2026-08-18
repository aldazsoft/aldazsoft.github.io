namespace Persiltech.Site;

/// <summary>
/// Registro de los servicios propios del sitio y de los que aporta MudBlazor.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra el catálogo de paquetes y los servicios de MudBlazor.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <returns>La misma colección, para encadenar llamadas.</returns>
    public static IServiceCollection AddSiteServices(this IServiceCollection services)
    {
        services.AddMudServices();
        services.AddScoped<IPackageCatalog, PackageCatalog>();

        return services;
    }
}
