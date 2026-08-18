namespace Persiltech.Site.Models;

/// <summary>
/// Vía de contacto que se ofrece en la página de contacto.
/// </summary>
/// <param name="Icon">Icono de MudBlazor que la representa.</param>
/// <param name="Label">Nombre de la vía (Ej. <c>Correo</c>).</param>
/// <param name="Value">Texto visible del enlace.</param>
/// <param name="Url">Destino del enlace.</param>
public sealed record ContactMethod(string Icon, string Label, string Value, string Url);
