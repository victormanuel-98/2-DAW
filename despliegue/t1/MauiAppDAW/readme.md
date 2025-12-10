# MauiAppDAW

Aplicación .NET MAUI (net9.0) que consume la API pública de SWAPI y muestra personajes, naves y planetas, con un historial local de búsquedas.

## Resumen
- Cliente .NET MAUI con UI temática (Star Wars).
- Búsqueda de `Personajes`, `Naves`, `Planetas`.
- Historial local por categoría (guardado en `LocalApplicationData/search_history.json`).
- API intermedia implementada en Node/Express (localmente en VSCode / desplegada en Azure for Students) que puede proxyar imágenes y exponer endpoints.

## Características principales
- Buscar y mostrar detalles de personajes.
- Guardar resultados en historial por categoría (Personajes / Naves / Planetas).
- Enriquecimiento:
  - `Starship`: nombre, modelo, fabricante, mundo y propietario.
  - `Planet`: nombre, lista de residentes famosos.

## Estructura (destacado)
- `Services/SwapiService.cs` — Lógica de consumo de SWAPI, selección y verificación de imágenes, enriquecimiento.
- `Services/SearchHistoryService.cs` — Guardado/lectura de historial (JSON).
- `Views/*` — Páginas MAUI (Main, Starships, Planets, History, etc.).
- `Models/*` — Modelos `Character`, `Starship`, `Planet`.
- `Resources/Styles/*` — Colores y estilos MAUI.
- `index.js` (API intermedia) — Express proxy.

## Despliegue y ejecución

### Requisitos
- .NET 9 / .NET MAUI (Visual Studio con workload MAUI) para la app.
- Node.js para la API intermedia (si se usa localmente).
- Cuenta de Azure (Azure for Students disponible para despliegues).

### Ejecutar la API intermedia (local, VS Code)
1. Desde la carpeta de la API en VS Code:
   - `npm install` (si procede)
   - `node index.js`
2. Por defecto escucha en `http://0.0.0.0:3000`.
3. Endpoints útiles:
   - `/characters?name=...` — proxy a SWAPI (JSON).
   - `/images/:slug` — sirve imágenes locales o proxyea `starwars-databank` (según `index.js`).

> Nota para emulador Android: en el emulador use `http://10.0.2.2:3000` para acceder a la API que corre en el host.

### Ejecutar la app MAUI
- Abrir solución en Visual Studio (con workload MAUI).
- Build → Rebuild Solution.
- Seleccionar emulador/dispositivo y Run.

Opcional (CLI):
- `dotnet build`
- Lanzar desde Visual Studio para la experiencia completa MAUI.

# Imágenes del Proyecto

## API en Azure
![API desde Azure](./imagenes/APIAzure.png)

## Despliegue de la API desde Visual Studio Code
![API en VSCode](./imagenes/APIvscodeScreen.png)

## Estructura de carpetas en Visual Studio Code
![Tree en vscode](./imagenes/APItree)

## Gif con el recorrido de la API desde Visaul Studio 2022 con SWApi
![Recorrido API Star Wars](./imagenes/APIRecorrido.gif)
