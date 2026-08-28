---
# Generado por el skill audit-nuget-packages. Los conteos son del día de la auditoría.
monorepoRoot: E:\Repos\Github\aldazsoft\Persiltech\Persiltech.Packages
legacyPackagesRoot: E:\Repos\Github\aldazsoft\Persiltech\Packages
siteUrl: https://aldazsoft.github.io
audited: 2026-08-27
totals:
  listo: 2
  sin-página: 0
  calidad: 0
  documentacion: 0
  metadata: 0
  estructura: 9
  no-legible: 0
---

# Inventario de homologación

Qué le falta a cada paquete para cumplir el estándar, y en qué orden conviene atacarlo.
Este archivo lo levanta una auditoría de solo lectura: ningún repositorio se modificó al
generarlo.

El estado es **la primera etapa sin terminar**, no el único problema: un paquete acumula
pendientes de varias etapas a la vez. El orden `estructura → metadata → documentacion →
calidad → sin-página` es el de trabajo, porque cada etapa se apoya en la anterior.

**Los pendientes del repositorio no cuentan para el estado de ningún paquete.** Salen aparte,
porque se arreglan una vez y valen para los diez.

## Pendientes del repositorio

### `Persiltech.Packages` — monorepo, 10 paquetes

| # | Pendiente | Por qué importa |
| - | --------- | --------------- |
| 1 | **Sin workflows en `.github/workflows/`** | El directorio existe pero está vacío: no hay CI que compile ni `publish.yml` que publique. Es lo que bloquea el paso 8 de cualquier paquete |
| 2 | `README.md` no está anclado en `Solution Items` del `.slnx` | Y tampoco existe: el monorepo no tiene portada. Los README que se empaquetan sí están, uno por paquete en su `src/` |
| 3 | Al `.gitattributes` le falta la regla `**/Migrations/**` | La fija la plantilla compartida. Sin ella, las migraciones generadas se guardan con otro fin de línea que el resto de la flota |

> **El `publish.yml` de un monorepo no es el de un repositorio de un paquete.** El del estándar
> se dispara con `v*` y empaqueta la solución; aquí eso publicaría los diez paquetes a la vez.
> La etiqueta tiene que nombrar el paquete —`Persiltech.Results-v1.0.1`— y el workflow deducir
> de ella qué `.csproj` empaquetar, comprobando que coincide con su `<VersionPrefix>`.

Además, y fuera de lo que la auditoría mide: **el repositorio no tiene ni un commit ni remoto**.
Está `git init` y nada más.

### `HttpDelegatingHandlers` — repositorio propio, 1 paquete

| # | Pendiente |
| - | --------- |
| 1 | Al `.gitattributes` le falta la regla `**/Migrations/**` |

## Resumen

| Paquete                                | Repositorio           | Estado        | Versión del proyecto | Pendientes |
| -------------------------------------- | --------------------- | ------------- | -------------------- | ---------- |
| `Persiltech.Blazor.JSInterop`          | `Persiltech.Packages` | `estructura`  | 1.1.1                | 2          |
| `Persiltech.Email`                     | `Persiltech.Packages` | `estructura`  | 0.1.0                | 2          |
| `Persiltech.Membership.Email`          | `Persiltech.Packages` | `estructura`  | 0.1.0                | 2          |
| `Persiltech.Membership.OAuth`          | `Persiltech.Packages` | `estructura`  | 0.2.0                | 2          |
| `Persiltech.Localizer`                 | `Persiltech.Packages` | `estructura`  | 1.0.2                | 1          |
| `Persiltech.Membership`                | `Persiltech.Packages` | `estructura`  | 0.5.0                | 1          |
| `Persiltech.Results`                   | `Persiltech.Packages` | `estructura`  | 1.0.1                | 1          |
| `Persiltech.UserServices`              | `Persiltech.Packages` | `estructura`  | 0.1.4                | 1          |
| `Persiltech.UserServices.Abstractions` | `Persiltech.Packages` | `estructura`  | 0.1.12               | 1          |
| `Persiltech.DomainValidation`          | `Persiltech.Packages` | `listo`       | 2.0.2                | 0          |
| `Persiltech.HttpDelegatingHandlers`    | `HttpDelegatingHandlers` | `listo`    | 1.0.3                | 0          |

Las versiones son las que declara el `.csproj`, no las publicadas. Ver _Pendiente de publicar_.

