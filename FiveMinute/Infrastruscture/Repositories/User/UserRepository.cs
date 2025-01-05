using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Repository
{
	public class UserRepository(ApplicationDbContext context) : DefaultRepository<AppUser>(context), IUserRepository
	{
		private readonly ApplicationDbContext context = context;

		public async Task<AppUser?> GetFullUserDataById(string id)
		{
			var user = await context.Users
				.Include(x => x.FMTemplates)
				.ThenInclude(x => x.Questions)
				.Include(x => x.FMTests)
				.ThenInclude(x => x.Results)
				.Include(appUser => appUser.PassedTestResults)
				.FirstOrDefaultAsync(x => x.Id == id);
			return user!;
		}
		public async Task<bool> AddFmTtoUser(FiveMinuteTemplate fmt, AppUser user)
		{
			user.AddFMT(fmt);
			return await Save();
		}
	}
}
