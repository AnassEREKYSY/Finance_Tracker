# Build phase
FROM node:18-alpine AS build

WORKDIR /app

COPY ./client/package.json ./client/package-lock.json ./

RUN npm install

COPY ./client ./

RUN npm run build --prod

# Serve phase
FROM nginx:alpine

# Remove default nginx config to avoid conflict
RUN rm /etc/nginx/conf.d/default.conf

# Copy custom nginx config (optional, you can skip this if no custom config is needed)
# COPY ./client/nginx.conf /etc/nginx/nginx.conf
# Copy custom nginx config
COPY ./client/nginx.conf /etc/nginx/conf.d/default.conf
# Copy the Angular build output from the build phase to NGINX's serving folder
COPY --from=build /app/dist/client/browser /usr/share/nginx/html/

# Expose port 80 to serve the application
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
