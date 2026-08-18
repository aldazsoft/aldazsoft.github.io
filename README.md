# aldazsoft.github.io

Sitio público de documentación y soporte de los paquetes NuGet de Persiltech,
publicado en <https://aldazsoft.github.io>.

El código fuente de los paquetes es privado. Este repositorio solo contiene el
sitio: la documentación de cada paquete, su canal de soporte y el enlace de
patrocinio. Es la URL que declara `<PackageProjectUrl>` en cada `.csproj`.

## Tecnología

Aplicación **Blazor WebAssembly** independiente sobre .NET 10 y C# 14, con
[MudBlazor](https://mudblazor.com) como biblioteca de componentes. Al ser
WebAssembly, el resultado publicado son archivos estáticos, que es justo lo que
GitHub Pages sabe servir.

## Estructura

    Persiltech.Site.slnx                 Solución de Visual Studio 2026
    Directory.Build.props                TFM, C# 14, nullable y estilo en compilación
    Directory.Packages.props             Versiones de los paquetes (gestión centralizada)
    src/Persiltech.Site/
      Program.cs                         Composición del host de WebAssembly
      DependencyInjection.cs             Registro de servicios del sitio y de MudBlazor
      GlobalUsings.cs                    Directivas 'using' globales
      SiteLinks.cs                       Correo de soporte, patrocinio y licencia
      Models/                            NuGetPackage, PackageRelease, ContractMember
      Services/                          IPackageCatalog y su implementación
      Theme/SiteTheme.cs                 Paletas clara y oscura de MudBlazor
      Layout/                            MainLayout, SiteAppBar, SiteFooter
      Components/                        Componentes reutilizables de documentación
      Pages/                             Portada, 404 y una página por paquete
      wwwroot/                           index.html, hoja de estilos y JS de portapapeles

## Desarrollo

    dotnet restore Persiltech.Site.slnx
    dotnet watch --project src/Persiltech.Site

## Despliegue

`.github/workflows/deploy.yml` publica en cada push a `main` mediante
`actions/deploy-pages`. En la configuración del repositorio, **Settings → Pages →
Source** debe estar en **GitHub Actions**, no en una rama.

El workflow copia `index.html` a `404.html` después de publicar: GitHub Pages
sirve `404.html` cuando la ruta no corresponde a un archivo, y así el enrutador
de Blazor puede resolver rutas como `/UserServices.Abstractions` en el cliente.

## Añadir un paquete al sitio

1. Añade su `NuGetPackage` a `Services/PackageCatalog.cs`.
2. Crea su página en `Pages/Packages/`, con la ruta que declare el
   `<PackageProjectUrl>` de su `.csproj`.
