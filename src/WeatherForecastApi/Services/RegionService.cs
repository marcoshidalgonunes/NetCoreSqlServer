using Microsoft.EntityFrameworkCore;
using WeatherForecastApi.Models;

namespace WeatherForecastApi.Repositories;

public class RegionService(WeatherForecastContext context)
{
    private readonly WeatherForecastContext _context = context;

    public async Task<Region> CreateAsync(Region region)
    {
        var newRegion = new Region { Name = region.Name };

        _context.Regions.Add(newRegion);
        await _context.SaveChangesAsync();

        return newRegion;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var currentRegion = await _context.Regions.FindAsync(id);
        if (currentRegion is null)
        {
            return false;
        }

        _context.Remove(currentRegion);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Region>> ReadAllAsync()
    {
        return await _context.Regions.ToListAsync();
    }

    public async Task<Region?> ReadByIdAsync(int id)
    {
        return await _context.Regions.FindAsync(id);
    }

    public async Task<bool> UpdateAsync(Region region)
    {
        var currentRegion = await _context.Regions.FindAsync(region.Id);
        if (currentRegion is null)
        {
            return false;
        }

        _context.Entry(currentRegion).State = EntityState.Detached;
        _context.Entry(region).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }
}
