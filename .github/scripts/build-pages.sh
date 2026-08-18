#!/usr/bin/env bash
#
# Genera un index.html por cada ruta del sitio, con su propio titulo y sus
# metadatos Open Graph.
#
# Dos motivos:
#
#   1. GitHub Pages sirve archivos, no rutas. Sin un archivo real, la peticion
#      cae en 404.html: el contenido se veria, pero con estado HTTP 404. Importa
#      en /UserServices.Abstractions, que es la URL que el paquete declara en su
#      <PackageProjectUrl>, y en las paginas de portafolio que se comparten.
#
#   2. Los rastreadores de vistas previas (LinkedIn, X, WhatsApp, Slack) no
#      ejecutan JavaScript: leen las etiquetas del HTML servido. Sin este paso,
#      todas las rutas compartirian el titulo y la descripcion de la portada.
#
set -euo pipefail

root="${1:?Uso: build-pages.sh <ruta del wwwroot publicado>}"
site="https://aldazsoft.github.io"

# ruta|titulo|descripcion
routes=(
  "packages|Paquetes|Librerías .NET publicadas en nuget.org, con su documentación y su canal de soporte."
  "UserServices.Abstractions|Persiltech.UserServices.Abstractions|El Output Port IUserService: estado de autenticación e identidad del usuario actual para soluciones con Arquitectura Limpia."
  "about|Trayectoria|Experiencia, tecnologías y forma de trabajar de Edinson Aldaz."
  "contact|Contacto|Soporte de los paquetes, consultoría y colaboración."
)

render() {
  local target="$1" title="$2" description="$3" url="$4"

  sed \
    -e "s#<title>[^<]*</title>#<title>${title}</title>#" \
    -e "s#\(<meta name=\"description\" content=\"\)[^\"]*#\1${description}#" \
    -e "s#\(<meta property=\"og:title\" content=\"\)[^\"]*#\1${title}#" \
    -e "s#\(<meta property=\"og:description\" content=\"\)[^\"]*#\1${description}#" \
    -e "s#\(<meta property=\"og:url\" content=\"\)[^\"]*#\1${url}#" \
    "${root}/index.html" > "${target}"
}

for entry in "${routes[@]}"; do
  IFS='|' read -r route title description <<< "${entry}"

  mkdir -p "${root}/${route}"
  render "${root}/${route}/index.html" "${title}" "${description}" "${site}/${route}/"

  echo "  ${route}/index.html"
done

# Red de seguridad para cualquier ruta no listada: Pages sirve 404.html, y al
# ser copia de index.html el enrutador de Blazor la resuelve en el cliente.
cp "${root}/index.html" "${root}/404.html"
echo "  404.html"
