---
# Estado del traslado de paquetes desde el monorepo a repositorios individuales.
# Lo mantiene el skill extract-package. Sobrevive al cierre de la terminal: al retomar,
# esto es lo primero que se lee.
monorepoRoot: E:\Repos\Github\aldazsoft\Persiltech\Persiltech
packagesRoot: E:\Repos\Github\aldazsoft\Persiltech\Packages
sitePath: E:\Repos\Github\aldazsoft\aldazsoft.github.io
updated: 2026-08-21

# Paquete en vuelo y paso en el que se quedó. Vacío = no hay ninguno empezado.
current:
  package: Persiltech.Results
  step: 10 — fila de la 1.0.1 añadida e inventario refrescado. Solo falta desplegar el sitio (8)

# Traslados empezados y aparcados a propósito. No se olvidan: se retoman por donde dice 'step'.
paused:
  - package: Persiltech.HttpDelegatingHandlers
    step: 8 — página desplegada y viva. Falta 9, publicar la 1.0.3
    since: 2026-08-22
    reason: Salto deliberado a Persiltech.Results, pedido por el usuario
---

# Traslado de paquetes

El monorepo `Persiltech` aloja decenas de paquetes en una sola solución. Se están sacando de
uno en uno a su propio repositorio, homologados al estándar de la casa, y documentados en este
portafolio.

**No hay restricción de orden por dependencias.** Los proyectos de `Src/` se consumen entre sí
por `PackageReference` desde nuget.org, no por `ProjectReference`: hay **cero** referencias de
proyecto entre ellos. El archivo `Info/Orden proyecto Persiltech.txt` del monorepo es el orden
de **republicación**, no de extracción; confundirlos cuesta semanas de secuenciación inútil.

## Los once pasos

| # | Paso | Repositorio | Herramienta |
|---|---|---|---|
| 1 | Elegir el candidato | monorepo | `Get-ExtractionCandidates.ps1` |
| 2 | Extraer: mover proyecto y verificadores, `git init`, crear estructura | paquete | a mano |
| 3 | Homologar | paquete | `homologate-nuget-package` *(global)* |
| 4 | Mejoras opcionales | paquete | `implement-nuget-package` *(global)* |
| 5 | Retirar del monorepo y commitear | monorepo | a mano |
| 6 | Crear el remoto en GitHub y empujar | paquete | **el usuario**, guiado |
| 7 | Alta en el sitio: especificación, catálogo, página, ruta | portafolio | `sync-package-pages` |
| 8 | **Desplegar el sitio** | portafolio | push a `main` |
| 9 | **Publicar** en nuget.org | paquete | **el usuario**, etiqueta `v*` |
| 10 | Añadir la fila de la versión y volver a desplegar | portafolio | `sync-package-pages` |
| 11 | Refrescar el inventario | portafolio | `audit-nuget-packages` |

### Por qué el 8 va antes que el 9

La metadata de nuget.org es **inmutable por versión**. Si el paquete se publica antes de que la
página exista, el `<PackageProjectUrl>` de esa versión apunta a un 404 **para siempre**: solo se
corrige publicando otra versión. Es el error más caro del flujo y el único que no tiene vuelta
atrás.

### Por qué el sitio se despliega dos veces

El catálogo **solo lista versiones verificadas en nuget.org**, política adoptada tras el
episodio de la `1.0.2` fantasma de `DomainValidation`. Así que:

- El paso 8 despliega la página **sin** la fila de la versión nueva → la URL ya resuelve.
- El paso 10 añade la fila cuando la versión existe de verdad → segundo despliegue.

Lo que arregla el 404 es la **página y la ruta**, no la fila del historial.

## Cola acordada

| Orden | Paquete | Dependientes | Archivos | `CS1591` | Por qué |
|---|---|---|---|---|---|
| **1** | `Persiltech.HttpDelegatingHandlers` | 5 | 5 | **16** | La mejor relación aporte/coste de la flota. Se compone en la aplicación del consumidor, así que estrena `samples/` con un caso que lo justifica |
| 2 | `Persiltech.Shared` | 4 | 5 | 36 | Sin dependencias. Es la dependencia interna del anterior |
| 3 | `Persiltech.Exceptions` | 7 | 12 | 70 | El más consumido de los que quedan, pero cuesta 4× en documentación y arrastra `Validation` (148 avisos) |

