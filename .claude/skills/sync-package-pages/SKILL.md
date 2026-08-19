---
name: sync-package-pages
description: Reconcilia las páginas de los paquetes de este portafolio con lo que sus repositorios publican de verdad — el catálogo, la página de cada paquete y la lista de rutas del generador de páginas. Usa este skill cuando el usuario pida añadir un paquete al sitio, actualizar la documentación de un paquete tras publicar una versión, o comprobar si el portafolio se ha quedado atrás (Ej. "añade Persiltech.Results al sitio", "actualiza la página de UserServices", "¿está el portafolio al día?").
---

# Sincronizador de las páginas de paquetes

## Objetivo
Mantener las páginas de este sitio alineadas con los paquetes que documentan.

El código fuente de los paquetes no es público: nuget.org solo publica el `.nupkg`, y este
sitio es su única documentación navegable. Cada paquete declara en su `<PackageProjectUrl>`
una URL de este dominio, así que **una página ausente o desactualizada es un defecto visible
desde la ficha del paquete**, no un detalle interno.

## Cuándo usar este skill
- Añadir al sitio un paquete que aún no tiene página
- Actualizar la página de un paquete tras publicar una versión nueva
- Auditar si alguna página se ha quedado atrás respecto a su paquete

**No uses este skill** para cambiar el diseño del sitio, su portafolio personal o su
navegación: esto solo toca la documentación de paquetes.

---

## Alcance

- **Los repositorios de los paquetes son de solo lectura.** De ellos se leen el `.csproj`,
  `specs/PublicApi.md` y el `README.md`. Nunca se editan desde aquí: la versión, la metadata
  y el README de un paquete se cambian en su propio repositorio, con su propio flujo, porque
  cada cambio ahí implica publicar. Lo que no cuadre **se reporta**, no se corrige.
- **No inventa versiones publicadas.** El `<VersionPrefix>` de un `.csproj` es la versión que
  se publicará *la próxima vez*, y puede no estar todavía en nuget.org.
- **No redacta notas de versión.** Ver el _Paso 4_.
- **No hace commit ni despliega.** El despliegue lo dispara un push a `main`.

---

## Reglas generales
- El directorio de trabajo es la raíz de este repositorio.
- La especificación es `specs/Packages.md`: qué paquetes se documentan y dónde vive el
  repositorio de cada uno. Es la única lista válida; no descubras paquetes por tu cuenta.
- Las convenciones de código (C# 14, .NET 10, identificadores en inglés, `GlobalUsings.cs`,
  namespaces file-scoped) están en el `AGENTS.md` global del usuario. Aplícalas; no las
  repitas aquí.
- Si falta un dato que impida completar la tarea, detente y pregunta.

---

## Los tres archivos

Documentar un paquete en este sitio toca **exactamente tres archivos**, y los tres son
obligatorios:

| Archivo | Qué aporta |
|---|---|
| `src/Persiltech.Site/Services/PackageCatalog.cs` | La entrada `NuGetPackage`: id, ruta, resumen, TFM, si es `0.x` y el historial de versiones |
| `src/Persiltech.Site/Pages/Packages/{Nombre}.razor` | La página, con `@page "/{route}"` |
| `.github/scripts/build-pages.sh` | La ruta en `routes` |

**El tercero es el que se olvida y el que más cuesta.** GitHub Pages sirve archivos, no rutas:
sin su entrada en `routes` no se genera el `index.html`, la petición cae en `404.html` y la
página se ve —el enrutador de Blazor la resuelve en el cliente— pero **servida con HTTP 404**,
justo en la URL que el paquete declara como su sitio oficial. De esa lista salen además el
`canonical`, las etiquetas Open Graph y la entrada del `sitemap.xml`, que es la única vía por
la que un rastreador descubre la página.

El índice `/packages` y la navegación no se tocan: ambos salen del catálogo.

---

## Flujo de ejecución

### Paso 0 — Leer la especificación

Lee `specs/Packages.md`. De su frontmatter salen `siteUrl`, `packagesRoot` y la lista
`packages` con el `id`, la `route` y el `path` de cada uno.

Si el usuario nombra un paquete concreto, trabaja solo sobre él. Si no nombra ninguno,
recorre la lista entera.

### Paso 1 — Leer cada paquete en su repositorio

Para cada entrada, el repositorio está en `{packagesRoot}/{path}`. Lee de él:

| Fuente | Qué sale de ahí |
|---|---|
| `src/{id}/{id}.csproj` | `<VersionPrefix>`, `<Description>`, `<PackageProjectUrl>`, el TFM y si el código es privado |
| `specs/PublicApi.md` | La **Superficie pública**: los tipos, sus miembros y las decisiones de diseño |
| `README.md` | La prosa ya redactada del paquete, que es la base de la página |

El TFM sale de `Directory.Build.props` si el `.csproj` no lo declara.

