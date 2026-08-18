namespace Persiltech.Site.Models;

/// <summary>
/// Entrada del historial de versiones de un paquete.
/// </summary>
/// <param name="Version">Versión publicada, o el rango que agrupa varias (Ej. <c>0.1.0 – 0.1.3</c>).</param>
/// <param name="Notes">Qué cambió en esa versión.</param>
public sealed record PackageRelease(string Version, string Notes);
