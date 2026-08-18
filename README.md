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

GitHub Pages sirve archivos, no rutas, y una ruta con punto —como
`/UserServices.Abstractions`— ningún servidor estático la trata como ruta de SPA:
la resuelve como nombre de archivo. Por eso el workflow hace dos cosas tras
publicar:

1. Copia `index.html` dentro de la carpeta de cada ruta de paquete
   (`UserServices.Abstractions/index.html`), para que responda **200**. Es la URL
   que cada paquete declara en su `<PackageProjectUrl>`, así que importa que no
   sea un 404.
2. Copia `index.html` a `404.html` como red de seguridad del resto de rutas.

Al añadir un paquete hay que añadir su ruta a la lista de ese paso.


## Añadir un paquete al sitio

1. Añade su `NuGetPackage` a `Services/PackageCatalog.cs`.
2. Crea su página en `Pages/Packages/`, con la ruta que declare el
   `<PackageProjectUrl>` de su `.csproj`.
3. Añade esa ruta a la lista del paso *Publicar las rutas de paquete como
   archivos* de `.github/workflows/deploy.yml`.
