---
# Dirección del sitio, sin barra final. De aquí sale la URL canónica de cada
# paquete: {siteUrl}/{route}/ — que es también el valor que su .csproj debe
# declarar en <PackageProjectUrl>.
siteUrl: https://aldazsoft.github.io

# El monorepo que publica los paquetes de la casa. Un solo repositorio, un .slnx,
# y un proyecto empaquetable por paquete bajo src/. Los 'project' de abajo son
# relativos a él.
monorepoRoot: E:\Repos\Github\aldazsoft\Persiltech\Persiltech.Packages

# Repositorios de un solo paquete que todavía no viven en el monorepo. Los 'path'
# de las entradas que lo usen son relativos a este directorio. Vacía la clave
# cuando no quede ninguno.
legacyPackagesRoot: E:\Repos\Github\aldazsoft\Persiltech\Packages

# Un paquete por entrada:
#   id      -> identificador en nuget.org; es la clave contra PackageCatalog
#   route   -> ruta dentro del sitio, sin barras. Por convención, el id sin el
#              prefijo de empresa (Persiltech.UserServices -> UserServices)
#   project -> directorio del proyecto dentro del monorepo, relativo a
#              monorepoRoot. Por convención, src/{id}
#   path    -> SOLO para los que aún viven en su propio repositorio: directorio
#              relativo a legacyPackagesRoot. Excluyente con 'project'
#
# De 'project' salen las tres rutas que el sitio necesita leer:
#   {monorepoRoot}/{project}/{id}.csproj   -> versión, descripción, TFM, URLs
#   {monorepoRoot}/{project}/README.md     -> la prosa del paquete publicado
#   {monorepoRoot}/specs/{id}/PublicApi.md -> la superficie pública
#
# El orden es el que el sitio muestra en /packages: el contrato antes que su
# adaptador, para que se lean encadenados.
packages:
  - id: Persiltech.UserServices.Abstractions
    route: UserServices.Abstractions
    project: src/Persiltech.UserServices.Abstractions

  - id: Persiltech.UserServices
    route: UserServices
    project: src/Persiltech.UserServices

  - id: Persiltech.Blazor.JSInterop
    route: Blazor.JSInterop
    project: src/Persiltech.Blazor.JSInterop

  - id: Persiltech.HttpDelegatingHandlers
    route: HttpDelegatingHandlers
    path: HttpDelegatingHandlers

  - id: Persiltech.Localizer
    route: Localizer
    project: src/Persiltech.Localizer

  - id: Persiltech.Results
    route: Results
    project: src/Persiltech.Results

  - id: Persiltech.DomainValidation
    route: DomainValidation
    project: src/Persiltech.DomainValidation

  - id: Persiltech.Email
    route: Email
    project: src/Persiltech.Email

  - id: Persiltech.Membership
    route: Membership
    project: src/Persiltech.Membership

  - id: Persiltech.Membership.OAuth
    route: Membership.OAuth
    project: src/Persiltech.Membership.OAuth

  - id: Persiltech.Membership.Email
    route: Membership.Email
    project: src/Persiltech.Membership.Email
---

# Propósito

Declara qué paquetes documenta este sitio y dónde vive el código de cada uno, para
poder reconciliar las páginas del portafolio con lo que los paquetes publican de verdad.

El enlace _Project website_ de cada paquete apunta aquí, así que este sitio es la portada
del paquete en nuget.org y su canal de soporte: una página que se queda atrás respecto a
su paquete es un defecto visible desde fuera.

## Dónde vive el código

Los paquetes se publican desde un **monorepo**, `Persiltech.Packages`: un `.slnx`, un
proyecto empaquetable por paquete bajo `src/`, y `tests/`, `samples/` y `specs/` compartidos.
Antes cada paquete tenía su propio repositorio; esa etapa terminó, y lo que queda de ella son
las entradas con `path` en lugar de `project`.

Por eso hay dos claves de raíz y no una:

| Clave | Para qué |
|---|---|
| `monorepoRoot` | Los paquetes que ya viven en el monorepo. Declaran `project` |
| `legacyPackagesRoot` | Los que todavía tienen repositorio propio. Declaran `path` |

**Una entrada declara `project` o `path`, nunca las dos.** Es lo que le dice a cualquier
herramienta dónde buscar el `.csproj`, el `README.md` y la especificación, que en las dos
disposiciones están en sitios distintos:

| | Monorepo (`project`) | Repositorio propio (`path`) |
|---|---|---|
| `.csproj` | `{project}/{id}.csproj` | `src/{id}/{id}.csproj` |
| `README.md` | `{project}/README.md` | en la raíz del repositorio |
| `PublicApi.md` | `specs/{id}/PublicApi.md` | `specs/PublicApi.md` |

> **`Persiltech.HttpDelegatingHandlers` es hoy el único que queda fuera.** Está publicado en
> nuget.org y tiene su página en este sitio, pero su código no se ha trasladado al monorepo.
> Cuando se traslade, su entrada cambia `path` por `project` y `legacyPackagesRoot` se puede
> vaciar.

## El código fuente es público

Los repositorios pasan a ser públicos —la monetización va por los términos de la licencia, no
por ocultar el código—, y eso no le resta trabajo a este sitio: el repositorio documenta *el
código*, esta página documenta *el paquete publicado*. Son dos documentos con lectores
distintos, no uno duplicado.

Mientras dure la transición **manda el `.csproj` de cada paquete**: uno que no declare
`<RepositoryUrl>`, o que traiga `<EnableSourceLink>false</EnableSourceLink>`, se sigue
documentando sin enlaces a GitHub. Hoy los del monorepo siguen así salvo
`Persiltech.UserServices.Abstractions`, que además apunta a su repositorio anterior; corregirlo
cuesta una publicación por paquete, así que se hace cuando cada uno vaya a publicar.
