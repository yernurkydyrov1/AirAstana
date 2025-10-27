
# AirAstana API(Тестовое задание)

AirAstana API — это backend-сервис для управления рейсами и пользователями авиакомпании, реализованный на .NET 6, с использованием Entity Framework Core, MediatR, JWT-аутентификации и PostgreSQL.

---

## Основные возможности

- Регистрация и аутентификация пользователей с ролями (User, Moderator)
- Управление рейсами: создание, обновление статуса, фильтрация
- Кэширование данных рейсов с помощью IMemoryCache
- Централизованная валидация команд и запросов через Pipeline Behaviors
- Логирование всех действий API

---

## Технологии

- .NET 6
- ASP.NET Core Web API
- Entity Framework Core 6
- PostgreSQL
- MediatR (CQRS-паттерн)
- JWT для аутентификации
- Swashbuckle / Swagger для документации
- IMemoryCache для кеширования
- xUnit & Moq для юнит-тестов

---

## Структура проекта

```
AirAstana/
│
├─ AirAstana.Presentation/      # Контроллеры API, настройка маршрутов, авторизации
├─ AirAstana.Application/       # Команды, запросы, DTO, сервисы, бизнес-логика
├─ AirAstana.Domain/            # Сущности и доменные объекты
├─ AirAstana.Infrastructure/    # Репозитории, DbContext, внешние сервисы
├─ AirAstana.Tests/             # Unit-тесты
└─ README.md
```

---

## Настройка проекта

1. Клонировать репозиторий

```bash
git clone https://github.com/yernurkydyrov1/AirAstana
cd AirAstana
```

2. Настроить PostgreSQL

Создайте базу данных, например `AirAstana`.  

Пример подключения в `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=AirAstana;Username=postgres;Password=12345"
}
```

3. JWT-настройки

```json
"JwtSettings": {
  "Key": "supersecretkey_airastana_12345678",
  "Issuer": "AirAstana",
  "Audience": "AirAstanaUsers",
  "ExpiresInMinutes": 60
}
```

> ⚠️ Обязательно используйте ключ длиной не менее 32 символов для алгоритма HS256.

4. Применить миграции

```bash
dotnet ef database update --project AirAstana.Infrastructure --startup-project AirAstana.Presentation
```

---

## Запуск проекта

```bash
dotnet run --project AirAstana.Presentation
```

Swagger доступен по адресу: `http://localhost:5000/swagger`

---

## Примеры запросов

Регистрация пользователя:

```http
POST /api/Auth/register
Content-Type: application/json

{
  "username": "user1",
  "password": "Password123!",
  "roleId": 1
}
```

Получение рейсов с фильтром:

```http
GET /api/Flights?origin=ALA&destination=AST
Authorization: Bearer <JWT_TOKEN>
```

Создание рейса (только для Moderator):

```http
POST /api/Flights
Authorization: Bearer <JWT_TOKEN>
Content-Type: application/json

{
  "origin": "ALA",
  "destination": "AST",
  "departureTime": "2025-11-01T12:00:00",
  "arrivalTime": "2025-11-01T15:00:00",
  "status": 1
}
```

---

## Тестирование

Для запуска всех тестов:

```bash
dotnet test
```

> Используются xUnit и Moq для юнит-тестирования сервисов и репозиториев.

---

## Автор

- Yernur Kydyrov  
- Email: yernurkydyrov@gmail.com
- GitHub: https://github.com/yernurkydyrov1

---

## Лицензия

MIT License © 2025
