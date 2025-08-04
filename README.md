# WeatherGrabber

Простой сервис для сбора данных о погоде на .NET 8.0.

## Что делает этот проект?

Этот проект автоматически собирает температуру для трех городов (Москва, Волгоград, Благовещенск) каждые 30 минут и сохраняет данные в базу данных.

## Как запустить?

1. Откройте папку WeatherGrabber
2. Выполните команду: `dotnet run`
3. Откройте браузер: http://localhost:5000/swagger

## API Endpoints (по ТЗ)

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
├── .gitignore                # Исключения для Git
├── README.md                 # Эта документация
└── WeatherGrabber.sln        # Файл решения
```

## Технологии

- **.NET 8.0** - современная платформа
- **ASP.NET Core Minimal API** - простой API
- **Entity Framework Core** - работа с базой данных
- **SQLite** - легкая база данных
- **Background Service** - фоновая обработка
- **Swagger** - документация API

## Загрузка в GitHub

### Пошаговые действия:

1. **Инициализируйте Git:**
   ```bash
   git init
   ```

2. **Добавьте файлы:**
   ```bash
   git add .
   ```

3. **Создайте первый коммит:**
   ```bash
   git commit -m "Initial commit: WeatherGrabber .NET project"
   ```

4. **Создайте репозиторий на GitHub:**
   - Зайдите на https://github.com
   - Нажмите "New repository"
   - Назовите: `WeatherGrabber`
   - Оставьте публичным
   - НЕ добавляйте README, .gitignore или license

5. **Подключите к GitHub:**
   ```bash
   git remote add origin https://github.com/YOUR_USERNAME/WeatherGrabber.git
   git push -u origin main
   ```

Готово! Проект загружен в GitHub.

