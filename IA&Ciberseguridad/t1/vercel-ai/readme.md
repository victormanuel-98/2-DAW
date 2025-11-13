# Proyecto IA con Vercel AI SDK y Node.js

Este proyecto consiste en un **servidor Node.js** que integra la API de OpenAI mediante **Vercel AI SDK**, permitiendo enviar prompts y recibir texto generado dinámicamente.

---

## Tecnologías usadas

- **Node.js** (v21+)  
- **Express** (servidor web)  
- **dotenv** (gestión de variables de entorno, para la API Key)  
- **Vercel AI SDK** (`ai` y `@ai-sdk/openai`)  
- **Postman** (para probar la API)  
- **curl** (alternativa a Postman para pruebas desde terminal)

---

## Instalación

1. Clona o descarga el proyecto en tu máquina:

```bash
git clone <URL_DEL_REPO>
cd vercel-ai
```

2. Intalar dependencias:

```bash
npm install
```

3. Crear archivo .env con la clave de OpenAI

4. Ejecutar en la terminal

```bash
node index.js
```

## Pruebas

1. Prueba con #curl# mediante terminal de VScode

```bash
curl -X POST http://localhost:3000/chat \
-H "Content-Type: application/json" \
-d "{\"prompt\":\"Hola, IA, muéstrame un ejemplo de respuesta\"}"
```
## Imagen
![Terminal con curl](./pruebaCurl.png)

2. Prueba con #Postman#

```json
{
  "prompt": "Hola, IA, muéstrame un ejemplo de respuesta"
}
```
## Imagen
![Postman mostrando la interfaz](./pruebaPostman.png)

3. Prueba desde url

## Imagen
![URL desde servidor Localhost3000](./serverLocalhost.png)



