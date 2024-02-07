using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WeatherForecastConsoleApp.Models;

namespace WeatherForecastConsoleApp.Services;

internal sealed class WeatherForecastService(IConfigurationRoot config)
{
    private readonly IConfigurationRoot _config = config;

    private static readonly string[] _summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

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

    internal static async Task<List<WeatherForecast>?> ReadFileAsync(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        List<WeatherForecast>? forecasts = await JsonSerializer.DeserializeAsync<List<WeatherForecast>>(stream, options);
        if (forecasts?.Count > 0)
        {
            var duplicates = forecasts.GroupBy(f => f.Date)
                .Any(g => g.Count() > 1);
            if (duplicates)
            {
                throw new InvalidOperationException("There are duplicates for daily forecasts");
            }

            var invalidSummaries = forecasts.Where(f => !_summaries.Contains(f.Summary)).Select(f => f.Summary).ToArray();
            if (invalidSummaries.Length > 0)
            {
                string wrongSummaries = string.Join(',', [.. invalidSummaries.Distinct().OrderBy(s => s)]);
                throw new InvalidOperationException($"One or more summaries are invalid: '{wrongSummaries}'");
            }
        }

        return forecasts;
    }
}
