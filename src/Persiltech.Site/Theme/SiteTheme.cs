namespace Persiltech.Site.Theme;

/// <summary>
/// Tema de MudBlazor que comparte todo el sitio, con sus paletas clara y oscura.
/// </summary>
public static class SiteTheme
{
    /// <summary>
    /// Obtiene el tema aplicado por <c>MudThemeProvider</c> en el diseño principal.
    /// </summary>
    public static MudTheme Instance { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#0a58ca",
            Secondary = "#5a636e",
            AppbarBackground = "#ffffff",
            AppbarText = "#1b1f24",
            Background = "#ffffff",
            Surface = "#f6f8fa",
            TextPrimary = "#1b1f24",
            TextSecondary = "#5a636e",
            LinesDefault = "#d8dee4"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#6cb6ff",
            Secondary = "#9198a1",
            AppbarBackground = "#0d1117",
            AppbarText = "#e6edf3",
            Background = "#0d1117",
            Surface = "#161b22",
            TextPrimary = "#e6edf3",
            TextSecondary = "#9198a1",
            LinesDefault = "#30363d"
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "8px"
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Roboto", "Segoe UI", "Helvetica", "Arial", "sans-serif"]
            }
        }
    };
}
