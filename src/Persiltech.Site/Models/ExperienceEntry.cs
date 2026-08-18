namespace Persiltech.Site.Models;

/// <summary>
/// Entrada de la trayectoria profesional que se muestra en la línea de tiempo.
/// </summary>
/// <param name="Period">Periodo que abarca (Ej. <c>2023 – actualidad</c>).</param>
/// <param name="Role">Puesto o rol desempeñado.</param>
/// <param name="Organization">Organización, cliente o proyecto.</param>
/// <param name="Summary">Qué se hizo, en una o dos frases.</param>
public sealed record ExperienceEntry(string Period, string Role, string Organization, string Summary);