**`Persiltech.Results` se adelantó** el 2026-08-22, por delante de `Shared` y `Exceptions`, a
petición del usuario. No arrastra ninguna dependencia de los que se saltan: la única que tiene
es `Persiltech.Localizer`, ya extraído y publicado. Cuesta 88 avisos, y es el primero que trae
**pruebas xUnit de verdad** en `tests/`, así que estrena un `dotnet test` que ejecuta algo.

Sigue siendo un frente propio: su valor de fondo es desbloquear la retirada del legacy
`Persiltech.Result` (singular, `1.0.6`), que tres proyectos del monorepo aún consumen.

La cola se recalcula con el script cuando convenga; estas cifras son del 2026-08-21.

## Ya trasladados

| Paquete | Publicado | En el sitio | Notas |
|---|---|---|---|
| `Persiltech.Localizer` | `1.0.2` | sí | Primera extracción. De aquí salió el procedimiento |
| `Persiltech.Blazor.JSInterop` | `1.1.1` | sí | Le queda retirar del README una `1.0.2` que nunca se publicó |

Quedan **76 proyectos** en `Src/` del monorepo.

## Decisiones que ya no se vuelven a preguntar

Viven detalladas en `PackageInventory.md`; aquí el resumen operativo:

- **Titular de la licencia**: Persiltech. El `LICENSE` del monorepo dice `2025 aldazsoft` y se
  reemplaza al extraer. Vale para todos.
- **Código privado**: todos los paquetes de la casa. El `.csproj` retira la metadata de
  repositorio y apaga SourceLink; ninguna página enlaza a GitHub.
- **Idioma**: el README va en el idioma de su `<Description>`; la página del sitio, en español.
- **Los `.Tests` del monorepo casi nunca son pruebas**: suelen ser apps de verificación.
  Compruébalo por el SDK del `.csproj` antes de moverlos, y **renómbralos al mover** según el
  estándar: `{{paquete}}.Sample` en `samples/` si es un verificador,
  `{{paquete}}.Tests` en `tests/` si son pruebas xUnit. El monorepo mezcla ambos nombres —y a
  veces el singular `.Test`—, pero en el repositorio extraído el nombre tiene que decir la
  verdad: si no, `dotnet test` en CI pasa en verde sin ejecutar nada.

## Trampas que ya nos costaron una corrección

- **Los `.yml` no son genéricos.** Llevan dentro el nombre de la solución y del `.csproj`.
  Copiarlos sin adaptar deja CI roto en la primera ejecución. El plan ya lo detecta.
- **`Directory.Packages.props` del monorepo hace también de `Directory.Build.props`**: el TFM y
  el `LangVersion` viven ahí. Un proyecto sacado sin eso no compila.
- **Los `PackageReference` sin versión** dependen del CPM del monorepo; hay que recuperar las
  versiones de allí antes de mover.
- **`bin/`, `obj/` y `.csproj.user`** viajan en la copia si no se excluyen.
- **Publicar antes de desplegar sale barato solo si la URL ya es la correcta.** El 2026-08-22
  la `1.0.1` de `Results` se publicó con el sitio sin desplegar: su `projectUrl` apuntaba a
  `https://aldazsoft.github.io/Results/`, que devolvía `404`. No se quemó la versión —la
  metadata era correcta y el despliegue la arregla—, pero entre publicar y empujar la ficha de
  nuget.org enlazó a una página que no existía. Lo irreparable es publicar con la URL
  **equivocada**; esto fue solo una ventana de 404. Aun así, el orden sigue siendo 8 → 9.
- **Al retirar del monorepo, el directorio sobrevive al `git commit`**: `obj/` está en el
  `.gitignore`, así que `git status` sale limpio y el proyecto parece borrado cuando no lo está.
  Comprueba que el recuento de `Src/` bajó, y busca residuos así:

  ```bash
  for d in Src/*/ Tests/*/; do
    [ -z "$(find "$d" -name '*.csproj' -not -path '*/obj/*' | head -1)" ] && echo "$d"
  done
  ```

  > Busca **ningún `.csproj` en todo el árbol**, no uno que coincida con el nombre del
  > directorio: hay proyectos legítimos donde ambos difieren —`Persiltech.WebPush.BlazorTests`
  > contiene `Persiltech.WebPush.Blazor.Tests.csproj`— o donde el `.csproj` cuelga de un
  > subdirectorio. La versión ingenua los marca a todos como residuo.
