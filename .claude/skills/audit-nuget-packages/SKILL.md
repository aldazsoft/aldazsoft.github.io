---
name: audit-nuget-packages
description: Audita, sin escribir en ellos, todos los repositorios de paquetes NuGet que cuelgan de packagesRoot y levanta el inventario de homologación en specs/PackageInventory.md — qué le falta a cada paquete para cumplir el estándar (estructura, metadata del .csproj, README, avisos de compilación y página en el sitio) y en qué orden conviene atacarlos. Usa este skill cuando el usuario pregunte por el estado de la flota de paquetes, pida saber cuáles están homologados o cuánto trabajo queda, o quiera la cola de trabajo antes de empezar a corregir (Ej. "¿cómo están mis paquetes?", "levanta el inventario", "¿cuáles faltan por homologar?").
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

`Get-PackageAudit.ps1` es un recorrido: descubre los repositorios e invoca sobre cada uno
`Get-HomologationPlan.ps1`, del skill global `homologate-nuget-package`, que es la **única
definición** del estándar a nivel de paquete. Si el auditor volviera a comprobar por su cuenta,
habría dos definiciones y se separarían.

Lo que este skill aporta es lo que el plan no puede saber, porque solo ve un repositorio:

| Lo pone el plan (global) | Lo pone este skill (portafolio) |
|---|---|
| Estructura, metadata, README, avisos de compilación | Descubrimiento de la flota |
| Las preguntas bloqueantes | La ruta asignada a cada paquete, de `specs/Packages.md` |
| | Si el paquete ya tiene página, ruta y entrada en el catálogo |

**Dependencia:** sin el skill global instalado, este no funciona, y falla con un
`CRITICAL_ERROR` que lo dice. Es deliberado: preferible a comprobar contra un estándar propio
que envejecería aparte.

---

## Alcance

- **Los repositorios de los paquetes son de solo lectura, sin excepción.** Lo que no cumpla el
  estándar **se anota en el inventario**, no se arregla. La única escritura que provoca la
  auditoría es la de `obj/` y `bin/` al compilar para medir avisos, que con `-fast` no ocurre.
- **No inventa lo que no puede comprobar.** Ver *Lo que no se puede saber leyendo el disco*.
- **No modifica `specs/Packages.md`.** Ese archivo declara qué documenta el sitio, y dar de
  alta un paquete es el último paso de la homologación, no el primero. El inventario vive aparte.
- **No encadena con ningún otro skill** ni empieza a corregir por su cuenta.

---

## Reglas generales
- El directorio de trabajo es la raíz del repositorio del portafolio.
- `packagesRoot` y `siteUrl` salen del frontmatter de `specs/Packages.md`.
- El único archivo que este skill escribe es `specs/PackageInventory.md`.
- Si falta un dato que impida completar la tarea, detente y pregunta.

---

## Flujo de ejecución

### Paso 0 — Parámetros

Lee el frontmatter de `specs/Packages.md` y toma `packagesRoot` y `siteUrl`. Si el usuario
nombra paquetes concretos, pásalos con `-package`.

### Paso 1 — Auditoría

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "{SKILL_DIR}/scripts/Get-PackageAudit.ps1" `
    -packagesRoot "{{packagesRoot}}" `
    -siteUrl "{{siteUrl}}" `
    -sitePath "{{raíz del portafolio}}" `
    -asText
```

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
| `CRITICAL_ERROR: No existe el directorio de paquetes '...'` | Detente. El `packagesRoot` es incorrecto o el disco no está montado |
| `CRITICAL_ERROR: No se encontró el plan de homologación en '...'` | Falta el skill global `homologate-nuget-package`. Sin él este skill no puede auditar; no improvises comprobaciones |
| Un paquete en estado `no-legible` | El plan no encontró proyecto empaquetable, o no devolvió JSON. Es un problema de la auditoría, no del paquete. Anótalo aparte |

### Paso 2 — Los estados

Cada paquete se clasifica por **la primera etapa sin terminar**, que es el orden de trabajo
porque cada una se apoya en la anterior. Las cuatro primeras las decide el plan:

| Estado | Qué significa |
|---|---|
| `estructura` | Falta el esqueleto: `.slnx`, gestión centralizada de paquetes, `Directory.Build.props`, `specs/` |
| `metadata` | El `.csproj` no cumple: versión, `PackageProjectUrl`, licencia, símbolos, documentación XML |
| `documentacion` | El `README.md` publicado no tiene la forma canónica |
| `calidad` | Compila con avisos: miembros públicos sin documentar (`CS1591`) o nulabilidad sin anotar |
| `sin-página` | El paquete cumple; falta darlo de alta en el portafolio |
| `sin-medir` | Cumple todo lo comprobado, pero se ejecutó con `-fast` y la calidad no se midió |
| `listo` | Cumple y está documentado en el sitio |
| `no-legible` | El plan no pudo leer el repositorio |

Un paquete acumula pendientes de varias etapas; el estado solo dice por dónde empezar. El
recuento entre paréntesis es el total.

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
packagesRoot: {{packagesRoot}}
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

## Resumen

| Paquete | Estado | Versión del proyecto | Pendientes |
|---|---|---|---|

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

- El recuento por estado y cuántos repositorios se auditaron
- Si se usó `-fast`, que la calidad no se midió y por eso nadie sale `listo`
- Los paquetes `no-legible`
- Las preguntas bloqueantes, todas juntas
- Que **no se escribió en ningún repositorio de paquete** y que `specs/Packages.md` sigue igual
- Que el inventario es una foto del día

Si el usuario va a empezar a homologar, recuérdale lo que el orden impone:

> La metadata de nuget.org es inmutable por versión, así que cada `PackageProjectUrl`
> corregido cuesta una publicación. **El sitio se despliega antes de publicar los paquetes**:
> si se publica primero, cada ficha corregida apunta a una página que todavía no existe.

**Aquí termina este skill.** No propongas correcciones concretas ni empieces a aplicarlas.
