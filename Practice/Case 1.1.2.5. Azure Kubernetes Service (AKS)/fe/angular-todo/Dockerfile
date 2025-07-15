FROM node:20-alpine AS builder
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
ARG CONFIG=production
RUN npm run build -- --configuration=$CONFIG

FROM nginx:stable-alpine
RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /app/dist/angular-todo/browser /usr/share/nginx/html/web

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
