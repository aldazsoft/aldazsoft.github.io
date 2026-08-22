---
# Dirección del sitio, sin barra final. De aquí sale la URL canónica de cada
# paquete: {siteUrl}/{route}/ — que es también el valor que su .csproj debe
# declarar en <PackageProjectUrl>.
siteUrl: https://aldazsoft.github.io

# Directorio que contiene los repositorios de los paquetes. Los 'path' de abajo
# son relativos a él.
packagesRoot: E:\Repos\Github\aldazsoft\Persiltech\Packages

# Un paquete por entrada:
#   id    -> identificador en nuget.org; es la clave contra PackageCatalog
#   route -> ruta dentro del sitio, sin barras. Por convención, el id sin el
#            prefijo de empresa (Persiltech.UserServices -> UserServices)
#   path  -> directorio del repositorio del paquete, relativo a packagesRoot
#
# El orden es el que el sitio muestra en /packages: el contrato antes que su
# adaptador, para que se lean encadenados.
packages:
  - id: Persiltech.UserServices.Abstractions
    route: UserServices.Abstractions
    path: UserServices.Abstractions

  - id: Persiltech.UserServices
    route: UserServices
    path: UserServices

  - id: Persiltech.Blazor.JSInterop
    route: Blazor.JSInterop
    path: Blazor.JSInterop

  - id: Persiltech.HttpDelegatingHandlers
    route: HttpDelegatingHandlers
    path: HttpDelegatingHandlers

  - id: Persiltech.Localizer
    route: Localizer
    path: Localizer

  - id: Persiltech.Results
    route: Results
    path: Results

  - id: Persiltech.DomainValidation
    route: DomainValidation
    path: Persiltech.DomainValidation
---

# Propósito

Declara qué paquetes documenta este sitio y dónde vive el repositorio de cada uno,
para poder reconciliar las páginas del portafolio con lo que los paquetes publican
de verdad.

El código fuente de los paquetes no es público: nuget.org solo muestra el `.nupkg`,
y este sitio es su única documentación navegable y su canal de soporte. Por eso el
enlace _Project website_ de cada paquete apunta aquí, y por eso una página que se
queda atrás respecto a su paquete es un defecto visible desde fuera.
