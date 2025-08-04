# WeatherGrabber

Простой сервис для сбора данных о погоде на .NET 8.0.

## Что делает этот проект?

Этот проект автоматически собирает температуру для трех городов (Москва, Волгоград, Благовещенск) каждые 30 минут и сохраняет данные в базу данных.

### Быстрый запуск:

1. **Дважды кликните на файл `01-start.bat`** - приложение запустится автоматически
2. **Откройте браузер и перейдите по адресу:** http://localhost:5000/swagger
3. **Для тестирования API кликните на `02-test-api.bat`**
4. **Для проверки требований кликните на `03-check-project.bat`**
   
### Что происходит при запуске:
- Останавливаются старые процессы (если есть)
- Запускается .NET приложение
- Открывается API на порту 5000
- Автоматически начинается сбор данных о погоде каждые 30 минут

### 1. Получить историю температур
```
GET http://localhost:5000/weather/Moscow
```
Возвращает последние 20 записей температуры для Москвы.

### 2. Получить последнюю температуру
```
GET http://localhost:5000/weather/Moscow/latest
```
Возвращает самую свежую запись температуры.

### Документация API
```
GET http://localhost:5000/swagger
```
Открывает интерактивную документацию API.

## Примеры запросов

```bash
# Получить историю для Москвы
curl http://localhost:5000/weather/Moscow

# Получить последнюю температуру
curl http://localhost:5000/weather/Moscow/latest
```

## Настройки

Все настройки находятся в файле `WeatherGrabber/appsettings.json`:

```json
{
  "WeatherGrabber": {
    "Cities": ["Moscow", "Volgograd", "Blagoveshchensk"],
    "ApiKey": "ваш_api_ключ_openweathermap",
    "PollIntervalMinutes": 30
  }
}
```

## Структура проекта

```
WeatherGrabber/
├── WeatherGrabber/           # Основной проект
│   ├── Program.cs            # API endpoints
│   ├── Worker.cs             # Фоновая служба
│   ├── Data/WeatherDbContext.cs
│   ├── Models/WeatherHistory.cs
│   └── appsettings.json      # Настройки
├── 01-start.bat              # Запуск приложения
├── 02-test-api.bat           # Тестирование API
├── 03-check-project.bat      # Проверка требований
└── README.md                 # Документация
```

## Технологии

- **.NET 8.0** - современная платформа
- **ASP.NET Core Minimal API** - простой API
- **Entity Framework Core** - работа с базой данных
- **SQLite** - легкая база данных
- **Background Service** - фоновая обработка
- **Swagger** - документация API
