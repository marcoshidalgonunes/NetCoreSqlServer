using System.Data;
using Microsoft.Data.SqlClient.Server;

namespace WeatherForecastApi.Models;

public static class WeatherForecastExtensions
{
    private static readonly SqlMetaData[] _schema = [
        new("Date", SqlDbType.Date),
        new("TemperatureC", SqlDbType.Int),
        new("Summary", SqlDbType.NVarChar, 255),
    ];

    public static List<SqlDataRecord> ToDataTable(this List<WeatherForecast> weatherForecasts)
    {
        List<SqlDataRecord> records = [];
        for (int i = 0; i < weatherForecasts.Count; i++)
        {
            var weatherForecast = weatherForecasts[i];

            var row = new SqlDataRecord(_schema);
            row.SetDateTime(0, weatherForecast.Date.ToDateTime(TimeOnly.MinValue));
            row.SetInt32(1, weatherForecast.TemperatureC);
            row.SetSqlString(2, weatherForecast.Summary);

            records.Add(row);
        }

        return records;
    }
}
