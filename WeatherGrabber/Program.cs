using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WeatherGrabber;
using WeatherGrabber.Data;
using WeatherGrabber.Models;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация базы данных
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlite("Data Source=weather.db"), ServiceLifetime.Singleton);

// HTTP клиент для внешних API
builder.Services.AddHttpClient();

// Конфигурация из appsettings.json
builder.Services.Configure<WeatherGrabberOptions>(
    builder.Configuration.GetSection("WeatherGrabber"));

// Фоновая служба для сбора данных
builder.Services.AddHostedService<Worker>();

// Swagger документация
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WeatherGrabber API",
        Version = "v1",
        Description = "Современный API для сбора и получения данных о погоде",
        Contact = new OpenApiContact
        {
            Name = "WeatherGrabber",
            Email = "dev@weathergrabber.com"
        }
    });
});

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
    context.Database.EnsureCreated();
}

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherGrabber API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "WeatherGrabber API Documentation";
});

// API endpoints
app.MapGet("/weather/{city}", async (string city, WeatherDbContext context) =>
{
    var weatherHistory = await context.WeatherHistory
        .Where(w => w.City == city)
        .OrderByDescending(w => w.TimestampUtc)
        .Take(20)
        .ToListAsync();

    return Results.Ok(weatherHistory);
})
.WithName("GetWeatherHistory")
.WithSummary("Получить последние 20 записей температуры для города")
.WithDescription("Возвращает историю температур для указанного города. Данные автоматически собираются каждые 30 минут.");

app.MapGet("/weather/{city}/latest", async (string city, WeatherDbContext context) =>
{
    var latestWeather = await context.WeatherHistory
        .Where(w => w.City == city)
        .OrderByDescending(w => w.TimestampUtc)
        .FirstOrDefaultAsync();

    if (latestWeather == null)
    {
        return Results.NotFound($"Данные для города {city} не найдены");
    }

    return Results.Ok(latestWeather);
})
.WithName("GetLatestWeather")
.WithSummary("Получить последнюю запись температуры для города")
.WithDescription("Возвращает самую свежую запись температуры для указанного города.");

app.Run();
