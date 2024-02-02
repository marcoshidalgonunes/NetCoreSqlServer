using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Models;
using WeatherForecastApi.Repositories;

namespace WeatherForecastApi.Services;

public class WeatherForecastService(WeatherForecastContext context) : IWeatherForecastService
{
    private readonly WeatherForecastContext _context = context;

    public async Task<bool> CreateAsync(int regionId, List<WeatherForecast> weatherForecasts)
    {
        if (!await RegionExistsAsync(regionId))
        {
            return false;
        }

        var firstForecast = weatherForecasts.Min(f => f.Date);
        if (firstForecast <= DateOnly.FromDateTime(DateTime.Now))
        {
            return false;
        }

        var duplicates = weatherForecasts.GroupBy(f => f.Date)
            .Any(g => g.Count() > 1);
        if (duplicates)
        {
            return false;
        }

        SqlParameter tableParameter = new() {
            SqlDbType = SqlDbType.Structured,
            Direction = ParameterDirection.Input,
            ParameterName = "listIds",
            TypeName = "[dbo].[Forecasts]", 
            Value = weatherForecasts.ToDataTable()
        };

        await _context.Database.ExecuteSqlAsync($"EXECUTE CreateForecasts {regionId}, {tableParameter}");
        return true;
    }

    public async Task<List<WeatherForecast>?> ReadAsync(int regionId)
    {
        if (!await RegionExistsAsync(regionId))
        {
            return null;
        }

        return await _context.WeatherForecasts.FromSql($"EXECUTE ReadForecasts {regionId}").ToListAsync();
    }

    private async Task<bool> RegionExistsAsync(int regionId)
    {
        var region = await _context.Regions.FindAsync(regionId);
        return region != null;
    }
}
