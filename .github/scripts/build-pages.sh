#!/usr/bin/env bash
#
# Genera un index.html por cada ruta del sitio —con su titulo, sus metadatos
# Open Graph y su canonical— mas el sitemap.xml y el robots.txt.
#
# Tres motivos:
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
#   3. Blazor pinta la navegacion en el cliente, asi que el HTML servido no
#      contiene ni un solo enlace navegable. Un rastreador que no ejecute
#      JavaScript no tiene por donde descubrir /about, /contact, /packages ni
#      la pagina del paquete: el sitemap es su unica via de entrada.
#
# Al anadir una ruta al sitio basta con anadirla a 'routes': las paginas, el
# sitemap y el robots salen de la misma lista.
#
set -euo pipefail

root="${1:?Uso: build-pages.sh <ruta del wwwroot publicado>}"
site="https://aldazsoft.github.io"

# La fecha del ultimo commit representa cuando cambio el contenido de verdad.
# Usar la fecha de compilacion marcaria todo como modificado en cada despliegue.
lastmod="$(git log -1 --format=%cs 2>/dev/null || date -u +%F)"

# ruta|titulo|descripcion
routes=(
  "packages|Paquetes|Librerías .NET publicadas en nuget.org, con su documentación y su canal de soporte."
  "UserServices.Abstractions|Persiltech.UserServices.Abstractions|El Output Port IUserService: estado de autenticación e identidad del usuario actual para soluciones con Arquitectura Limpia."
  "UserServices|Persiltech.UserServices|El adaptador de ASP.NET Core para IUserService: resuelve la identidad y el estado de autenticación desde HttpContext.User."
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
    -e "s#\(<link rel=\"canonical\" href=\"\)[^\"]*#\1${url}#" \
    "${root}/index.html" > "${target}"
}

# La portada se sirve desde el index.html de origen, que ya trae los metadatos
# correctos; aqui solo se anade al sitemap.
sitemap_entries="  <url>
    <loc>${site}/</loc>
    <lastmod>${lastmod}</lastmod>
  </url>"

for entry in "${routes[@]}"; do
  IFS='|' read -r route title description <<< "${entry}"

  # Pages redirige /ruta a /ruta/ con un 301, asi que la forma canonica y la
  # que se declara en el sitemap es la que lleva barra final.
  url="${site}/${route}/"

  mkdir -p "${root}/${route}"
  render "${root}/${route}/index.html" "${title}" "${description}" "${url}"

  sitemap_entries="${sitemap_entries}
  <url>
    <loc>${url}</loc>
    <lastmod>${lastmod}</lastmod>
  </url>"

  echo "  ${route}/index.html"
done

cat > "${root}/sitemap.xml" <<XML
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
${sitemap_entries}
</urlset>
XML
echo "  sitemap.xml"

cat > "${root}/robots.txt" <<TXT
User-agent: *
Allow: /

Sitemap: ${site}/sitemap.xml
TXT
echo "  robots.txt"

# Red de seguridad para cualquier ruta no listada: Pages sirve 404.html, y al
# ser copia de index.html el enrutador de Blazor la resuelve en el cliente.
cp "${root}/index.html" "${root}/404.html"
echo "  404.html"
