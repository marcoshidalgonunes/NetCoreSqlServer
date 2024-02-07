using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Models;

namespace WeatherForecastApi.Repositories;

public class WeatherForecastContext : DbContext
{
    public WeatherForecastContext(DbContextOptions<WeatherForecastContext> options) : base(options) { }

    public DbSet<Region> Regions { get; set; }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}
