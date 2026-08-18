namespace Persiltech.Site.Models;

/// <summary>
/// Perfil profesional que alimenta las páginas de portafolio.
/// </summary>
/// <param name="FullName">Nombre completo con el que se firma el sitio.</param>
/// <param name="Headline">Titular de una línea que resume a qué te dedicas.</param>
/// <param name="Introduction">Párrafo de presentación de la portada.</param>
/// <param name="Biography">Presentación extendida de la página de trayectoria.</param>
/// <param name="Skills">Tecnologías agrupadas por categoría.</param>
/// <param name="Experience">Trayectoria, de lo más reciente a lo más antiguo.</param>
/// <param name="ContactMethods">Vías de contacto que se publican.</param>
public sealed record SiteProfile(
    string FullName,
    string Headline,
    string Introduction,
    string Biography,
    IReadOnlyList<SkillGroup> Skills,
    IReadOnlyList<ExperienceEntry> Experience,
    IReadOnlyList<ContactMethod> ContactMethods);
