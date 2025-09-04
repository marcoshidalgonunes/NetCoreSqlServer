using Microsoft.AspNetCore.Mvc;
using WeatherForecastApi.Models;
using WeatherForecastApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherForecastApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RegionController(RegionService service) : ControllerBase
{
    private readonly RegionService _service = service;

    // GET: /Region
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Region>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get()
    {
        var regions = await _service.ReadAllAsync();
        if (regions.Count == 0)
        {
            return NotFound();
        }

        return Ok(regions);
    }

    // GET: /Region/{id}
    [HttpGet("{id}", Name = "GetById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Region))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Get(int id)
    {
        var region = await _service.ReadByIdAsync(id);
        if (region is null)
        {
            return NotFound();                
        }

        return Ok(region);
    }

    // POST /Region
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(Region region)
    {
        Region newRegion = await _service.CreateAsync(region);

        return CreatedAtRoute("GetById", new { id = newRegion.Id }, newRegion);
    }

    // PUT /Region
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Put(Region region)
    {
        if (!await _service.UpdateAsync(region)) 
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE /Region/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _service.DeleteAsync(id))
        {
            return NotFound();
        }

        return NoContent();
    }
}
