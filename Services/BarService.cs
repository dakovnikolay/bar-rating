using BarRating.Data;
using BarRating.Data.Entities;
using BarRating.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BarRating.Services;

public class BarService : IBarService
{
    private readonly ApplicationDbContext _context;

    public BarService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Bar>> GetAllBarsAsync()
    {
        return await _context.Bars.ToListAsync();
    }

    public async Task<Bar> GetBarByIdAsync(int barId)
    {
        return await _context.Bars.FirstOrDefaultAsync(b => b.Id == barId);
    }

    public async Task<IdentityResult> CreateBarAsync(Bar bar)
    {
        try
        {
            _context.Bars.Add(bar);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }

    public async Task<IdentityResult> UpdateBarAsync(Bar bar)
    {
        try
        {
            _context.Bars.Update(bar);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(new IdentityError { Description = ex.Message });
        }
    }

    public async Task<IdentityResult> DeleteBarAsync(int barId)
    {
        var bar = await _context.Bars.FindAsync(barId);
        if (bar != null)
        {
            _context.Bars.Remove(bar);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        return IdentityResult.Failed(new IdentityError { Description = "Bar not found." });
    }
}
