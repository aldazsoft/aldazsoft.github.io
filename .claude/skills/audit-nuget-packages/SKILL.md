---
name: audit-nuget-packages
description: Audita, sin escribir en ellos, todos los paquetes NuGet de la flota —los del monorepo Persiltech.Packages y los que aún tienen repositorio propio— y levanta el inventario de homologación en specs/PackageInventory.md, con qué le falta a cada uno para cumplir el estándar (estructura, metadata del .csproj, README, avisos de compilación y página en el sitio) y en qué orden conviene atacarlos. Usa este skill cuando el usuario pregunte por el estado de la flota de paquetes, pida saber cuáles están homologados o cuánto trabajo queda, o quiera la cola de trabajo antes de empezar a corregir (Ej. "¿cómo están mis paquetes?", "levanta el inventario", "¿cuáles faltan por homologar?").
allowed-tools:
  - PowerShell
  - Read
  - Write
  - Edit
  - Glob
  - Grep
---

# Auditor de la flota de paquetes

## Objetivo
Saber, sobre datos y no sobre impresiones, qué le falta a cada paquete para cumplir el
estándar de la casa, y dejarlo escrito en un inventario que sirva de cola de trabajo.

Este skill **no homologa nada**. Produce la lista de lo que hay que hacer; hacerlo es
`homologate-nuget-package`, y el usuario decide cuándo.

## Cuándo usar este skill
- Levantar por primera vez el inventario de la flota
- Refrescarlo tras homologar unos cuantos paquetes, para ver qué queda
- Responder cuántos paquetes están listos, cuáles bloquean y por qué

**No uses este skill** para corregir un paquete, escribir su `README.md` o tocar su `.csproj`:
eso es homologación, y aquí no se escribe en ningún repositorio de paquete.

---

## De dónde sale el estándar

**Este skill no define qué es "homologado", y no lo comprueba por su cuenta.**

`Get-PackageAudit.ps1` es un recorrido: descubre los paquetes e invoca sobre cada uno
`Get-HomologationPlan.ps1`, del skill global `homologate-nuget-package`, que es la **única
definición** del estándar a nivel de paquete. Si el auditor volviera a comprobar por su cuenta,
habría dos definiciones y se separarían.

Lo que este skill aporta es lo que el plan no puede saber, porque solo ve un paquete:

| Lo pone el plan (global) | Lo pone este skill (portafolio) |
|---|---|
| Estructura, metadata, README, avisos de compilación | Descubrimiento de la flota |
| Las preguntas bloqueantes | La ruta asignada a cada paquete, de `specs/Packages.md` |
| Qué es del repositorio y qué del paquete | Si el paquete ya tiene página, ruta y entrada en el catálogo |
| | Agrupar por repositorio lo que es del repositorio |

---

## Dónde vive la flota

La mayoría de los paquetes se publican desde el **monorepo** `Persiltech.Packages`: un
repositorio, un `.slnx`, y un proyecto empaquetable por paquete bajo `src/`. Los que todavía
tienen repositorio propio se auditan igual, pero solo si `specs/Packages.md` los declara con
`path`.

**Esa lista es la que manda, y no es un detalle.** Al trasladar un paquete al monorepo, su
repositorio de origen se queda en disco con una copia. Recorrer el directorio a ciegas auditaría
cada paquete **dos veces** y daría por pendiente en la copia vieja lo que ya se arregló en el
monorepo. El auditor las salta y las enumera aparte, como *no auditadas*.

**Los pendientes del repositorio se cuentan una vez.** El plan separa lo que es del repositorio
—la solución, la gestión centralizada, el `LICENSE`, el `.gitattributes`, los workflows— de lo
que es del paquete, y el auditor lo agrupa por repositorio. Sin eso, un `publish.yml` que falta
saldría diez veces y dejaría los diez paquetes del monorepo en estado `estructura` por un solo
defecto que se arregla una vez.

Lo mismo con las **preguntas bloqueantes**: el plan las emite por paquete porque solo ve uno,
pero "¿el código es público?" es una pregunta del repositorio. El auditor las agrupa por texto y
dice a qué paquetes afecta cada una.

**Dependencia:** sin el skill global instalado, este no funciona, y falla con un
`CRITICAL_ERROR` que lo dice. Es deliberado: preferible a comprobar contra un estándar propio
que envejecería aparte.

---

## Alcance

- **El monorepo y los repositorios de paquete son de solo lectura, sin excepción.** Lo que no
  cumpla el estándar **se anota en el inventario**, no se arregla. La única escritura que provoca
  la auditoría es la de `obj/` y `bin/` al compilar para medir avisos, que con `-fast` no ocurre.
