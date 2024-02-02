using System.Data;
using Microsoft.Data.SqlClient.Server;

namespace WeatherForecastApi.Models;

public static class WeatherForecastExtensions
{
    private static readonly SqlMetaData[] _weatherForecastSchema = CreateSchema();

    private static SqlMetaData[] CreateSchema()
    {
        return
        [
            new("Date", SqlDbType.Date),
            new("TemperatureC", SqlDbType.Int),
            new("Summary", SqlDbType.NVarChar, 255),
        ];
    }

    public static List<SqlDataRecord> ToDataTable(this List<WeatherForecast> weatherForecasts)
    {
        List<SqlDataRecord> weatherForecastRecords = new();
        for (int i = 0; i < weatherForecasts.Count; i++)
        {
            var weatherForecast = weatherForecasts[i];

            var weatherForecastRow = new SqlDataRecord(_weatherForecastSchema);
            weatherForecastRow.SetDateTime(0, weatherForecast.Date.ToDateTime(TimeOnly.MinValue));
            weatherForecastRow.SetInt32(1, weatherForecast.TemperatureC);
            weatherForecastRow.SetSqlString(2, weatherForecast.Summary);

            weatherForecastRecords.Add(weatherForecastRow);
        }

        return weatherForecastRecords;
    }
}