**La solución compila limpia: 0 avisos y 0 errores en los 23 proyectos.** Las etapas de
`metadata`, `documentacion` y `calidad` están cerradas en toda la flota salvo el historial del
README de `Blazor.JSInterop`. Lo único que queda a nivel de paquete es la estructura de `specs/`.

## Fuera de esta auditoría

- **Nueve repositorios de `legacyPackagesRoot`** —`Blazor.JSInterop`, `Email`, `Localizer`,
  `Membership`, `Membership.Email`, `Persiltech.DomainValidation`, `Results`, `UserServices`,
  `UserServices.Abstractions`— son **copias que quedaron atrás**: su paquete ya vive en el
  monorepo. El auditor los salta a propósito, porque auditarlos daría por pendiente en la copia
  vieja lo que ya se arregló en el destino. Conviene borrarlos: mientras estén, alguien puede
  editar el archivo equivocado.

La flota que este inventario cubre es la de `specs/Packages.md`, y no hay más: el catálogo de
paquetes de la casa es el que `Persiltech.Packages` publica.

## Pendiente de publicar

| Paquete                             | En el `.csproj` | En nuget.org | Qué sigue mostrando la ficha                                                |
| ----------------------------------- | --------------- | ------------ | --------------------------------------------------------------------------- |
| `Persiltech.HttpDelegatingHandlers` | 1.0.3           | 1.0.2        | El README anterior a la homologación, y sin `PackageProjectUrl` a su página |
| `Persiltech.Membership`             | 0.5.0           | —            | No está publicado. Su página ya avisa de *Próximamente en NuGet*            |
| `Persiltech.Membership.OAuth`       | 0.2.0           | —            | No está publicado. Su página ya avisa de *Próximamente en NuGet*            |
| `Persiltech.Membership.Email`       | 0.1.0           | —            | No está publicado. Su página ya avisa de *Próximamente en NuGet*            |

Los cuatro tienen página desplegada, así que la URL que declaran ya resuelve y pueden publicarse
cuando se quiera —en cuanto el monorepo tenga remoto y `publish.yml`.

## Por confirmar

- **¿El código pasa a ser público?** `specs/goals.md` del monorepo dice que los repositorios
  serán públicos en GitHub, pero **nueve de los diez `.csproj` siguen declarando lo contrario**:
  `EnableSourceLink=false`, `EnableSourceControlManagerQueries=false` y sin `<RepositoryUrl>`.
  Salen todos del mismo git, así que es **una sola decisión**, no diez.

  Consecuencias de cambiarlo, para que se decida con ellas a la vista:
  - Cada `.csproj` corregido **cuesta una publicación de ese paquete**: la metadata de nuget.org
    es inmutable por versión.
  - El README deja de tener obligatoria la sección *Historial de versiones* —pasa a haber
    commits públicos que consultar— y puede volver a enlazar a `github.com`.
  - SourceLink empieza a funcionar: el depurador del consumidor puede entrar en el código.

  Por eso la recomendación es aplicarlo **paquete a paquete, cuando a cada uno le toque
  publicar**, en lugar de en un cambio único que obligue a publicar diez versiones seguidas.

- **`Persiltech.UserServices.Abstractions` declara `<RepositoryUrl>` apuntando a
  `https://github.com/aldazsoft/UserServices.Abstractions.git`**, su repositorio de la etapa
  anterior. Ya no es donde vive el código. Es el único de los diez con metadata de repositorio,
  y por eso el plan lo señala como incoherente con sus vecinos.

### Resueltas

- **Titular de la licencia** — se decidió **Persiltech** el 2026-08-19, y así está en el
  `LICENSE` de `Persiltech.Packages`, que es el que se empaqueta en los diez.
- **`Persiltech.DomainValidation`** — su `LICENSE` declaraba a Miguel Muñoz Serafín (2025) frente
  al `<Copyright>` de Persiltech; se alineó a Persiltech, conservando el crédito del
  entrenamiento como atribución de origen.
- **Nombre de los directorios de `specs/`** — se unifica en el **id completo del paquete**
  (`specs/Persiltech.Results/`), igual que `src/`, `tests/` y `samples/`, para que ir del paquete
  a su especificación sea una concatenación y no una tabla de equivalencias. Ocho de los diez
  siguen con el nombre corto; ver _Detalle_.
- **El legacy `Persiltech.Result`** (singular, `1.0.0` – `1.0.6`) queda sustituido por
  `Persiltech.Results`.

## Detalle

Solo los paquetes que no están `listo`.

### El renombrado de `specs/` — ocho paquetes

Ocho directorios llevan el nombre corto en lugar del id completo:

