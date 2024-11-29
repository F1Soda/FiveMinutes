using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Repository
{
	public class FiveMinuteTemplateRepository : DefaultRepository<FiveMinuteTemplate>, IFiveMinuteTemplateRepository
	{
		public FiveMinuteTemplateRepository(ApplicationDbContext context) : base(context) { }

		public async Task<FiveMinuteTemplate?> GetByIdAsync(int id)
		{
			return await context.FiveMinuteTemplates
				.Include(x => x.UserOwner)
				.Include(x => x.Questions)
					.ThenInclude(q => q.AnswerOptions)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id)
		{
			return await context.FiveMinuteTemplates
				.Include(x => x.UserOwner) // если мы хотим в FiveMinuteTemplate знать об AppUser
				.Include(x => x.Questions)
					.ThenInclude(q => q.AnswerOptions)
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
		}


		public async Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUserId(string userId)
		{
			return await context.FiveMinuteTemplates
				.Where(x => x.UserOwnerId == userId)
				.ToListAsync();
		}

		public async Task<bool> Update(FiveMinuteTemplate existingTemplate, FiveMinuteTemplate newTemplate)
		{
			context.FiveMinuteTemplates.Attach(existingTemplate);
            existingTemplate.Name = newTemplate.Name;
            existingTemplate.ShowInProfile = newTemplate.ShowInProfile;
            existingTemplate.LastModificationTime = DateTime.UtcNow;
            existingTemplate.Questions = newTemplate.Questions;
            context.Entry(existingTemplate).State = EntityState.Modified;
			return await Save();
		}

	}
}

