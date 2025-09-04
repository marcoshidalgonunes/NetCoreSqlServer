using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WeatherForecastConsoleApp.Models;

namespace WeatherForecastConsoleApp.Repositories;

internal class WeatherForecastRepository(IConfigurationRoot config)
{
    private readonly IConfigurationRoot _config = config;

    internal async Task BulkInsertAsync(int regionId, List<WeatherForecast> weatherForecasts)
    {
        using SqlBulkCopy copy = new(_config.GetConnectionString("DefaultConnection"));

        copy.DestinationTableName = "dbo.RegionForecasts";
        copy.ColumnMappings.Add("RegionId", "RegionId");
        copy.ColumnMappings.Add(nameof(WeatherForecast.Date), "[Date]");
        copy.ColumnMappings.Add(nameof(WeatherForecast.TemperatureC), "TemperatureC");
        copy.ColumnMappings.Add(nameof(WeatherForecast.Summary), "Summary");

        await copy.WriteToServerAsync(weatherForecasts.ToDataTable(regionId));
    }
}
