using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Models;
using WeatherForecastApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherForecastApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionController(WeatherForecastContext context) : ControllerBase
    {
        private readonly WeatherForecastContext _context = context;

        // GET: /Region
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Region>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get()
        {
            var books = await _context.Regions.ToListAsync();
            return Ok(books);
        }

        // GET: /Region/{id}
        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Region))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get(int id)
        {
            var region = await _context.Regions.FindAsync(id);
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
            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetById", new { id = region.Id }, region);
        }

        // PUT /Region
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Region region)
        {
            var currentRegion = await _context.Regions.FindAsync(region.Id);
            if (currentRegion is null) 
            {
                return NotFound();
            }

            _context.Entry(currentRegion).State = EntityState.Detached;
            _context.Entry(region).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE /Region/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var currentRegion = await _context.Regions.FindAsync(id);
            if (currentRegion is null)
            {
                return NotFound();
            }

            _context.Remove(currentRegion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
