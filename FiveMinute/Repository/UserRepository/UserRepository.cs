using FiveMinute.Database;
using FiveMinute.Models;
using FiveMinute.Repository.DefaultRepository;
using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Repository.UserRepository
{
	public class UserRepository : DefaultRepository<AppUser>, IUserRepository
	{
		private readonly ApplicationDbContext context;

		public UserRepository(ApplicationDbContext context) : base(context)
		{
			this.context = context;
		}
		public Task<IdentityResult> CreateAsync(AppUser user)
		{
			throw new NotImplementedException();
		}


		public Task<AppUser?> FindByEmailAsync(string email)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<AppUser>> GetAllUsers()
		{
			return await context.Users.ToListAsync();
		}

		public async Task<AppUser> GetUserById(string id)
		{
			return await context.Users.FindAsync(id);
		}
		public async Task<bool> AddFMTtoUser(FiveMinuteTemplate fmt, AppUser user)
		{
			user.AddFMT(fmt);
			return await Save();
		}
	}
}
