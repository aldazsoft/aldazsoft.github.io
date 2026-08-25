namespace Persiltech.Site.Services;

/// <inheritdoc cref="IPackageCatalog" />
/// <remarks>
/// El catálogo se declara en código porque el sitio es estático: no hay servidor
/// que consultar, y el número de paquetes es pequeño y cambia con cada publicación.
/// </remarks>
public sealed class PackageCatalog : IPackageCatalog
{
    private static readonly NuGetPackage UserServicesAbstractions = new(
        Id: "Persiltech.UserServices.Abstractions",
        Route: "/UserServices.Abstractions",
        Summary: "El Output Port IUserService: estado de autenticación e identidad del usuario actual para soluciones con Arquitectura Limpia.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.1.12", "Publica la versión que la etiqueta v0.1.12 no llegó a subir."),
            new PackageRelease("0.1.11", "Apartado de licencia retirado del README; ya lo publica nuget.org."),
            new PackageRelease("0.1.10", "Historial de versiones al día en el README."),
            new PackageRelease("0.1.9", "Insignia de licencia enlazada al texto real, no a la plantilla."),
            new PackageRelease("0.1.8", "Texto de la licencia empaquetado dentro del .nupkg."),
            new PackageRelease("0.1.7", "Metadata de empaquetado adaptada a un repositorio privado."),
            new PackageRelease("0.1.6", "Enlace absoluto al texto de la licencia en el README."),
            new PackageRelease("0.1.5", "Icono del paquete y documentación al día."),
            new PackageRelease("0.1.4", "Documentación y metadata de empaquetado al día."),
            new PackageRelease("0.1.0 – 0.1.3", "Primeras publicaciones de IUserService.")
        ]);

    private static readonly NuGetPackage UserServices = new(
        Id: "Persiltech.UserServices",
        Route: "/UserServices",
        Summary: "El adaptador de ASP.NET Core para IUserService: resuelve la identidad y el estado de autenticación desde HttpContext.User.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.1.4", "La dependencia mínima de Abstractions baja de 0.1.8 a 0.1.0: instalar el paquete deja de forzar una actualización que nadie necesitaba."),
            new PackageRelease("0.1.3", "Insignia de licencia enlazada al texto real, no a la plantilla."),
            new PackageRelease("0.1.2", "Página del proyecto en el portafolio y metadata adaptada a un repositorio privado."),
            new PackageRelease("0.1.1", "Licencia publicada como archivo dentro del paquete."),
            new PackageRelease("0.1.0", "Primera publicación del adaptador HttpContextUserService.")
        ]);

    private static readonly NuGetPackage BlazorJSInterop = new(
        Id: "Persiltech.Blazor.JSInterop",
        Route: "/Blazor.JSInterop",
        Summary: "Clases base para los servicios de Blazor que cargan un módulo de JavaScript o de WebAssembly por JSInterop, con importación perezosa y liberación con el componente.",
        TargetFramework: "net10.0",
        IsPrerelease: false,
        Releases:
        [
            new PackageRelease("1.1.1", "Liberar el servicio mientras su primera llamada aún importa el módulo ya no filtra la referencia, y una llamada en vuelo no falla al salir. Una llamada que sobrevive a su servicio se registra como Debug, no como error. Los constructores documentan las excepciones que lanzan."),
            new PackageRelease("1.1.0", "Corrige el wasmModuleLoader.js empaquetado, que viajaba truncado y no se podía importar. Una importación fallida deja de cachearse: la siguiente llamada reintenta en vez de dejar el servicio muerto. DisposeAsync tolera un circuito caído. WasmLoaderServiceBase pasa a ser abstract, deriva de JSLoaderServiceBase y acepta un ILogger plano. La dependencia se estrecha a Microsoft.JSInterop y Microsoft.Extensions.Logging.Abstractions."),
            new PackageRelease("1.0.0 – 1.0.1", "Primeras publicaciones de JSLoaderServiceBase y WasmLoaderServiceBase.")
        ]);

    private static readonly NuGetPackage HttpDelegatingHandlers = new(
        Id: "Persiltech.HttpDelegatingHandlers",
        Route: "/HttpDelegatingHandlers",
        Summary: "Dos delegating handlers para HttpClient: uno convierte la respuesta de error en excepción, el otro reenvía la cultura de la aplicación Blazor en cada petición.",
        TargetFramework: "net10.0",
        IsPrerelease: false,
        Releases:
        [
            new PackageRelease("1.0.0 – 1.0.2", "Primeras publicaciones de ExceptionDelegatingHandler y LocalizationDelegatingHandler.")
        ]);

    private static readonly NuGetPackage Localizer = new(
        Id: "Persiltech.Localizer",
        Route: "/Localizer",
        Summary: "Acceso fuertemente tipado a archivos de recursos .resx, resuelto desde la cultura de la interfaz del hilo o desde la que se indique.",
        TargetFramework: "net10.0",
        IsPrerelease: false,
        Releases:
        [
            new PackageRelease("1.0.2", "El paquete pasa a su propio repositorio y solución, fuera del monorepo compartido. La página del proyecto pasa a ser esta. El texto real de la licencia viaja dentro del .nupkg en lugar de una expresión SPDX, y la superficie pública queda documentada con comentarios XML, así que IntelliSense funciona en el consumidor. Sin cambios en la API pública."),
            new PackageRelease("1.0.0 – 1.0.1", "Primeras publicaciones de LocalizationUtils y CultureScope.")
        ]);

    private static readonly NuGetPackage DomainValidation = new(
        Id: "Persiltech.DomainValidation",
        Route: "/DomainValidation",
        Summary: "Validación de reglas de negocio con el patrón Specification: reglas fluidas por propiedad, evaluación asíncrona y errores reunidos en un ValidationResult.",
        TargetFramework: "net10.0",
        IsPrerelease: false,
        Releases:
        [
            new PackageRelease("2.0.2", "Renueva el icono del paquete, que es lo único que cambia de cara al consumidor: pesa la mitad (12 401 → 6 575 bytes) con la misma resolución de 128 × 128. Sin cambios en el código ni en la superficie pública."),
            new PackageRelease("2.0.1", "Corrige el historial de versiones, que listaba una 1.0.2 y una 1.0.3 que se prepararon pero nunca llegaron a nuget.org. Sin cambios en el código ni en la superficie pública."),
            new PackageRelease("2.0.0", "La evaluación deja de guardar estado: las especificaciones devuelven sus errores en lugar de dejarlos en una propiedad, así que una instancia compartida ya no devuelve el veredicto de otra entidad. El recorrido pasa a ser asíncrono de extremo a extremo y acepta CancellationToken. Nuevas MustAsync y AsyncSpecification, sobrecargas de comparación para anulables por valor, DependencyContainer renombrado a DependencyInjection y erratas corregidas."),
            new PackageRelease("1.0.1", "Primera versión disponible en nuget.org; reemplaza a la 1.0.0, retirada del listado.")
        ]);

    private static readonly NuGetPackage Results = new(
        Id: "Persiltech.Results",
        Route: "/Results",
        Summary: "El patrón Result: una operación devuelve su éxito o su fallo como valor, con mensajes de error localizados, en lugar de lanzar excepciones para el flujo previsible.",
        TargetFramework: "net10.0",
        IsPrerelease: false,
        Releases:
        [
            new PackageRelease("1.0.1", "El paquete pasa a su propio repositorio y solución, fuera del monorepo compartido. La página del proyecto pasa a ser esta. El texto real de la licencia viaja dentro del .nupkg en lugar de una expresión SPDX, y la superficie pública queda documentada con comentarios XML, así que IntelliSense funciona en el consumidor. El README se reescribió entero: el anterior tenía tres líneas y nombraba un paquete que no existe. Sin cambios en la API pública."),
            new PackageRelease("1.0.0", "Primera publicación de Result, Result<TSuccess> y Result<TSuccess, TError>.")
        ]);

    private static readonly NuGetPackage Email = new(
        Id: "Persiltech.Email",
        Route: "/Email",
        Summary: "Envío de correo por SMTP: el contrato IEmailSender y su implementación con MailKit, con las opciones del servidor validadas al arrancar la aplicación.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.1.0", "Primera publicación de IEmailSender, EmailMessage y el envío SMTP con MailKit. Las opciones se validan al arrancar con IValidateOptions y devuelven todos los fallos juntos. Remitente y destinatario se analizan con el mismo criterio, que rechaza las direcciones sin dominio antes de abrir la conexión.")
        ]);

    private static readonly NuGetPackage MembershipEmail = new(
        Id: "Persiltech.Membership.Email",
        Route: "/Membership.Email",
        Summary: "El adaptador de correo de Persiltech.Membership: compone los avisos de la cuenta con plantillas HTML que se rebrandean por configuración, y los entrega por Persiltech.Email.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.1.0", "Primera publicación del adaptador de IMembershipEmailSender: confirmación del correo, reinicio de contraseña y cambio de correo, con plantillas HTML embebidas que se sustituyen por archivo. La marca, los colores y las rutas de la aplicación cliente son configuración, y las opciones se validan al arrancar.")
        ]);

    // El contrato va primero y su adaptador después, y una dependencia antes que quien la
    // consume: es el orden en que se leen encadenados.
    private static readonly IReadOnlyList<NuGetPackage> Packages =
        [
            UserServicesAbstractions,
            UserServices,
            BlazorJSInterop,
            HttpDelegatingHandlers,
            Localizer,
            Results,
            DomainValidation,
            Email,
            MembershipEmail
        ];

    /// <inheritdoc />
    public IReadOnlyList<NuGetPackage> GetAll() => Packages;

    /// <inheritdoc />
    public NuGetPackage? Find(string packageId) =>
        Packages.FirstOrDefault(p => p.Id == packageId);
}