- **No inventa lo que no puede comprobar.** Ver *Lo que no se puede saber leyendo el disco*.
- **No modifica `specs/Packages.md`.** Ese archivo declara qué documenta el sitio, y dar de
  alta un paquete es el último paso de la homologación, no el primero. El inventario vive aparte.
- **No encadena con ningún otro skill** ni empieza a corregir por su cuenta.

---

## Reglas generales
- El directorio de trabajo es la raíz del repositorio del portafolio.
- `monorepoRoot`, `legacyPackagesRoot` y `siteUrl` salen del frontmatter de `specs/Packages.md`.
- El único archivo que este skill escribe es `specs/PackageInventory.md`.
- Si falta un dato que impida completar la tarea, detente y pregunta.

---

## Flujo de ejecución

### Paso 0 — Parámetros

Lee el frontmatter de `specs/Packages.md` y toma `monorepoRoot`, `legacyPackagesRoot` y
`siteUrl`. Si el usuario nombra paquetes concretos, pásalos con `-package`, por su id o por el
nombre de su directorio.

### Paso 1 — Auditoría

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "{SKILL_DIR}/scripts/Get-PackageAudit.ps1" `
    -monorepoRoot "{{monorepoRoot}}" `
    -legacyPackagesRoot "{{legacyPackagesRoot}}" `
    -siteUrl "{{siteUrl}}" `
    -sitePath "{{raíz del portafolio}}" `
    -asText
```

> `-legacyPackagesRoot` se omite cuando `specs/Packages.md` ya no declara ninguna entrada con
> `path`, es decir, cuando toda la flota vive en el monorepo.

> `{SKILL_DIR}` es el directorio de este `SKILL.md`, dentro de `.claude/skills/` del
> portafolio. Resuélvelo a ruta absoluta: `pwsh -File` no acepta rutas relativas de forma fiable.

`-asText` da el recuento por estado y las preguntas agrupadas. Para el detalle, ejecútalo con
`-package {{nombre}}` y sin `-asText`: la salida es JSON.

**Sobre `-fast`.** Sin él, el plan compila cada paquete para medir los avisos, que es la etapa
más cara de la homologación y la que no se puede estimar de otro modo. Con una flota grande
eso son minutos; `-fast` los ahorra, pero entonces **ningún paquete puede salir `listo`**:
salen como `sin-medir`. Úsalo para un primer barrido y repite sin él antes de planificar.

**Con muchos paquetes, no vuelques el JSON completo al contexto.**

| Salida | Acción |
|---|---|
| `CRITICAL_ERROR: No existe el monorepo '...'` | Detente. El `monorepoRoot` es incorrecto o el disco no está montado |
| `CRITICAL_ERROR: No se encontró el plan de homologación en '...'` | Falta el skill global `homologate-nuget-package`. Sin él este skill no puede auditar; no improvises comprobaciones |
| `CRITICAL_ERROR: El plan no devolvió una lista de paquetes legible` | El skill global está instalado pero es una versión anterior, sin `-listPackages`. Sin ella no se puede descubrir qué hay dentro del monorepo. Detente y actualízalo |
| Un paquete en estado `no-legible` | El plan no encontró proyecto empaquetable, o no devolvió JSON. Es un problema de la auditoría, no del paquete. Anótalo aparte |
| Un bloque `No auditados (n)` | No es error: son los repositorios sueltos cuyo paquete ya vive en el monorepo. Menciónalos en el Paso 5 como candidatos a borrar, y **no los audites a mano** |

### Paso 2 — Los estados

Cada paquete se clasifica por **la primera etapa sin terminar**, que es el orden de trabajo
porque cada una se apoya en la anterior. Las cuatro primeras las decide el plan:

| Estado | Qué significa |
|---|---|
| `estructura` | Le falta a **este paquete** parte del esqueleto: sus especificaciones, o su directorio de `specs/` con el nombre que no toca |
| `metadata` | El `.csproj` no cumple: versión, `PackageProjectUrl`, licencia, símbolos, documentación XML |
| `documentacion` | El `README.md` publicado no tiene la forma canónica |
| `calidad` | Compila con avisos: miembros públicos sin documentar (`CS1591`) o nulabilidad sin anotar |
| `sin-página` | El paquete cumple; falta darlo de alta en el portafolio |
| `sin-medir` | Cumple todo lo comprobado, pero se ejecutó con `-fast` y la calidad no se midió |
| `listo` | Cumple y está documentado en el sitio |
| `no-legible` | El plan no pudo leer el repositorio |

