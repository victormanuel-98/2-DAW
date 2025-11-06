# Imagen base de Node.js
FROM node:latest

# Crear y usar una carpeta de trabajo
WORKDIR /app

# Copiar los archivos necesarios al contenedor
COPY server.js .

# Instalar Express
RUN npm init -y && npm install express

# Exponer el puerto 3000
EXPOSE 3000

# Comando para ejecutar el servidor
CMD ["node", "server.js"]