Un paquete se considera de **código privado** si su `.csproj` no declara `<RepositoryUrl>` o
trae `<EnableSourceLink>false</EnableSourceLink>`. En ese caso la página no enlaza a
`github.com/{owner}/{repo}` en ninguna sección.

**El `README.md` del paquete es la fuente de la página, no la especificación.** El README
documenta lo que se implementó; `specs/PublicApi.md` documenta lo que se diseñó, y puede ir
por delante. Cuando se contradigan, manda el README, y dilo en el Paso 6.

### Paso 2 — Comprobación cruzada de la URL

Compara el `<PackageProjectUrl>` del paquete con `{siteUrl}/{route}/`.

Si no coinciden, **repórtalo en el Paso 6 y no lo corrijas**: el arreglo vive en el
repositorio del paquete y obliga a publicar una versión nueva, porque la metadata de
nuget.org es inmutable por versión. Sigue con la sincronización de la página igualmente: que
el paquete apunte a otro sitio no es motivo para dejar la página sin actualizar.

Si el paquete no declara `<PackageProjectUrl>`, es lo mismo: repórtalo.

### Paso 3 — La página

Si el paquete no tiene `.razor`, créala. Si la tiene, actualiza solo lo que haya cambiado y
respeta lo que el usuario haya escrito a mano.

**Toma como plantilla la página que ya exista en `Pages/Packages/`.** Ese es el contrato
visual del sitio, y una página que no se le parezca canta. Los componentes disponibles son
`PackageHeader`, `SectionHeading`, `CodeBlock`, `ContractMemberTable`, `ReleaseHistoryTable`
y `CalloutCard`; los enlaces compartidos —correo de soporte, patrocinio, licencia— están en
`SiteLinks`, y de ahí se toman: nunca se escriben literales en la página.

La página recupera su paquete del catálogo por id, así que el `PackageId` de su `@code` y el
`Id` de la entrada del catálogo tienen que coincidir **exactamente**. Si no, la página se
queda con el indicador de carga girando para siempre, y eso el compilador no lo detecta.

El código de los bloques `CodeBlock` se copia del paquete —del README o de la superficie
implementada—, no se reescribe: tiene que compilar en la solución del consumidor.

### Paso 4 — El historial de versiones

Aquí es donde este skill se detiene a propósito.

- La **versión** sale del `<VersionPrefix>` del `.csproj`.
- Las **notas** no se generan. Un "qué cambió y por qué" inventado envejece mal y nadie lo
  revisa después.

Si el `<VersionPrefix>` no está en el historial del catálogo, añade la fila con la versión y
deja las notas en `"PENDIENTE: describir el cambio."`. Enumera esas filas en el Paso 6 para
que el usuario las escriba.

Y **no des por publicada una versión solo porque esté en el `.csproj`**: puede ser la que se
publicará después. Si no sabes si llegó a nuget.org, pregunta antes de añadir la fila. Las
versiones antiguas sin nada que contar se agrupan en un rango, como ya hace el catálogo.

Ojo con `IsPrerelease`: es `true` mientras la versión sea `0.x`. Al llegar a `1.0.0` hay que
ponerlo en `false`, y entonces la página deja de mostrar el aviso de superficie inestable.

### Paso 5 — Verificación

```powershell
dotnet build Persiltech.Site.slnx
```

Cero avisos es el criterio. Después, reproduce lo que hace CI, que es lo único que demuestra
que la ruta existe de verdad:

```powershell
dotnet publish src/Persiltech.Site/Persiltech.Site.csproj --configuration Release --output ./publish-check
bash .github/scripts/build-pages.sh ./publish-check/wwwroot
```

La salida tiene que listar `{route}/index.html`. Comprueba en ese archivo el `<title>`, el
`canonical` y el `og:url`, y que la ruta aparece en `sitemap.xml`. Elimina `./publish-check`
al terminar.

Compilar **no** verifica que la página se pinte bien: el sitio es WebAssembly y se renderiza
en el navegador. Dilo en el Paso 6 y deja la comprobación visual al usuario.

### Paso 6 — Confirmación

Indica los archivos creados o modificados y, de forma explícita:

- Las filas de historial que quedaron en `PENDIENTE`
- Todo `<PackageProjectUrl>` que no coincida con `{siteUrl}/{route}/`, con las dos URLs a la
  vista y recordando que se corrige en el repositorio del paquete y solo entra en vigor al
  publicar la versión siguiente
- Los paquetes de la especificación cuyo repositorio no se pudo leer
- Que la comprobación visual queda pendiente

Si un paquete ya estaba al día, dilo, en lugar de dar a entender que se actualizó algo.

**El orden importa cuando el paquete todavía no se ha publicado.** Si la página es nueva, el
sitio tiene que estar desplegado *antes* de que se publique el paquete que apunta a ella; si
no, la ficha de nuget.org enlaza a un 404 hasta el despliegue siguiente. Recuérdalo.
