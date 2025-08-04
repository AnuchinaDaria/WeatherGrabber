using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WeatherGrabber.Data;
using WeatherGrabber.Models;

namespace WeatherGrabber;

/// <summary>
/// Конфигурация приложения из appsettings.json
/// </summary>
public class WeatherGrabberOptions
{
    public string[] Cities { get; set; } = Array.Empty<string>();
    public string ApiKey { get; set; } = string.Empty;
    public int PollIntervalMinutes { get; set; } = 30;
}

/// <summary>
/// Фоновая служба для автоматического сбора данных о погоде
/// </summary>
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;
    private readonly WeatherDbContext _context;
    private readonly WeatherGrabberOptions _options;

    public Worker(
        ILogger<Worker> logger,
        HttpClient httpClient,
        WeatherDbContext context,
        IOptions<WeatherGrabberOptions> options)
    {
        _logger = logger;
        _httpClient = httpClient;
        _context = context;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Weather Worker started. Interval: {Interval} minutes", _options.PollIntervalMinutes);
        _logger.LogInformation("Cities to monitor: {Cities}", string.Join(", ", _options.Cities));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting weather data collection cycle at {Time}", DateTime.UtcNow);
                await FetchWeatherDataForAllCities();
                _logger.LogInformation("Weather data collection cycle completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching weather data");
            }

            _logger.LogInformation("Waiting {Minutes} minutes until next collection...", _options.PollIntervalMinutes);
            await Task.Delay(TimeSpan.FromMinutes(_options.PollIntervalMinutes), stoppingToken);
        }
    }

    /// <summary>
    /// Сбор данных о погоде для всех городов (последовательно)
    /// </summary>
    private async Task FetchWeatherDataForAllCities()
    {
        foreach (var city in _options.Cities)
        {
            try
            {
                await FetchWeatherDataForCity(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data for city: {City}", city);
            }
        }
    }

    /// <summary>
    /// Сбор данных о погоде для конкретного города
    /// </summary>
    /// <param name="city">Название города</param>
    private async Task FetchWeatherDataForCity(string city)
    {
        try
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_options.ApiKey}&units=metric";
            
            _logger.LogDebug("Fetching weather data for {City} from OpenWeatherMap API", city);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("API Response for {City}: {JsonResponse}", city, jsonString);
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var weatherData = JsonSerializer.Deserialize<WeatherApiResponse>(jsonString, options);

            if (weatherData?.Main?.Temp == null)
            {
                _logger.LogWarning("Invalid weather data received for city: {City}. WeatherData: {@WeatherData}", city, weatherData);
                return;
            }

            var timestamp = DateTime.UtcNow;
            var temperature = weatherData.Main.Temp.Value;

            // Проверяем, есть ли уже запись за последние 30 секунд
            var existingRecord = await _context.WeatherHistory
                .Where(w => w.City == city && 
                           w.TimestampUtc >= timestamp.AddSeconds(-30))
                .FirstOrDefaultAsync();

            if (existingRecord != null)
            {
                _logger.LogDebug("Skipping duplicate record for {City} at {Time}", city, timestamp);
                return;
            }

            var weatherRecord = new WeatherHistory
            {
                City = city,
                Temperature = temperature,
                TimestampUtc = timestamp
            };

            _context.WeatherHistory.Add(weatherRecord);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Saved weather data for {City}: {Temperature}°C at {Time}", 
                city, temperature, timestamp);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error while fetching weather data for {City}", city);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for {City}", city);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while processing weather data for {City}", city);
        }
    }
}

/// <summary>
/// Модель ответа от OpenWeatherMap API
/// </summary>
public class WeatherApiResponse
{
    [JsonPropertyName("main")]
    public MainData? Main { get; set; }
}

/// <summary>
/// Основные данные о погоде
/// </summary>
public class MainData
{
    [JsonPropertyName("temp")]
    public double? Temp { get; set; }
}
