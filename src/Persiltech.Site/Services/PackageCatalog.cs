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
            new PackageRelease("0.1.15", "Restaura en el README el historial de versiones, el soporte y el apoyo al desarrollo, que se perdieron al migrar el paquete al monorepo. El soporte pasa a las incidencias de GitHub, ahora que el repositorio es público. Sin cambios en el código ni en la superficie pública."),
            new PackageRelease("0.1.14", "Estrena las notas de versión, que el paquete no declaraba. Anunciaba la restauración del README, que por un commit incompleto no llegó a viajar en el paquete: llega en 0.1.15. Sin cambios en el código ni en la superficie pública."),
            new PackageRelease("0.1.13", "El .nuspec declara el repositorio, ahora público, y activa SourceLink: el depurador del consumidor puede entrar al código fuente. Sin cambios en el código ni en la superficie pública."),
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
            new PackageRelease("0.1.6", "El .nuspec declara el repositorio, ahora público, y activa SourceLink: el depurador del consumidor puede entrar al código fuente. El paquete estrena sus notas de versión, el soporte pasa a las incidencias de GitHub y el suelo de Abstractions sube de 0.1.12 a 0.1.15. Sin cambios en la superficie pública."),
            new PackageRelease("0.1.5", "La dependencia de Abstractions sube de 0.1.0 a 0.1.12: dentro del monorepo el contrato lo aporta el proyecto vecino, y dotnet pack declara la versión que este tiene al empaquetar. Sin cambios en la superficie pública."),
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
            new PackageRelease("1.1.2", "El .nuspec declara el repositorio, ahora público, y activa SourceLink: el depurador del consumidor puede entrar al código fuente. El soporte pasa a las incidencias de GitHub. Microsoft.JSInterop y Microsoft.Extensions.Logging.Abstractions suben de 10.0.9 a 10.0.11. Sin cambios en el código ni en la superficie pública."),
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
            new PackageRelease("1.0.3", "El .nuspec declara el repositorio, ahora público, y activa SourceLink: el depurador del consumidor puede entrar al código fuente. El README enlaza al monorepo y el soporte pasa a las incidencias de GitHub. Microsoft.Extensions.Localization sube de 10.0.9 a 10.0.11. Sin cambios en el código ni en la superficie pública."),
            new PackageRelease("1.0.2", "La página del proyecto pasa a ser esta. El texto real de la licencia viaja dentro del .nupkg en lugar de una expresión SPDX, y la superficie pública queda documentada con comentarios XML, así que IntelliSense funciona en el consumidor. Sin cambios en la API pública."),
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
            new PackageRelease("2.0.3", "El .nuspec declara el repositorio, ahora público, y activa SourceLink: el depurador del consumidor puede entrar al código fuente. El README enlaza al monorepo y el soporte pasa a las incidencias de GitHub. El suelo de Persiltech.Localizer sube de 1.0.1 a 1.0.3. Sin cambios en el código ni en la superficie pública."),
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
            new PackageRelease("1.0.2", "El .nuspec declara el repositorio, ahora público, y activa SourceLink: el depurador del consumidor puede entrar al código fuente. El README enlaza al monorepo y el soporte pasa a las incidencias de GitHub. El suelo de Persiltech.Localizer sube de 1.0.1 a 1.0.3. Sin cambios en el código ni en la superficie pública."),
            new PackageRelease("1.0.1", "La página del proyecto pasa a ser esta. El texto real de la licencia viaja dentro del .nupkg en lugar de una expresión SPDX, y la superficie pública queda documentada con comentarios XML, así que IntelliSense funciona en el consumidor. El README se reescribió entero: el anterior tenía tres líneas y nombraba un paquete que no existe. Sin cambios en la API pública."),
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
            new PackageRelease("0.1.1", "El .nuspec declara el repositorio, ahora público, y activa SourceLink: el depurador del consumidor puede entrar al código fuente. El README enlaza al monorepo y el soporte pasa a las incidencias de GitHub. Sin cambios en el código ni en la superficie pública."),
            new PackageRelease("0.1.0", "Primera publicación de IEmailSender, EmailMessage y el envío SMTP con MailKit. Las opciones se validan al arrancar con IValidateOptions y devuelven todos los fallos juntos. Remitente y destinatario se analizan con el mismo criterio, que rechaza las direcciones sin dominio antes de abrir la conexión.")
        ]);

    private static readonly NuGetPackage Membership = new(
        Id: "Persiltech.Membership",
        Route: "/Membership",
        Summary: "Sistema de membresía para ASP.NET Core: registro y autenticación sobre ASP.NET Core Identity, endpoints de Minimal API que montas donde quieras y emisión de un JSON Web Token firmado con HMAC-SHA256.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.8.0", "IAccessTokenClaimsProvider deja que el consumidor añada sus propias reclamaciones al token de acceso: el paquete no sabe qué es un inquilino y no debe saberlo, así que lo aporta quien sí lo sabe. No se pueden sobrescribir Name, Role ni Fullname —sin ese corte, registrar un proveedor sería una vía para concederse cualquier rol— y dos proveedores que aporten la misma reclamación fallan, porque cuál ganara dependería del orden de registro. El emisor pasa de único a con ámbito, ya que las aportaciones suelen salir de una consulta."),
            new PackageRelease("0.7.0", "Las clases de opciones pasan a ser planas y cada una estrena su validador IValidateOptions al lado: JwtOptions y MembershipApiOptions dejan de depender de anotaciones de datos, que cortaban en el primer fallo. El validador reúne todos los fallos y nombra la ruta completa de la clave que falta."),
            new PackageRelease("0.6.1", "Solo documentación. La 0.6.0 se publicó sin la sección que explica cómo elegir el esquema de las tablas, que se escribió después; el README es lo único que un consumidor ve de ese cambio, y sin subir la versión no le llega. El código no cambia."),
            new PackageRelease("0.6.0", "Sesión renovable y usuario extensible. SessionEndpoints monta user/refresh y user/logout, ambos anónimos: el testigo de renovación es la credencial, y exigir además un token vigente haría imposible renovar justo cuando hace falta. El testigo son 32 bytes aleatorios y en la base solo vive su SHA-256, así que leer la tabla no entrega sesiones utilizables. Cada renovación lo consume y emite otro de la misma familia; presentar uno ya consumido revoca la familia entera, como recomienda la OAuth 2.0 Security BCP."),
            new PackageRelease("0.5.0", "Primera versión en nuget.org. Registro, autenticación y emisión de JWT sobre ASP.NET Core Identity, con los endpoints de cuenta, roles, usuarios, contraseña, correo, teléfono, perfil y doble factor. Los avisos por correo y SMS salen por puertos que implementa el consumidor. Las versiones 0.1.0 a 0.4.0 fueron internas.")
        ]);

    private static readonly NuGetPackage MembershipOAuth = new(
        Id: "Persiltech.Membership.OAuth",
        Route: "/Membership.OAuth",
        Summary: "Servidor de autorización OAuth 2.0 y OpenID Connect sobre OpenIddict para Persiltech.Membership: Authorization Code con PKCE, credenciales de cliente y renovación por refresh token.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.3.0", "MembershipOAuthOptions pasa a ser una clase plana con su validador MembershipOAuthOptionsValidator al lado, que reúne todos los fallos de configuración en lugar de cortar en el primero y nombra la ruta completa de cada clave."),
            new PackageRelease("0.2.0", "Primera versión en nuget.org. Servidor de autorización sobre OpenIddict con Authorization Code + PKCE, credenciales de cliente y refresh token, emitiendo para las mismas cuentas de ASP.NET Core Identity que administra el paquete base. El registro de clientes es idempotente. La versión 0.1.0 fue interna.")
        ]);

    private static readonly NuGetPackage MembershipEmail = new(
        Id: "Persiltech.Membership.Email",
        Route: "/Membership.Email",
        Summary: "El adaptador de correo de Persiltech.Membership: compone los avisos de la cuenta con plantillas HTML que se rebrandean por configuración, y los entrega por Persiltech.Email.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.2.0", "ClientBaseUrls declara una dirección de vuelta por portal, y la cabecera clientId —que los frontales ya enviaban sin que nadie la leyera— dice cuál usar. Con una sola dirección, quien pedía su contraseña desde el portal de clientes recibía un enlace hacia el administrativo. La clave llega en una cabecera que cualquiera puede escribir, así que solo elige entre lo configurado y una desconocida cae en ClientBaseUrl: construir el enlace con una dirección venida de la petición sería enviar phishing con un testigo válido dentro."),
            new PackageRelease("0.1.0", "Primera publicación del adaptador de IMembershipEmailSender: confirmación del correo, reinicio de contraseña y cambio de correo, con plantillas HTML embebidas que se sustituyen por archivo. La marca, los colores y las rutas de la aplicación cliente son configuración, y las opciones se validan al arrancar.")
        ]);

    private static readonly NuGetPackage MembershipBlazor = new(
        Id: "Persiltech.Membership.Blazor",
        Route: "/Membership.Blazor",
        Summary: "Cliente Blazor de Persiltech.Membership: el estado de autenticación a partir del JWT que emite la API, la renovación automática de la sesión, el manejador que firma cada petición y los formularios de MudBlazor de sus pantallas.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("2.0.0-preview.4", "Los cuatro formularios pasan a EditForm y cada error de la API se pinta bajo el campo que lo provocó, con Persiltech.Validation.Blazor —la única dependencia nueva—. Desaparece MembershipValidationErrors, el cartel que reunía los errores de todos los campos: lo que no es de ningún campo sigue saliendo arriba, y el resto va a su sitio. El aviso de que las dos contraseñas no coinciden sale ahora bajo su campo. Los campos obligatorios y el formato del correo se comprueban ya en el navegador, y los botones envían el formulario, así que la tecla Intro funciona."),
            new PackageRelease("2.0.0-preview.3", "El formulario de reinicio deja de pintar el testigo en un campo editable: es una credencial, y enseñarlo solo consigue que acabe copiado en un chat de soporte o capturado en una pantalla. Se lee del enlace y se queda en memoria, y la cadena de consulta se limpia de la barra de direcciones en cuanto se lee —ClearQueryString lo desactiva—. El correo pasa a solo lectura, pero solo si vino en el enlace. Se añade la confirmación de contraseña, que el servidor no puede validar porque recibe una sola."),
            new PackageRelease("2.0.0-preview.2", "MembershipApiOptions pasa a ser una clase plana, sin anotaciones de datos. La comprobación la hace MembershipApiOptionsValidator, que AddMembershipBlazor invoca al registrar —en WebAssembly no hay host que arranque servicios, así que ValidateOnStart no correría nunca—, y que ahora exige además que BaseAddress sea una URL http o https: en Unix una ruta como /api parsea como URI absoluta y se colaba."),
            new PackageRelease("2.0.0-preview.1", "Reescritura completa. Cliente de Persiltech.Membership 0.6.0: estado de autenticación con renovación, almacén de testigos sustituible, manejador que firma cada petición y los formularios de sesión, registro y contraseña."),
            new PackageRelease("1.0.0 – 1.0.1", "Versiones del monorepo anterior, con otra API y con las pantallas de empleados y clientes.")
        ]);

    private static readonly NuGetPackage ValidationBlazor = new(
        Id: "Persiltech.Validation.Blazor",
        Route: "/Validation.Blazor",
        Summary: "Lleva los errores de validación que devuelve una API al campo que los provocó: los escribe en el EditContext de Blazor para que el componente de entrada los muestre como si fueran suyos.",
        TargetFramework: "net10.0",
        IsPrerelease: true,
        Releases:
        [
            new PackageRelease("0.1.0", "Primera versión. ApiValidator escribe en el EditContext los errores que devuelve la API, emparejando la clave de cada uno con la propiedad del modelo —incluidas las rutas anidadas y las indexadas— y dejando en Unmatched lo que no encuentra dueño, en lugar de descartarlo.")
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
            ValidationBlazor,
            Email,
            Membership,
            MembershipOAuth,
            MembershipEmail,
            MembershipBlazor
        ];

    /// <inheritdoc />
    public IReadOnlyList<NuGetPackage> GetAll() => Packages;

    /// <inheritdoc />
    public NuGetPackage? Find(string packageId) =>
        Packages.FirstOrDefault(p => p.Id == packageId);
}
