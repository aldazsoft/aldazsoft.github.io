---
# Generado por el skill audit-nuget-packages. Los conteos son del día de la auditoría.
packagesRoot: E:\Repos\Github\aldazsoft\Persiltech\Packages
siteUrl: https://aldazsoft.github.io
audited: 2026-08-19
totals:
  listo: 3
  sin-página: 0
  documentación: 0
  metadata: 0
  estructura: 0
  no-legible: 0
---

# Inventario de homologación

Qué le falta a cada paquete para cumplir el estándar, y en qué orden conviene atacarlo.
Este archivo lo levanta una auditoría de solo lectura: ningún repositorio de paquete se
modificó al generarlo.

El estado es **la primera etapa sin terminar**, no el único problema: un paquete acumula
bloqueos de varias etapas a la vez. El orden `estructura → metadata → documentación →
sin-página` es el de trabajo, porque cada etapa se apoya en la anterior.

## Resumen

| Paquete | Estado | Versión del proyecto | Bloqueos |
|---|---|---|---|
| `Persiltech.DomainValidation` | `listo` | 1.0.2 | 0 |
| `Persiltech.UserServices` | `listo` | 0.1.4 | 0 |
| `Persiltech.UserServices.Abstractions` | `listo` | 0.1.11 | 0 |

Las versiones son las que declara el `.csproj`, es decir, las que se publicarán la próxima
vez. **No consta que sean las publicadas en nuget.org**: eso no se deduce del disco.

## Pendiente de publicar

- **`Persiltech.DomainValidation` 1.0.2** — homologado en local el 2026-08-19, sin publicar.
  Hasta que se publique, la ficha de nuget.org sigue mostrando la `1.0.1`, cuyo
  *Project website* apunta a `github.com/aldazsoft/DomainValidation` (privado). El orden es:
  **desplegar el sitio primero, publicar el paquete después.**

## Por confirmar

Preguntas que la auditoría no puede responder leyendo el disco.

Ninguna abierta.

### Resueltas

- **`Persiltech.DomainValidation`** — ¿es público `https://github.com/aldazsoft/DomainValidation`?
  **No** (confirmado por el usuario el 2026-08-19). La metadata de repositorio se retiró del
  `.csproj` y SourceLink quedó apagado.
- **`Persiltech.DomainValidation`** — el `LICENSE` declaraba a Miguel Muñoz Serafín (2025) como
  titular, mientras el `.csproj` declaraba Copyright de Persiltech. Se alineó a
  **Persiltech (2026)** por decisión del usuario el 2026-08-19. El crédito del entrenamiento se
  conserva en el README y en la página del portafolio, que es atribución de origen, no de
  titularidad.

## Detalle

Solo los paquetes que no están `listo`.

Ninguno.

## Qué mide esta auditoría, y qué no

El auditor **no comprueba el estándar por su cuenta**: recorre los repositorios e invoca sobre
cada uno `Get-HomologationPlan.ps1`, del skill global `homologate-nuget-package`, que es la
única definición. Sin ese skill instalado, la auditoría falla en vez de improvisar.

Sí mide, desde la refactorización del 2026-08-19:

- **La etapa de calidad**, que antes se le escapaba: `Persiltech.DomainValidation` llegó a
  `listo` con 147 avisos abiertos. Ahora compila cada paquete con `Nullable` y
  `GenerateDocumentationFile` forzados, porque son las dos propiedades que la homologación
  enciende y sin ellas un repositorio heredado compila limpio y oculta el trabajo real.
- En `DomainValidation` fueron **91 miembros públicos sin documentar y 21 avisos de
  nulabilidad**, y cerrarlos costó más que toda la homologación de empaquetado junta. **A
  escala de la flota es el coste dominante**, y conviene contar con él al planificar.

No mide, y nunca supone:

- **Qué versiones están publicadas en nuget.org.** El `.csproj` dice qué se publicará la
  próxima vez, no qué se publicó.
- **Si un repositorio de GitHub es público**, ni **qué titular debe llevar una licencia** cuando
  el `LICENSE` y el `<Copyright>` no coinciden. Ambas salen como preguntas al usuario.
