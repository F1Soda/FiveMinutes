using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Repository
{
	public class FiveMinuteTemplateRepository : DefaultRepository<FiveMinuteTemplate>, IFiveMinuteTemplateRepository
	{
		public FiveMinuteTemplateRepository(ApplicationDbContext context) : base(context) { }

		public async Task<FiveMinuteTemplate?> GetByIdAsync(int id)
		{
			return await context.FiveMinuteTemplates
				.Include(x =>x.UserOwner) // если мы хотим в FiveMinuteTemplate знать об AppUse
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id)
		{
			return await context.FiveMinuteTemplates
				.Include(x =>x.UserOwner) // если мы хотим в FiveMinuteTemplate знать об AppUser
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUserId(string userId)
		{
			return await context.FiveMinuteTemplates
				.Where(x => x.UserOwnerId == userId)
				.ToListAsync();
		}
	}
}
