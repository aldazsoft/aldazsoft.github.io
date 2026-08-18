namespace Persiltech.Site.Models;

/// <summary>
/// Conjunto de tecnologías agrupadas bajo un mismo rótulo.
/// </summary>
/// <param name="Title">Rótulo del grupo (Ej. <c>Plataforma</c>).</param>
/// <param name="Skills">Tecnologías que contiene, en el orden en que se muestran.</param>
public sealed record SkillGroup(string Title, IReadOnlyList<string> Skills);
