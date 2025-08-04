namespace WeatherGrabber.Models;

// данные о погоде для города
public class WeatherHistory
{
    public int Id { get; set; }
    public string City { get; set; } = null!;
    public double Temperature { get; set; }
    public DateTime TimestampUtc { get; set; }
}
