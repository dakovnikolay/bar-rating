using BarRating.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BarRating.Services.Interfaces;

public interface IBarService
{
    Task<List<Bar>> GetAllBarsAsync();
    Task<Bar> GetBarByIdAsync(int barId);
    Task<IdentityResult> CreateBarAsync(Bar bar);
    Task<IdentityResult> UpdateBarAsync(Bar bar);
    Task<IdentityResult> DeleteBarAsync(int barId);
    Task<int> GetBarCountAsync();
}
