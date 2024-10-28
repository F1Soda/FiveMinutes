using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FiveMinutes.Repository
{
	public class FiveMinuteTemplateRepository : DefaultRepository<FiveMinuteTemplate>, IFiveMinuteTemplateRepository
	{
		public FiveMinuteTemplateRepository(ApplicationDbContext context) : base(context) { }

		public async Task<FiveMinuteTemplate?> GetByIdAsync(int id)
		{
			return await context.FiveMinuteTemplates
				//.Include(x =>x.AppUser) если мы хотим в FiveMinuteTemplate знать об AppUse
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id)
		{
			return await context.FiveMinuteTemplates
				//.Include(x =>x.AppUser) если мы хотим в FiveMinuteTemplate знать об AppUser
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUserId(int userId)
		{
			return await context.FiveMinuteTemplates
				.Where(x => x.UserOwnerId == userId)
				.ToListAsync();
		}

		public FiveMinuteTemplate GetById(int id)
		{
			// достаёт по id соответствующую пятиминутку
			return new FiveMinuteTemplate();
		}

	}
}