Un paquete acumula pendientes de varias etapas; el estado solo dice por dónde empezar. El
recuento entre paréntesis es el total.

**Los pendientes del repositorio no cuentan para el estado de ningún paquete**, y es
deliberado: un `publish.yml` que falta es un trabajo, no diez, y dejar los diez paquetes del
monorepo en `estructura` por él escondería lo que de verdad le falta a cada uno. Salen en su
propio bloque, `REPOSITORIO {{nombre}}`, y se atacan **antes** que cualquier paquete: se
arreglan una vez y desbloquean a todos.

**`calidad` es la etapa que más cuesta y la que menos se ve venir.** Documentar con XML
decenas de miembros públicos supera con holgura el trabajo de metadata y empaquetado juntos.

### Paso 3 — Lo que no se puede saber leyendo el disco

El plan emite **preguntas bloqueantes** y el auditor las agrupa al final. No las respondas por
tu cuenta: preséntaselas todas juntas al usuario. Hoy son dos —si un repositorio de GitHub es
público, y qué titular debe llevar la licencia cuando el `LICENSE` y el `<Copyright>` no
coinciden—, y ambas tienen consecuencias que solo él puede decidir.

Hay además un dato que ninguno de los dos puede comprobar: **qué versiones están publicadas en
nuget.org**. El `.csproj` dice qué se publicará la próxima vez, no qué se publicó. El
inventario registra la versión del proyecto y la nombra como tal; no la declara publicada.

Anota todo eso como `por confirmar`. Un inventario que lo suponga es peor que no tenerlo,
porque se actúa sobre él.

### Paso 4 — El inventario

Escribe `specs/PackageInventory.md`. Si ya existe, **actualízalo**: conserva las anotaciones
que el usuario haya añadido a mano y las respuestas a los `por confirmar` ya resueltas.

```markdown
---
# Generado por el skill audit-nuget-packages. Los conteos son del día de la auditoría.
monorepoRoot: {{monorepoRoot}}
legacyPackagesRoot: {{legacyPackagesRoot}}
siteUrl: {{siteUrl}}
audited: {{AAAA-MM-DD}}
totals:
  listo: 0
  sin-página: 0
  calidad: 0
  documentacion: 0
  metadata: 0
  estructura: 0
  no-legible: 0
---

# Inventario de homologación

## Pendientes del repositorio

Lo que es del repositorio y no de ningún paquete, agrupado por repositorio. Va primero porque
se arregla una vez y desbloquea a todos sus paquetes.

## Resumen

| Paquete | Repositorio | Estado | Versión del proyecto | Pendientes |
|---|---|---|---|---|

## Pendiente de publicar

Paquetes homologados en local y todavía sin publicar, con lo que su ficha sigue mostrando.

## Por confirmar

Preguntas que la auditoría no puede responder leyendo el disco, y las ya resueltas con su fecha.

## Detalle

Solo los paquetes que no están `listo`, con sus pendientes por etapa.
```

La fecha de `audited` es la del día en que se ejecuta. **No la inventes**: si no la tienes,
pregúntala.

### Paso 5 — Confirmación

Indica al usuario:

- El recuento por estado, cuántos paquetes se auditaron y en cuántos repositorios
- **Los pendientes del repositorio, aparte y primero**, con el aviso de que se arreglan una vez
  y valen para todos sus paquetes
- Si se usó `-fast`, que la calidad no se midió y por eso nadie sale `listo`
- Los paquetes `no-legible`
- **Los repositorios no auditados**, si los hay: son copias que quedaron atrás de paquetes ya
  trasladados al monorepo, y borrarlos evita que alguien edite la copia equivocada
- Las preguntas bloqueantes, todas juntas y ya agrupadas, diciendo a qué paquetes afecta cada una
- Que **no se escribió en el monorepo ni en ningún repositorio de paquete** y que
  `specs/Packages.md` sigue igual
- Que el inventario es una foto del día

Si el usuario va a empezar a homologar, recuérdale lo que el orden impone:

> La metadata de nuget.org es inmutable por versión, así que cada `PackageProjectUrl`
> corregido cuesta una publicación. **El sitio se despliega antes de publicar los paquetes**:
> si se publica primero, cada ficha corregida apunta a una página que todavía no existe.

**Aquí termina este skill.** No propongas correcciones concretas ni empieces a aplicarlas.
