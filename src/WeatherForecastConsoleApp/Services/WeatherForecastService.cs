using System.Text.Json;
using WeatherForecastConsoleApp.Models;

namespace WeatherForecastConsoleApp.Services;

internal sealed class WeatherForecastService
{

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