| Hoy | Debe ser |
| --- | -------- |
| `specs/Blazor.JSInterop` | `specs/Persiltech.Blazor.JSInterop` |
| `specs/Email` | `specs/Persiltech.Email` |
| `specs/Localizer` | `specs/Persiltech.Localizer` |
| `specs/Membership` | `specs/Persiltech.Membership` |
| `specs/Membership.Email` | `specs/Persiltech.Membership.Email` |
| `specs/Results` | `specs/Persiltech.Results` |
| `specs/UserServices` | `specs/Persiltech.UserServices` |
| `specs/UserServices.Abstractions` | `specs/Persiltech.UserServices.Abstractions` |

`specs/Persiltech.DomainValidation` y `specs/Persiltech.Membership.OAuth` ya cumplen.

Es un renombrado, no una reescritura: **no toca el contenido, no toca ningún `.csproj` y no
cuesta ninguna publicación.** Se puede hacer de una vez para los ocho.

### Especificaciones que faltan

| Paquete | Falta |
| ------- | ----- |
| `Persiltech.Membership.OAuth` | `Package.md` **y** `PublicApi.md`: el directorio existe pero está vacío. Es el único paquete de la flota sin ninguna especificación |
| `Persiltech.Email` | `Package.md` |
| `Persiltech.Membership.Email` | `Package.md` |

`Package.md` se reconstruye desde el `.csproj` y queda congelado; `PublicApi.md` se levanta
leyendo la superficie implementada. Ambos llevan la nota de que se reconstruyeron al homologar y
no precedieron al código.

### `Persiltech.Blazor.JSInterop` — además, documentación

El historial de su README lista una **`1.0.2` que nunca llegó a nuget.org**. Se preparó al
homologar el paquete, pero el trabajo siguió hasta la `1.1.0` y esa versión no se publicó. En la
ficha del paquete invita a instalar algo que no existe. Hay que fundir sus cambios en la fila de
la versión que sí los publicó y retirarla.

> Es el mismo caso que `Persiltech.DomainValidation` resolvió en su `2.0.1`. La corrección solo
> entra en vigor al publicar la versión siguiente.

## Qué mide esta auditoría, y qué no

El auditor **no comprueba el estándar por su cuenta**: descubre los paquetes e invoca sobre cada
uno `Get-HomologationPlan.ps1`, del skill global `homologate-nuget-package`, que es la única
definición. Sin ese skill instalado, la auditoría falla en vez de improvisar.

Sí mide:

- **La etapa de calidad**, compilando cada paquete con `Nullable` y `GenerateDocumentationFile`
  forzados, porque son las dos propiedades que la homologación enciende y sin ellas un
  repositorio heredado compila limpio y oculta el trabajo real.
- **Si la compilación falla**, y entonces lo dice en lugar de reportar cero avisos.
- **Las secciones canónicas del README en español o en inglés**, y **el README correcto**: el que
  el `.csproj` empaqueta, que en un monorepo es el de `src/{PackageId}/`, no el de la raíz.
- **Que los workflows no apunten a archivos inexistentes** y que `publish.yml` traiga la guarda
  que aborta si la etiqueta no coincide con `<VersionPrefix>`.
- **Que el historial del README no liste versiones ausentes de nuget.org.**
- **Que los verificadores y las pruebas se llamen por lo que son**: `.Sample` en `samples/`,
  `.Tests` en `tests/`.
- **Que el `.slnx` enganche todos los proyectos empaquetables.** Uno sin anclar no lo compila la
  solución, así que CI pasa en verde sin haberlo tocado.
- **Que el `version:` de `specs/PublicApi.md` coincida con `<VersionPrefix>`**, distinguiendo las
  dos direcciones: por delante es un bump pendiente que `implement-nuget-package` aplicaría; por
  detrás es una especificación que se quedó atrás. El de `specs/Package.md` **no se compara**:
  está congelado por diseño.
- **Que el monorepo no mezcle las dos posturas sobre el código fuente.** Salen todos del mismo
  git; que unos `.csproj` declaren la metadata de repositorio y otros la apaguen publica fichas
  incoherentes desde un mismo commit.

No mide, y nunca supone:

- **Si un repositorio de GitHub es público**, ni **qué titular debe llevar una licencia** cuando
  el `LICENSE` y el `<Copyright>` no coinciden. Ambas salen como preguntas al usuario.
- **Qué versiones están publicadas en nuget.org** para decidir el estado. Consulta el feed para
  contrastar el historial del README, pero la versión que registra es la del `.csproj`, y la
  nombra como tal.
