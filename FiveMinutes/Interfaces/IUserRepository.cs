using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;

namespace FiveMinutes.Interfaces
{
// Скорее всего он не нужен
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser?> GetUserById(string id);
        Task<AppUser?> FindByEmailAsync(string email);
        Task<IdentityResult> CreateAsync(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
