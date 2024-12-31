FROM node:18-alpine AS build
WORKDIR /app
COPY ./client/package.json ./client/package-lock.json ./
RUN npm install
COPY ./client ./
RUN npm run build --prod

FROM nginx:alpine
COPY --from=build /app/dist/client/browser /usr/share/nginx/html/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
