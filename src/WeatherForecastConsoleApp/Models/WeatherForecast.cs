namespace WeatherForecastConsoleApp.Models;

public sealed class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public required string Summary { get; set; }
}