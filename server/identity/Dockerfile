FROM node:lts-alpine AS development
WORKDIR /app/identity
COPY package*.json ./
RUN npm install glob rimraf
RUN npm install --only=development
COPY . .
RUN npm run build

FROM node:lts-alpine as production
ARG NODE_ENV=production
ENV NODE_ENV=${NODE_ENV}
WORKDIR /app/identity
COPY package*.json ./
RUN npm install --only=production
COPY . .
COPY --from=development /app/identity/dist ./dist
CMD ["node", "dist/main"]