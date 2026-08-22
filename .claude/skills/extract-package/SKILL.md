---
name: extract-package
description: Dirige el traslado de un paquete NuGet desde el monorepo Persiltech hasta su propio repositorio, publicado y documentado en el portafolio. Lee specs/Extraction.md para saber en qué paso quedó el trabajo, elige el siguiente candidato midiendo la flota, y delega cada etapa en el skill que le corresponde. Usa este skill al retomar el trabajo tras cerrar la terminal, al preguntar por dónde íbamos o cuál es el siguiente paquete, y para avanzar un paso del traslado (Ej. "¿dónde nos quedamos?", "sigamos con el siguiente paquete", "extrae Persiltech.Shared").
allowed-tools:
  - PowerShell
  - Bash
  - Read
  - Write
  - Edit
  - Glob
  - Grep
---

# Director del traslado de paquetes

## Objetivo
Que el traslado de decenas de paquetes sobreviva a cerrar la terminal, y que cada etapa la
haga quien sabe hacerla.

Este skill **no homologa, no sincroniza el sitio y no audita**. Sabe en qué paso está el
trabajo, elige el siguiente candidato y **delega**.

`specs/Extraction.md` es el estado, y la única fuente de verdad sobre dónde nos quedamos.

## Cuándo usar este skill
- Retomar tras cerrar la terminal o reiniciar: *"¿dónde nos quedamos?"*
- Elegir el siguiente paquete: *"¿cuál sigue?"*
- Avanzar un paso del traslado en curso

**No uses este skill** para homologar un paquete concreto, dar de alta una página o refrescar el
inventario si el usuario lo pide directamente: para eso están los tres skills que ya existen, y
llamarlos sin pasar por aquí es correcto.

---

## Alcance

- **El único archivo que este skill escribe es `specs/Extraction.md`.** Todo lo demás lo
  escriben los skills a los que delega, cada uno con sus propias reglas.
- **No publica en nuget.org ni empuja a ningún remoto.** Los pasos 6 y 9 son del usuario: son
  irreversibles y salen del equipo. Este skill los prepara, los explica y espera.
- **No encadena pasos por su cuenta.** Ejecuta el paso en el que esté, actualiza el estado y
  devuelve el control. El usuario decide cuándo seguir.

---

## Reglas generales
- El directorio de trabajo es la raíz del portafolio.
- `monorepoRoot`, `packagesRoot` y `sitePath` salen del frontmatter de `specs/Extraction.md`.
- **Actualiza `current` en el frontmatter en cuanto un paso quede hecho**, no al final de la
  sesión. Ese campo es lo que se lee mañana.
- Si falta un dato que impida completar la tarea, detente y pregunta.

---

## Los once pasos, y quién hace cada uno

La tabla completa, con el porqué de su orden, vive en `specs/Extraction.md`. Aquí, quién ejecuta:

| # | Paso | Quién |
|---|---|---|
| 1 | Elegir el candidato | Este skill, con `Get-ExtractionCandidates.ps1` |
| 2 | Extraer y crear el repositorio | Este skill, a mano |
| 3 | Homologar | Delegar en `homologate-nuget-package` |
| 4 | Mejoras opcionales | Delegar en `implement-nuget-package`, solo si el usuario lo pide |
| 5 | Retirar del monorepo y commitear | Este skill |
| 6 | Crear el remoto y empujar | **El usuario**, guiado paso a paso |
| 7 | Alta en el sitio | Delegar en `sync-package-pages` |
| 8 | Desplegar el sitio | **El usuario** |
| 9 | Publicar en nuget.org | **El usuario**, etiqueta `v*` |
| 10 | Añadir la fila de la versión y desplegar | Delegar en `sync-package-pages` |
| 11 | Refrescar el inventario | Delegar en `audit-nuget-packages` |

**El 8 va antes que el 9, siempre.** La metadata de nuget.org es inmutable por versión: publicar
antes de desplegar deja el `<PackageProjectUrl>` de esa versión apuntando a un 404 para siempre.

---

## Flujo de ejecución

### Paso 0 — Leer el estado

Lee `specs/Extraction.md`. Del frontmatter salen las tres rutas y `current`:

- **`current.package` vacío** → no hay nada en vuelo. Ve al Paso 1.
- **`current.package` con valor** → hay trabajo a medias. Dile al usuario **qué paquete y en qué
  paso**, y ofrece continuar desde ahí. No empieces otro.

