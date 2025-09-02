using System.Data;

namespace WeatherForecastConsoleApp.Models;

internal static class WeatherForecastExtensions
{
    public static DataTable ToDataTable(this List<WeatherForecast> weatherForecasts, int regionId)
    {
        DataTable table = new();
        DataColumnCollection columns = table.Columns;
        columns.Add("RegionId", typeof(int));
        columns.Add("Date", typeof(DateTime));
        columns.Add("TemperatureC", typeof(int));
        columns.Add("Summary", typeof(string));

        DateTime currentDate = DateTime.Today;

        for (int i = 0; i < weatherForecasts.Count; i++)
        {
            var weatherForecast = weatherForecasts[i];
            var date = currentDate.AddDays(i + 1);

            var row = table.NewRow();
            row["RegionId"] = regionId;
            row["Date"] = date;
            row["TemperatureC"] = weatherForecast.TemperatureC;
            row["Summary"] = weatherForecast.Summary;

            table.Rows.Add(row);
        }

        return table;
    }
}