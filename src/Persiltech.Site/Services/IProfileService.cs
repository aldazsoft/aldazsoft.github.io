namespace Persiltech.Site.Services;

/// <summary>
/// Perfil profesional que se muestra en las páginas de portafolio.
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Obtiene el perfil publicado en el sitio.
    /// </summary>
    /// <returns>El perfil con su trayectoria, tecnologías y vías de contacto.</returns>
    SiteProfile GetProfile();
}
