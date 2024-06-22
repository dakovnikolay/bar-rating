
using BarRating.Data.Entities;
using BarRating.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BarRating.Services;

public class RoleService : IRoleService
{
    public async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        string[] roleNames = { "Admin", "User" };
        string[] roleDescs = { "Admin giving full rights.", "Standard user with limited rights." };
        
        for (int i = 0; i < roleNames.Length; i++)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleNames[i]);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleNames[i], Description = roleDescs[i] });
            }
        }
    }
}