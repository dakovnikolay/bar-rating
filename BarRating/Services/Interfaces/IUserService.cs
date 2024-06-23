using BarRating.Data.Entities;
using BarRating.Data.ViewModels;
using BarRating.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BarRating.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel model);
        Task<SignInResult> LoginUserAsync(LoginViewModel model);
        Task LogoutUserAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IdentityResult> UpdateUserAsync(UserViewModel user);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<int> GetUserCountAsync();
    }

}