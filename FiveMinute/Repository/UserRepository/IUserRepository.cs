using FiveMinute.Models;
using FiveMinute.Repository.DefaultRepository;
using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;

namespace FiveMinute.Repository.UserRepository
{
	// Скорее всего он не нужен
	public interface IUserRepository : IDefaultRepository<AppUser>
	{
		Task<IEnumerable<AppUser>> GetAllUsers();
		Task<AppUser?> GetUserById(string id);
		Task<AppUser?> FindByEmailAsync(string email);
		Task<IdentityResult> CreateAsync(AppUser user);
		Task<bool> AddFMTtoUser(FiveMinuteTemplate fmt, AppUser user);
	}
}
