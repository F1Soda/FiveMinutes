using FiveMinutes.Models;

namespace FiveMinutes.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser?> GetUserById(string id);
        Task<AppUser?> FindByEmailAsync(string email);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
