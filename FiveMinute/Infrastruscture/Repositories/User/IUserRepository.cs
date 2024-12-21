using FiveMinute.Models;
using Microsoft.AspNetCore.Identity;

namespace FiveMinute.Interfaces
{
    public interface IUserRepository : IDefaultRepository<AppUser>
    {
        Task<AppUser> GetFullUserDataById(string id);
        Task<bool> AddFmTtoUser(FiveMinuteTemplate fmt, AppUser user);
    }
}
