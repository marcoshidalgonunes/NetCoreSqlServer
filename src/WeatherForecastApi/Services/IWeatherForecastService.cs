using WeatherForecastApi.Models;

namespace WeatherForecastApi.Services;

public interface IWeatherForecastService
{
    Task<bool> CreateAsync(int regionId, List<WeatherForecast> weatherForecasts);

    Task<List<WeatherForecast>?> ReadAsync(int regionId);
}
