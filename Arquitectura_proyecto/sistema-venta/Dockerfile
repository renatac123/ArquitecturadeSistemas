# Primera etapa de construcción
FROM node:14 AS build-step

# Crear el directorio de trabajo
RUN mkdir -p /app

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar el archivo de dependencias
COPY package.json /app

# Instalar las dependencias
RUN npm install

# Copiar el resto de la aplicación
COPY . /app

# Construir la aplicación Angular
RUN npm run build --prod

# Segunda etapa: Servir la aplicación con Nginx
FROM nginx:1.17.1-alpine

# Copiar los archivos compilados a la carpeta de Nginx
COPY --from=build-step /app/dist/sistema-venta /usr/share/nginx/html
