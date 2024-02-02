using Microsoft.AspNetCore.Mvc;
using WeatherForecastApi.Models;
using WeatherForecastApi.Services;

namespace WeatherForecastApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IWeatherForecastService service) : ControllerBase
{
    private readonly IWeatherForecastService _service = service;

    // GET: /WeatherForecast/{regionId}
    [HttpGet("{regionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WeatherForecast>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get(int regionId)
    {
        var weatherForecasts = await _service.ReadAsync(regionId);
        if (weatherForecasts is null) 
        {
            return NotFound();
        }
        
        return Ok(weatherForecasts);
    }

    // POST: /WeatherForecast/{regionId}
    [HttpPost("{regionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(int regionId, List<WeatherForecast> weatherForecasts)
    {
        if (!await _service.CreateAsync(regionId, weatherForecasts))
        {
            return BadRequest();
        }

        return Ok();
    }
}
