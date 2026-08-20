---
# Generado por el skill audit-nuget-packages. Los conteos son del día de la auditoría.
packagesRoot: E:\Repos\Github\aldazsoft\Persiltech\Packages
siteUrl: https://aldazsoft.github.io
audited: 2026-08-19
totals:
  listo: 3
  sin-página: 1
  calidad: 0
  documentacion: 0
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
| `Persiltech.Localizer` | `sin-página` | 1.0.2 | 3 |
| `Persiltech.DomainValidation` | `listo` | 2.0.1 | 0 |
| `Persiltech.UserServices` | `listo` | 0.1.4 | 0 |
| `Persiltech.UserServices.Abstractions` | `listo` | 0.1.11 | 0 |

Las versiones son las que declara el `.csproj`, es decir, las que se publicarán la próxima
vez. **No consta que sean las publicadas en nuget.org** salvo donde se indique.

## Fuera de esta auditoría

- **`Persiltech.Results`** sigue en el monorepo (`Persiltech/Src/Persiltech.Results`), a la
  espera de decidir si sustituye al publicado `Persiltech.Result` (singular, 1.0.6), al que
  tres proyectos del monorepo aún apuntan. La copia previa en `Packages/Results` se eliminó.
- Los **~95 paquetes restantes** del monorepo no se auditan hasta extraerlos: el plan espera
  **un paquete por repositorio** y no sabe leer una solución con 80 proyectos.

## Pendiente de publicar

- **`Persiltech.Localizer` 1.0.2** — extraído del monorepo y homologado el 2026-08-19, sin
  publicar y sin commitear. Su ficha en nuget.org sigue mostrando la `1.0.1`, cuyo
  *Project website* apunta al repositorio del monorepo. **Despliega el sitio con su página
  antes de publicar**, o la ficha corregida enlazará a un 404.

## Por confirmar

Ninguna abierta.

### Resueltas

- **`Persiltech.DomainValidation`** — ¿es público `https://github.com/aldazsoft/DomainValidation`?
  **No** (2026-08-19). Metadata de repositorio retirada y SourceLink apagado.
- **`Persiltech.DomainValidation`** — el `LICENSE` declaraba a Miguel Muñoz Serafín (2025) como
  titular frente al `<Copyright>` de Persiltech. Se alineó a **Persiltech (2026)** por decisión
  del usuario el 2026-08-19. El crédito del entrenamiento se conserva como atribución de origen.
- **Titular de la licencia para los paquetes que salen del monorepo** — el `LICENSE` del
  monorepo dice `2025 aldazsoft`. Se decidió **Persiltech** el 2026-08-19, y así se aplicó a
  `Persiltech.Localizer`. **Vale para todos los que se extraigan después.**
- **Versiones publicadas**, verificadas contra nuget.org el 2026-08-19:
  - `Persiltech.DomainValidation`: `1.0.1`, `2.0.0`, `2.0.1` listadas; `1.0.0` deslistada. Las
    `1.0.2` y `1.0.3` nunca se publicaron.
  - `Persiltech.Localizer`: `1.0.0` y `1.0.1` listadas.
  - `Persiltech.Results`: `1.0.0` listada.

## Detalle

Solo los paquetes que no están `listo`.

### Persiltech.Localizer — `sin-página`

`Localizer` · net10.0 · ruta prevista `/Localizer`

El paquete **cumple el estándar entero**: estructura, metadata, README y 0 avisos de
compilación. Lo único que falta es darlo de alta en el portafolio, que es el último paso.

**Sitio** (3)

- No está declarado en `specs/Packages.md`
- Sin página `/Localizer` en el sitio
- Sin la ruta `Localizer` en `build-pages.sh`

> Es el primer paquete **extraído** del monorepo, no solo homologado: se movieron su proyecto y
> sus dos aplicaciones de verificación, se creó el repositorio desde cero y se retiró del
> monorepo junto con su workflow. El monorepo quedó con 21 borrados y su `.sln` modificado, sin
> commitear.

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
  el README esté en el mismo idioma que el `<Description>`. Corregido el 2026-08-19, tras
  reportar como ausentes cuatro secciones que `Persiltech.Localizer` sí tenía, en inglés.

No mide, y nunca supone:

- **Si un repositorio de GitHub es público**, ni **qué titular debe llevar una licencia** cuando
  el `LICENSE` y el `<Copyright>` no coinciden. Ambas salen como preguntas al usuario.
