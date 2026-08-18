namespace Persiltech.Site.Services;

/// <inheritdoc cref="IProfileService" />
/// <remarks>
/// El perfil se declara en código por la misma razón que el catálogo de paquetes:
/// el sitio es estático y no hay servidor al que consultarlo.
/// <para>
/// Los textos marcados con <c>PENDIENTE</c> son marcadores de posición: hay que
/// sustituirlos antes de publicar. <c>grep -rn "PENDIENTE" src/</c> los enumera todos.
/// </para>
/// </remarks>
public sealed class ProfileService : IProfileService
{
    private static readonly SiteProfile Profile = new(
        FullName: "Edinson Aldaz",
        Headline: "PENDIENTE: titular de una línea (Ej. «Desarrollador .NET especializado en Arquitectura Limpia»)",
        Introduction: "PENDIENTE: dos o tres frases para la portada. Qué problemas resuelves y para quién. Evita fechas concretas: envejecen mal.",
        Biography: "PENDIENTE: presentación extendida para la página de trayectoria. Cómo trabajas, qué tipo de proyectos te interesan y qué te diferencia.",
        Skills:
        [
            new SkillGroup("Plataforma", ["PENDIENTE: .NET 10", "PENDIENTE: C# 14", "PENDIENTE: ASP.NET Core"]),
            new SkillGroup("Arquitectura", ["PENDIENTE: Arquitectura Limpia", "PENDIENTE: Puertos y adaptadores"]),
            new SkillGroup("Datos", ["PENDIENTE: Entity Framework Core", "PENDIENTE: SQL Server"]),
            new SkillGroup("Herramientas", ["PENDIENTE: Git", "PENDIENTE: GitHub Actions", "PENDIENTE: Visual Studio"])
        ],
        Experience:
        [
            new ExperienceEntry(
                Period: "PENDIENTE: 20XX – actualidad",
                Role: "PENDIENTE: puesto",
                Organization: "PENDIENTE: organización o proyecto",
                Summary: "PENDIENTE: qué construiste y con qué impacto. Una o dos frases."),
            new ExperienceEntry(
                Period: "PENDIENTE: 20XX – 20XX",
                Role: "PENDIENTE: puesto",
                Organization: "PENDIENTE: organización o proyecto",
                Summary: "PENDIENTE: qué construiste y con qué impacto. Una o dos frases.")
        ],
        ContactMethods:
        [
            new ContactMethod(
                Icon: Icons.Material.Outlined.Mail,
                Label: "Correo",
                Value: SiteLinks.SupportEmail,
                Url: SiteLinks.SupportMailTo),
            new ContactMethod(
                Icon: Icons.Custom.Brands.GitHub,
                Label: "GitHub",
                Value: "aldazsoft",
                Url: "https://github.com/aldazsoft"),
            new ContactMethod(
                Icon: Icons.Custom.Brands.LinkedIn,
                Label: "LinkedIn",
                Value: "PENDIENTE: usuario de LinkedIn",
                Url: "PENDIENTE-URL-LINKEDIN"),
            new ContactMethod(
                Icon: Icons.Material.Outlined.Favorite,
                Label: "GitHub Sponsors",
                Value: "aldazsoft",
                Url: SiteLinks.SponsorUrl)
        ]);

    /// <inheritdoc />
    public SiteProfile GetProfile() => Profile;
}
