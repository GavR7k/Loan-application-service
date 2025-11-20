# Loan-application-service

## Структура проекта
- **infra/**  
  Содержит `docker-compose.yml` для запуска всех сервисов (PostgreSQL, backend, frontend).
  Содержит также `docker-compose.hub.yml` для запуска из DockerHub
  
- **frontend/**  
  Содержит `Dockerfile` для сборки фронтенда, `nginx.conf` для настройки Nginx и сам проект на Vue.
- **backend/**  
  Содержит `Dockerfile` для сборки и запуска .NET 8 Web API.
- **PostgreSQL**  
  Настраивается и запускается через `docker-compose.yml` (используется официальный образ `postgres:15`).



