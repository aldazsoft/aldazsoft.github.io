namespace Persiltech.Site.Layout;

/// <summary>
/// Enlace de navegación del sitio.
/// </summary>
/// <param name="Href">Ruta de destino.</param>
/// <param name="Text">Texto visible.</param>
/// <param name="Icon">Icono que lo acompaña en el menú lateral.</param>
/// <param name="Match">Cómo se decide si el enlace está activo.</param>
public sealed record SiteNavLink(string Href, string Text, string Icon, NavLinkMatch Match);
