using System.CommandLine;
using Microsoft.Extensions.Configuration;
using WeatherForecastConsoleApp.Models;
using WeatherForecastConsoleApp.Services;

namespace WeatherForecastConsoleApp;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        var regionOption = new Option<int>(
            name: "--regionId",
            description: "The identifier of region of forecasts to be loaded.",
            parseArgument: result =>
            {
                return int.TryParse(result.Tokens.Single().Value, out int value) ? value : 0;

            })
        {
            IsRequired = true
        };

        var fileOption = new Option<string>(
            name: "--jsonFile",
            description: "The Json file to read and load data into WeatherForecast SQL Server database.",
            parseArgument: result =>
            {
                string? filePath = result.Tokens.Single().Value;
                if (!File.Exists(filePath))
                {
                    result.ErrorMessage = $"File '{filePath}' does not exist";
                    return string.Empty;
                }
                else
                {
                    return filePath;
                }
            })
        {
            IsRequired = true
        };

        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        RootCommand rootCommand = new("App to load forecasts from Json file to SQL Server database via bulk insert");
        rootCommand.AddOption(regionOption);
        rootCommand.AddOption(fileOption);

        rootCommand.SetHandler(async (regionId, filepath) => {
            if (regionId > 0)
            {
                List<WeatherForecast>? forecasts = await WeatherForecastService.ReadFileAsync(filepath);
                if (forecasts?.Count > 0)
                {
                    var repository = new WeatherForecastService(config);
                    await repository.BulkInsertAsync(regionId, forecasts);
                }
                else
                {
                    Console.WriteLine($"Invalid forecasts file\r\n({filepath})");
                }
            }
            else
            {
                Console.WriteLine("RegionId must be greater than 0");
            }
        }, regionOption, fileOption);

        try
        {
            return await rootCommand.InvokeAsync(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return -1;
        }
    }
}
