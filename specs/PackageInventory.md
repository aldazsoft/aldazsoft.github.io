---
# Generado por el skill audit-nuget-packages. Los conteos son del día de la auditoría.
packagesRoot: E:\Repos\Github\aldazsoft\Persiltech\Packages
siteUrl: https://aldazsoft.github.io
audited: 2026-08-22
totals:
  listo: 6
  sin-página: 0
  calidad: 0
  documentacion: 1
  metadata: 0
  estructura: 0
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
| `Persiltech.Blazor.JSInterop` | `documentacion` | 1.1.1 | 1 |
| `Persiltech.HttpDelegatingHandlers` | `listo` | 1.0.3 | 0 |
| `Persiltech.Results` | `listo` | 1.0.1 | 0 |
| `Persiltech.Localizer` | `listo` | 1.0.2 | 0 |
| `Persiltech.DomainValidation` | `listo` | 2.0.2 | 0 |
| `Persiltech.UserServices` | `listo` | 0.1.4 | 0 |
| `Persiltech.UserServices.Abstractions` | `listo` | 0.1.12 | 0 |

Las versiones son las que declara el `.csproj`. Todas coinciden con la última publicada en
nuget.org **salvo `Persiltech.HttpDelegatingHandlers`** (ver *Pendiente de publicar*),
verificado el 2026-08-22 (ver *Resueltas*).

## Fuera de esta auditoría

- Los **76 paquetes restantes** del monorepo no se auditan hasta extraerlos: el plan espera
  **un paquete por repositorio** y no sabe leer una solución con decenas de proyectos.

## Pendiente de publicar

| Paquete | En el `.csproj` | En nuget.org | Qué sigue mostrando la ficha |
|---|---|---|---|
| `Persiltech.HttpDelegatingHandlers` | 1.0.3 | 1.0.2 | El README anterior a la homologación, y sin `PackageProjectUrl` a su página |

Su página del portafolio **ya está publicada**, así que la `1.0.3` puede subirse cuando se
quiera: el `<PackageProjectUrl>` que trae ya resuelve.

## Por confirmar

Ninguna abierta.

### Resueltas

- **Titular de la licencia para los paquetes que salen del monorepo** — el `LICENSE` del
  monorepo dice `2025 aldazsoft`. Se decidió **Persiltech** el 2026-08-19. **Vale para todos
  los que se extraigan después**, y así se aplicó a `Localizer`, `Blazor.JSInterop`,
  `HttpDelegatingHandlers` y `Results`.
- **`Persiltech.DomainValidation`** — ¿es público `https://github.com/aldazsoft/DomainValidation`?
  **No** (2026-08-19). Metadata de repositorio retirada y SourceLink apagado. Su `LICENSE`
  declaraba a Miguel Muñoz Serafín (2025) frente al `<Copyright>` de Persiltech; se alineó a
  Persiltech, conservando el crédito del entrenamiento como atribución de origen.
- **Versiones publicadas**, verificadas contra nuget.org el 2026-08-22:
  - `Persiltech.Blazor.JSInterop`: `1.0.0`, `1.0.1`, `1.1.0`, `1.1.1`
  - `Persiltech.HttpDelegatingHandlers`: `1.0.0`, `1.0.1`, `1.0.2`
  - `Persiltech.Localizer`: `1.0.0`, `1.0.1`, `1.0.2`
  - `Persiltech.Results`: `1.0.0`, `1.0.1`
  - `Persiltech.DomainValidation`: `1.0.1`, `2.0.0`, `2.0.1`, `2.0.2` listadas; `1.0.0`
    deslistada. Las `1.0.2` y `1.0.3` nunca se publicaron
  - `Persiltech.UserServices`: `0.1.0` – `0.1.4`
  - `Persiltech.UserServices.Abstractions`: `0.1.0` – `0.1.12`
- **El legacy `Persiltech.Result`** (singular, `1.0.0` – `1.0.6`) queda sustituido por
  `Persiltech.Results`. Tres proyectos del monorepo aún apuntan al viejo y migrarán cuando
  toque; el CPM del monorepo ya ofrece el plural, para que ningún proyecto nuevo caiga en el
  antiguo por inercia.

## Detalle

Solo los paquetes que no están `listo`.

### Persiltech.Blazor.JSInterop — `documentacion`

`Blazor.JSInterop` · net10.0 · ruta `/Blazor.JSInterop`

Cumple estructura, metadata y calidad —0 avisos—, y **ya está dado de alta en el portafolio**:
catálogo, página y ruta. Le queda una sola cosa, en su propio repositorio.

**Documentación** (1)

- El historial del README lista una **`1.0.2` que nunca llegó a nuget.org**. Se preparó al
  homologar el paquete, pero el trabajo siguió hasta la `1.1.0` y esa versión no se publicó.
  En la ficha del paquete invita a instalar algo que no existe. Hay que fundir sus cambios en
  la fila de la versión que sí los publicó y retirarla.

> Es el mismo caso que `Persiltech.DomainValidation` resolvió en su `2.0.1`.

La corrección vive en el repositorio del paquete y solo entra en vigor al publicar la versión
siguiente.

> **Corregido el 2026-08-21:** una auditoría anterior anotó aquí que `specs/Package.md`
> declaraba `1.1.0` frente al `1.1.1` del `.csproj`. **No es un defecto.** Ese `version:` está
> congelado por diseño —es la entrada con la que nació el paquete—, y de hecho
> `Persiltech.UserServices` sigue declarando `0.1.0` con el proyecto en `0.1.4`. El que sí debe
> coincidir es el de `specs/PublicApi.md`, y aquí coincide.

## Qué mide esta auditoría, y qué no

El auditor **no comprueba el estándar por su cuenta**: recorre los repositorios e invoca sobre
cada uno `Get-HomologationPlan.ps1`, del skill global `homologate-nuget-package`, que es la
única definición. Sin ese skill instalado, la auditoría falla en vez de improvisar.

Sí mide:

- **La etapa de calidad**, compilando cada paquete con `Nullable` y `GenerateDocumentationFile`
  forzados, porque son las dos propiedades que la homologación enciende y sin ellas un
  repositorio heredado compila limpio y oculta el trabajo real.
- **Si la compilación falla**, y entonces lo dice en lugar de reportar cero avisos.
- **Las secciones canónicas del README en español o en inglés**, porque el estándar exige que
  el README esté en el mismo idioma que el `<Description>`.
- **Que los workflows no apunten a archivos inexistentes** —un `.yml` copiado de otro paquete
  deja CI roto en la primera ejecución— y que `publish.yml` traiga la guarda que aborta si la
  etiqueta no coincide con `<VersionPrefix>`.
- **Que el historial del README no liste versiones ausentes de nuget.org.**
- **Que los verificadores y las pruebas se llamen por lo que son**: `.Sample` en `samples/`,
  `.Tests` en `tests/`. Un verificador llamado `.Tests` hace que `dotnet test` pase en verde
  sin ejecutar nada.

No mide, y nunca supone:

- **Si un repositorio de GitHub es público**, ni **qué titular debe llevar una licencia** cuando
  el `LICENSE` y el `<Copyright>` no coinciden. Ambas salen como preguntas al usuario.

Y desde el 2026-08-21 mide también **que el `version:` de `specs/PublicApi.md` coincida con
`<VersionPrefix>`**, distinguiendo las dos direcciones: por delante es un bump pendiente que
`implement-nuget-package` aplicaría; por detrás es una especificación que se quedó atrás. El
`version:` de `specs/Package.md` **no se compara**: está congelado por diseño.
