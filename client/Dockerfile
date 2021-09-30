# build
FROM node:lts-alpine as build
WORKDIR /app/web
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# production
FROM nginx:stable-alpine as production
COPY --from=build /app/web/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]