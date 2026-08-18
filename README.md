# aldazsoft.github.io

Portafolio de Edinson Aldaz y documentación de los paquetes NuGet de Persiltech,
publicado en <https://aldazsoft.github.io>.

El código fuente de los paquetes es privado. Este repositorio contiene solo el
sitio: el portafolio, la documentación de cada paquete, el canal de soporte y el
enlace de patrocinio. Es la URL que declara `<PackageProjectUrl>` en cada
`.csproj`.

## Tecnología

Aplicación **Blazor WebAssembly** independiente sobre .NET 10 y C# 14, con
[MudBlazor](https://mudblazor.com) como biblioteca de componentes. Al ser
WebAssembly, el resultado publicado son archivos estáticos, que es justo lo que
GitHub Pages sabe servir.

## Rutas

    /                            Portada del portafolio
    /packages                    Índice de paquetes
    /UserServices.Abstractions   Documentación de Persiltech.UserServices.Abstractions
    /about                       Trayectoria
    /contact                     Contacto

Las rutas de paquete **no se pueden mover**: cada versión publicada en nuget.org
conserva para siempre la URL que declaró en su `<PackageProjectUrl>`. Por eso
cada paquete vive en la raíz, con el nombre del paquete, y no bajo `/packages/`.

## Estructura

    Persiltech.Site.slnx                 Solución de Visual Studio 2026
    Directory.Build.props                TFM, C# 14, nullable y estilo en compilación
    Directory.Packages.props             Versiones de los paquetes (gestión centralizada)
    .github/scripts/build-pages.sh       Generación de una página por ruta
    src/Persiltech.Site/
      Program.cs                         Composición del host de WebAssembly
      DependencyInjection.cs             Registro de servicios del sitio y de MudBlazor
      GlobalUsings.cs                    Directivas 'using' globales
      SiteLinks.cs                       Correo de soporte, patrocinio y licencia
      Models/                            Paquetes, versiones, perfil, trayectoria
      Services/                          Catálogo de paquetes y perfil profesional
      Theme/SiteTheme.cs                 Paletas clara y oscura de MudBlazor
      Layout/                            MainLayout, barra superior, navegación y pie
      Components/                        Componentes reutilizables
      Pages/                             Portada, portafolio, 404 y páginas de paquete
      wwwroot/                           index.html, hoja de estilos y JS de portapapeles

## Contenido pendiente

Las páginas de portafolio están maquetadas con marcadores de posición. Los textos
reales viven en `Services/ProfileService.cs`, y todos los que faltan están
marcados con `PENDIENTE`:

    grep -rn "PENDIENTE" src/

El correo de soporte está en `SiteLinks.cs`.

## Desarrollo

    dotnet restore Persiltech.Site.slnx
    dotnet watch --project src/Persiltech.Site

## Despliegue

`.github/workflows/deploy.yml` publica en cada push a `main` mediante
`actions/deploy-pages`. En la configuración del repositorio, **Settings → Pages →
Source** debe estar en **GitHub Actions**, no en una rama.

Tras publicar, `build-pages.sh` genera un `index.html` por ruta. Hace falta por
dos motivos, ambos consecuencia de servir una SPA desde un hosting estático:

1. **GitHub Pages sirve archivos, no rutas.** Sin un archivo real, la petición
   cae en `404.html`: el contenido se vería, pero con estado HTTP 404.
2. **Los rastreadores de vistas previas no ejecutan JavaScript.** LinkedIn, X,
   WhatsApp o Slack leen las etiquetas del HTML servido, así que sin este paso
   todas las rutas compartirían el título y la descripción de la portada.

## Añadir un paquete al sitio

1. Añade su `NuGetPackage` a `Services/PackageCatalog.cs`.
2. Crea su página en `Pages/Packages/`, con la ruta que declare el
   `<PackageProjectUrl>` de su `.csproj`.
3. Añade esa ruta, con su título y su descripción, a `routes` en
   `.github/scripts/build-pages.sh`.
