namespace Persiltech.Site.Models;

/// <summary>
/// Titulación, curso o certificación que se muestra en la página de trayectoria.
/// </summary>
/// <param name="Title">Nombre de la titulación o del curso.</param>
/// <param name="Institution">Centro o plataforma que la imparte.</param>
/// <param name="Period">Fecha o estado (Ej. <c>09/2024</c>, <c>Titulado</c>).</param>
public sealed record Credential(string Title, string Institution, string Period);
