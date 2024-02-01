using System.Net;

namespace WeatherForecastApi.Models;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
