namespace Persiltech.Site;

/// <summary>
/// Direcciones externas que el sitio repite en varias páginas.
/// </summary>
/// <remarks>
/// Viven en un único sitio para que cambiar el correo de soporte o el destino
/// del patrocinio no obligue a recorrer los componentes uno por uno.
/// </remarks>
public static class SiteLinks
{
    /// <summary>
    /// Correo al que se dirigen las dudas, los informes de error y las peticiones de mejora.
    /// </summary>
    public const string SupportEmail = "eduar2083@gmail.com";

    /// <summary>
    /// Perfil de GitHub Sponsors que recibe el apoyo económico.
    /// </summary>
    public const string SponsorUrl = "https://github.com/sponsors/aldazsoft";

    /// <summary>
    /// Texto de la licencia MIT bajo la que se publican los paquetes.
    /// </summary>
    public const string LicenseUrl = "https://licenses.nuget.org/MIT";

    /// <summary>
    /// Enlace <c>mailto:</c> derivado de <see cref="SupportEmail"/>.
    /// </summary>
    public static string SupportMailTo => $"mailto:{SupportEmail}";
}
