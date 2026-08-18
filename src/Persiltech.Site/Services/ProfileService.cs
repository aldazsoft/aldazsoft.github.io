namespace Persiltech.Site.Services;

/// <inheritdoc cref="IProfileService" />
/// <remarks>
/// El perfil se declara en código por la misma razón que el catálogo de paquetes:
/// el sitio es estático y no hay servidor al que consultarlo.
/// <para>
/// El contenido procede del CV del autor. Deliberadamente no se publican el
/// teléfono ni las referencias laborales: estas últimas son datos personales de
/// terceros. Lo que quede por confirmar se marca con <c>PENDIENTE</c>.
/// </para>
/// </remarks>
public sealed class ProfileService : IProfileService
{
    private static readonly SiteProfile Profile = new(
        FullName: "Edinson Aldaz",

        Headline: "Desarrollador .NET y líder técnico, especializado en Arquitectura Limpia y sistemas empresariales de alta complejidad.",

        Introduction: "Más de 17 años analizando, diseñando y desarrollando sistemas informáticos para los sectores cementero, universitario, telecomunicaciones, salud y retail. He trabajado como Analista Programador y como Líder Técnico, liderando equipos multidisciplinarios. Publico en nuget.org los contratos y librerías que uso a diario.",

        Biography: "Soy técnico en Computación e Informática, titulado por Cibertec, con más de 17 años de experiencia en el análisis, diseño y desarrollo de sistemas. He trabajado como Analista Programador y como Líder Técnico en proyectos de alta complejidad, y esa doble perspectiva —escribir el código y decidir cómo se estructura— es la que traigo a cada proyecto. Trabajo sobre todo con .NET y C#, apoyado en Arquitectura Limpia, principios SOLID y patrones de diseño, tanto en el backend como en el frontend con Blazor WebAssembly. Los paquetes que publico en nuget.org salen de ese trabajo diario: son los contratos que repito de una solución a otra.",

        Skills:
        [
            new SkillGroup("Lenguajes", ["C#", "T-SQL", "PL/SQL", "JavaScript", "Visual Basic", "Java", "Bash"]),
            new SkillGroup("Plataforma .NET", [".NET / .NET Core", ".NET Framework", "ASP.NET MVC", "Web API", "Blazor WebAssembly", "Hosted Services"]),
            new SkillGroup("Datos", ["SQL Server", "Oracle", "Entity Framework Core", "ADO.NET", "LINQ", "SQL Developer", "Toad for Oracle"]),
            new SkillGroup("Nube y mensajería", ["Azure App Services", "Azure Functions", "Azure Service Bus", "Azure CLI", "RabbitMQ", "AWS S3", "Firebase"]),
            new SkillGroup("Arquitectura y patrones", ["Arquitectura Limpia", "Arquitectura Hexagonal", "SOLID", "Domain-Driven Design", "CQRS", "Mediator", "Repository", "Unit of Work", "Inversión de control"]),
            new SkillGroup("Herramientas y proceso", ["Visual Studio", "Git / GitFlow", "Azure DevOps", "Docker", "Postman", "Scrum", "Jira", "Bizagi"])
        ],

        Experience:
        [
            new ExperienceEntry(
                Period: "02/2026 – Actualidad",
                Role: "Desarrollador .NET",
                Organization: "Brightcell Perú",
                Summary: "Desarrollo los nuevos requerimientos de la cartera de clientes de la empresa —repartida entre Chile, México y Puerto Rico— y construyo las integraciones con sus sistemas. Trabajo sobre .NET 8 y .NET 10 con SQL Server, estructurando las soluciones con Arquitectura Hexagonal para aislar el dominio de cada integración, y llevo el ciclo de desarrollo con Git y Azure DevOps."),

            new ExperienceEntry(
                Period: "10/2024 – 01/2026",
                Role: "Desarrollador Fullstack",
                Organization: "TPA Consultores S.A.C. — Cliente: UNACEM Perú S.A.",
                Summary: "Integración de nuevas funcionalidades en Progremas, uno de los sistemas de ventas estratégicos de la empresa, y gestión de incidencias del portal y de los procesos en segundo plano. Participé en la integración de pagos entre UNACEM y Asbanc, que habilita la interoperabilidad con BCP, Interbank, Scotiabank y BBVA, y administré los repositorios Git y Azure DevOps coordinando los despliegues a Calidad y Producción."),

            new ExperienceEntry(
                Period: "05/2024 – 08/2025",
                Role: "Desarrollador Fullstack independiente",
                Organization: "Freelance",
                Summary: "Implementé un e-commerce PWA para la venta en línea de libros de la UNFV: backend en .NET 8, frontend en Blazor WebAssembly y persistencia en Azure SQL Server con Entity Framework Core. Añadí procesos en segundo plano con Hosted Services, RabbitMQ como broker de mensajería, AWS S3 para el almacenamiento de documentos y BoXtream DRM para la protección de los libros electrónicos."),

            new ExperienceEntry(
                Period: "09/2024 – 01/2025",
                Role: "Desarrollador Fullstack",
                Organization: "Digitalia — Cliente: Conecta Market Place S.A.C.",
                Summary: "Integración de un marketplace con VTEX, Magento y Shopify. Estructuré las soluciones en .NET 7 y .NET 8 siguiendo Arquitectura Limpia, con frontend en Blazor WebAssembly, Azure Functions para los procesos serverless y Azure Service Bus para la comunicación asíncrona entre servicios."),

            new ExperienceEntry(
                Period: "08/2020 – 05/2024",
                Role: "Consultor en Tecnologías de Información · Líder Técnico",
                Organization: "TPA Consultores S.A.C. — Cliente: UNACEM Perú S.A.",
                Summary: "Lideré el desarrollo del portal Progre+, usado por los socios Progresol para gestionar y seguir sus pedidos. Desarrollé Web APIs, frontend en ASP.NET MVC y Blazor, y aplicaciones de background —Servicios Windows y Hosted Services— para el procesamiento de pagos y el envío de notificaciones. Fui promovido a Líder Técnico, y el portal acabó integrado con los distribuidores La Viga, Macisa, Cemensa y ABerio, y con Asbanc."),

            new ExperienceEntry(
                Period: "05/2020 – 08/2020",
                Role: "Consultor en Tecnologías de Información",
                Organization: "Métrica Andina — Cliente: UPC",
                Summary: "Nuevos módulos de la aplicación web que da acceso a los formularios de alumnos de Pregrado, EPE y Postgrado, orientados a facilitar el acceso a los beneficios económicos y tecnológicos durante la crisis sanitaria de la Covid-19."),

            new ExperienceEntry(
                Period: "03/2019 – 05/2020",
                Role: "Consultor en Tecnologías de Información",
                Organization: "Métrica Andina — Cliente: UPN",
                Summary: "Aplicaciones y servicios web del portal de docentes: apertura de cursos, emisión de certificados y análisis de informes finales del sistema de capacitación, además del soporte y la formación al personal administrativo y docente."),

            new ExperienceEntry(
                Period: "07/2017 – 03/2019",
                Role: "Consultor en Tecnologías de Información",
                Organization: "Métrica Andina — Cliente: UPC",
                Summary: "Nuevos módulos del portal de trámites en línea, y desarrollo del portal de inglés de la universidad cubriendo el flujo completo: captación, matrícula, control de asistencia, registro de notas e informes."),

            new ExperienceEntry(
                Period: "01/2014 – 07/2017",
                Role: "Analista Programador",
                Organization: "Teamsoft — Cliente: América Móvil",
                Summary: "Aplicaciones para el área de seguridad de TI del cliente, con módulos de gestión administrativa y operacional. Construí procesos críticos de carga y procesamiento masivo con herramientas Unix/Linux, Bash y Oracle."),

            new ExperienceEntry(
                Period: "08/2013 – 12/2013",
                Role: "Analista Programador",
                Organization: "Alephsystem — Cliente: Metro de Lima",
                Summary: "Aplicación web para la gestión del mantenimiento y la supervisión de las estaciones del Metro de Lima, sobre una arquitectura de N capas con C#, ASP.NET WebForms, JavaScript, HTML5/CSS3 y SQL Server."),

            new ExperienceEntry(
                Period: "07/2012 – 07/2013",
                Role: "Analista Programador",
                Organization: "Infoparque Perú — Sector salud",
                Summary: "ERP a medida para empresas del sector salud, participando en todas las fases del proyecto —de la concepción a la puesta en producción— sobre una arquitectura en capas."),

            new ExperienceEntry(
                Period: "01/2012 – 06/2012",
                Role: "Analista",
                Organization: "Tgestiona — Sector telecomunicaciones",
                Summary: "Consultas SQL en Access y SQL Server para los informes e indicadores estadísticos de las ventas diarias del área.")
        ],

        Education:
        [
            new Credential("Computación e Informática", "Cibertec", "Titulado"),
            new Credential("Inglés americano", "ICPNA", "En curso")
        ],

        Training:
        [
            new Credential("Control de versiones con Git", "TI Capacitación", "06/2026"),
            new Credential("Gestión de proyectos con Jira y Scrum", "Udemy", "09/2024"),
            new Credential("Blazor WebAssembly con .NET 8", "TI Capacitación", "05/2024"),
            new Credential("Introducción a la inyección de dependencias", "TI Capacitación", "03/2024"),
            new Credential("Azure para desarrolladores", "TI Capacitación", "08/2022"),
            new Credential("Fundamentos de Azure", "TI Capacitación", "05/2022"),
            new Credential("Introducción a Clean Architecture", "TI Capacitación", "08/2021"),
            new Credential("Programación asíncrona con C#", "TI Capacitación", "07/2021"),
            new Credential("Iniciando con Docker", "TI Capacitación", "07/2021"),
            new Credential("Introducción a OAuth 2 y OIDC con ASP.NET Core", "TI Capacitación", "03/2021"),
            new Credential("Blazor WebAssembly con .NET 5", "TI Capacitación", "03/2020"),
            new Credential("JavaScript ES6", "Udemy", "02/2020"),
            new Credential("Introducción a Entity Framework Core 3.1", "Udemy", "12/2019")
        ],

        ContactMethods:
        [
            new ContactMethod(
                Icon: Icons.Material.Outlined.Mail,
                Label: "Correo",
                Value: SiteLinks.SupportEmail,
                Url: SiteLinks.SupportMailTo),
            new ContactMethod(
                Icon: Icons.Custom.Brands.LinkedIn,
                Label: "LinkedIn",
                Value: "edinson-aldaz",
                Url: "https://www.linkedin.com/in/edinson-aldaz/"),
            new ContactMethod(
                Icon: Icons.Custom.Brands.GitHub,
                Label: "GitHub",
                Value: "aldazsoft",
                Url: "https://github.com/aldazsoft"),
            new ContactMethod(
                Icon: Icons.Material.Outlined.Favorite,
                Label: "GitHub Sponsors",
                Value: "aldazsoft",
                Url: SiteLinks.SponsorUrl)
        ]);

    /// <inheritdoc />
    public SiteProfile GetProfile() => Profile;
}