Resume en dos líneas antes de proponer nada: qué se hizo, qué toca.

### Paso 1 — Elegir el candidato

Si la cola de `specs/Extraction.md` ya tiene un siguiente acordado, **respétalo**: se decidió con
datos y con el usuario delante.

Para recalcularla:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "{SKILL_DIR}/scripts/Get-ExtractionCandidates.ps1" `
    -monorepoRoot "{{monorepoRoot}}" -top 10
```

> `{SKILL_DIR}` es el directorio de este `SKILL.md`. Resuélvelo a ruta absoluta.

Sin `-skipBuild` compila cada proyecto para contar los `CS1591`, que es la mitad más cara del
coste y la que no se estima de otro modo. Sobre 78 proyectos son minutos: usa `-skipBuild` para
un barrido rápido y repite sin él antes de decidir.

| Columna | Qué significa |
|---|---|
| `usan` | Cuántos proyectos del monorepo lo consumen. Es el arrastre |
| `avisos` | `CS1591` a documentar. Es el coste dominante |
| `score` | `usan / (1 + avisos/10)`. Guía, no veredicto |
| `ROTO` | No compila. Arreglarlo antes de plantearse extraerlo |
| `espera a:` | Dependencias internas que siguen en el monorepo |

**`espera a:` no bloquea**: esas dependencias se consumen desde nuget.org. Solo avisa de que, si
se homologan después, este querrá fijar su versión nueva.

Propón el candidato con sus cifras y **espera confirmación** antes del Paso 2.

### Paso 2 — Extraer

Antes de mover, comprueba en el monorepo:

1. **Las versiones que el CPM fija** para sus `PackageReference`. En el monorepo,
   `Directory.Packages.props` hace también de `Directory.Build.props`: el TFM y el `LangVersion`
   viven ahí. Un proyecto sacado sin eso **no compila**.
2. **Si lo que acompaña al proyecto son pruebas de verdad o una app de verificación.** Míralo
   por el `Sdk=` del `.csproj`: `Microsoft.NET.Sdk.BlazorWebAssembly` o `.Web` es verificación y
   va a `samples/`; xUnit es prueba y va a `tests/`.

   > **Renómbralo al mover.** El monorepo los llama `.Tests` o `.Test` sin distinguir qué son;
   > el estándar usa dos nombres, y el nombre tiene que decir la verdad:
   >
   > | Qué es | Dónde va | Cómo se llama |
   > |---|---|---|
   > | App de verificación | `samples/` | `{{paquete}}.Sample` |
   > | Pruebas xUnit | `tests/` | `{{paquete}}.Tests` |
   >
   > Renombrar arrastra el directorio, el `.csproj`, el `.slnx` y **los namespaces de sus
   > archivos**: hazlo con `git mv` y un reemplazo sobre `.cs`, `.razor` y `.slnx`, y compila
   > antes de dar el paso por bueno. Es barato ahora y caro después de publicar.
   >
   > Importa porque `dotnet test` en CI solo significa algo si lo que hay en `tests/` son
   > pruebas: un verificador llamado `.Tests` hace que el paso pase en verde sin ejecutar nada.
3. **Quién lo consume por `ProjectReference`**, que serían los únicos que romperían al moverlo.

Después: copia el proyecto y sus acompañantes, **excluyendo `bin/`, `obj/` y `.csproj.user`**,
crea la estructura del estándar, y `git init`.

> **Los `.yml` no se copian sin adaptar.** Llevan dentro el nombre de la solución y del
> `.csproj`. El plan del Paso 3 lo detecta, pero es más barato hacerlo bien de entrada.

### Pasos 3 a 11 — Delegar y esperar

Invoca el skill que toca, deja que haga su trabajo y **actualiza `current.step`**.

En los pasos **6, 8 y 9** el trabajo es del usuario. Guíalo con los comandos exactos, recuérdale
lo que el paso implica, y **no los ejecutes tú**: publicar y empujar salen del equipo y no se
deshacen.

Al terminar el 11, vacía `current`, mueve el paquete a *Ya trasladados* y anota lo aprendido si
algo del procedimiento falló.

### Confirmación

Cada vez que este skill termina un paso, dice:

- Qué paso quedó hecho y **cuál es el siguiente**
- Qué archivos se tocaron, y en qué repositorio
- Qué está sin commitear, sin empujar y sin publicar
- Que `specs/Extraction.md` ya refleja el avance

**Aquí termina.** No sigas al paso siguiente sin que el usuario lo pida.
