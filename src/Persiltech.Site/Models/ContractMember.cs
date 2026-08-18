namespace Persiltech.Site.Models;

/// <summary>
/// Miembro de la superficie pública de un tipo, tal como se documenta en el sitio.
/// </summary>
/// <param name="Signature">Firma del miembro (Ej. <c>bool IsAuthenticated { get; }</c>).</param>
/// <param name="Description">Qué expone el miembro.</param>
public sealed record ContractMember(string Signature, string Description);
