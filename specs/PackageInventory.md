---
# Generado por el skill audit-nuget-packages. Los conteos son del día de la auditoría.
packagesRoot: E:\Repos\Github\aldazsoft\Persiltech\Packages
siteUrl: https://aldazsoft.github.io
audited: 2026-08-19
totals:
  listo: 3
  sin-página: 0
  calidad: 0
  documentacion: 0
  metadata: 0
  estructura: 1
  no-legible: 0
---

# Inventario de homologación

Qué le falta a cada paquete para cumplir el estándar, y en qué orden conviene atacarlo.
Este archivo lo levanta una auditoría de solo lectura: ningún repositorio de paquete se
modificó al generarlo.

El estado es **la primera etapa sin terminar**, no el único problema: un paquete acumula
pendientes de varias etapas a la vez. El orden `estructura → metadata → documentacion →
calidad → sin-página` es el de trabajo, porque cada etapa se apoya en la anterior.

## Resumen

| Paquete | Estado | Versión del proyecto | Pendientes |
|---|---|---|---|
| `Persiltech.Results` | `estructura` | 1.0.0 | 22 |
| `Persiltech.DomainValidation` | `listo` | 2.0.1 | 0 |
| `Persiltech.UserServices` | `listo` | 0.1.4 | 0 |
| `Persiltech.UserServices.Abstractions` | `listo` | 0.1.11 | 0 |

Las versiones son las que declara el `.csproj`, es decir, las que se publicarán la próxima
vez. **No consta que sean las publicadas en nuget.org**: eso no se deduce del disco.

## Pendiente de publicar

Ninguno. `Persiltech.DomainValidation` publicó su `2.0.1` el 2026-08-19 y el portafolio ya la
documenta.

## Por confirmar

Preguntas que la auditoría no puede responder leyendo el disco.

Ninguna abierta. `Persiltech.Results` no declara `<RepositoryUrl>`, así que se toma como
código privado sin necesidad de preguntar, y no tiene `LICENSE` con el que su `<Copyright>`
pueda entrar en conflicto — ese conflicto aparecerá al crearlo.

### Resueltas

- **`Persiltech.DomainValidation`** — ¿es público `https://github.com/aldazsoft/DomainValidation`?
  **No** (confirmado el 2026-08-19). La metadata de repositorio se retiró del `.csproj` y
  SourceLink quedó apagado.
- **`Persiltech.DomainValidation`** — el `LICENSE` declaraba a Miguel Muñoz Serafín (2025) como
  titular, mientras el `.csproj` declaraba Copyright de Persiltech. Se alineó a
  **Persiltech (2026)** por decisión del usuario el 2026-08-19. El crédito del entrenamiento se
  conserva en el README y en la página del portafolio, que es atribución de origen, no de
  titularidad.
- **Versiones publicadas de `Persiltech.DomainValidation`** — verificado contra nuget.org el
  2026-08-19: `1.0.1`, `2.0.0` y `2.0.1` listadas, `1.0.0` deslistada. Las `1.0.2` y `1.0.3`
  se prepararon pero **nunca se publicaron**; la `2.0.1` corrigió el historial del README y el
  catálogo del sitio ya no las menciona.

## Detalle

Solo los paquetes que no están `listo`.

### Persiltech.Results — `estructura`

`Results` · ruta prevista `/Results` · 9 tipos públicos

Es un repositorio anterior al flujo de `scaffold-nuget-package` y le falta el esqueleto
entero: no tiene solución, ni `Directory.Build.props`, ni gestión centralizada de paquetes, ni
los archivos de repositorio. Sin `<TargetFramework>` **no compila**, así que la etapa de
calidad no se pudo medir todavía.

**Estructura** (10)

- No hay solución en la raíz
- Sin `Directory.Packages.props` con `ManagePackageVersionsCentrally`
- Sin `Directory.Build.props`
- Faltan `.editorconfig`, `.gitattributes`, `global.json` y `LICENSE`
- Sin manifiesto de herramientas locales (`.config`)
- Sin `specs/Package.md` ni `specs/PublicApi.md`

**Metadata** (7)

- Declara `<Version>1.0.0</Version>` en vez de `<VersionPrefix>`
- Sin `<PackageReadmeFile>`, `<PackageIcon>` ni `<Title>`
- La licencia se declara como expresión SPDX, no como archivo empaquetado
- Sin `<GenerateDocumentationFile>` ni `<IncludeSymbols>`

**Documentación** (1)

- Sin `README.md`

**Calidad** (1)

- No se pudo medir: el proyecto no compila en su estado actual
  (`NETSDK1013: El valor de TargetFramework "" no se reconoció`). Se vuelve a medir tras la
  etapa de estructura, que es la que aporta el `<TargetFramework>`.

**Sitio** (3)

- No está declarado en `specs/Packages.md`
- Sin página `/Results`
- Sin la ruta `Results` en `build-pages.sh`

> El alta en el portafolio es el **último** paso: primero el paquete cumple el estándar,
> después se documenta aquí.

## Qué mide esta auditoría, y qué no

El auditor **no comprueba el estándar por su cuenta**: recorre los repositorios e invoca sobre
cada uno `Get-HomologationPlan.ps1`, del skill global `homologate-nuget-package`, que es la
única definición. Sin ese skill instalado, la auditoría falla en vez de improvisar.

Sí mide:

- **La etapa de calidad**, compilando cada paquete con `Nullable` y `GenerateDocumentationFile`
  forzados, porque son las dos propiedades que la homologación enciende y sin ellas un
  repositorio heredado compila limpio y oculta el trabajo real. En `DomainValidation` fueron
  91 miembros públicos sin documentar y 21 avisos de nulabilidad: **a escala de la flota es el
  coste dominante**.
- **Si la compilación falla**, y entonces lo dice en lugar de reportar cero avisos. Corregido
  el 2026-08-19, tras detectar que `Persiltech.Results` salía con `0 avisos` de una compilación
  que ni siquiera arrancaba.

No mide, y nunca supone:

- **Qué versiones están publicadas en nuget.org.** El `.csproj` dice qué se publicará la
  próxima vez, no qué se publicó.
- **Si un repositorio de GitHub es público**, ni **qué titular debe llevar una licencia** cuando
  el `LICENSE` y el `<Copyright>` no coinciden. Ambas salen como preguntas al usuario.
