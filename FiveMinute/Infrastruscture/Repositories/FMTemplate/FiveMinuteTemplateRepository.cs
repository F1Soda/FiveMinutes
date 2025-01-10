using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using Microsoft.EntityFrameworkCore;
using FiveMinute.Repository.FMTestRepository;

namespace FiveMinute.Repository {
	public class FiveMinuteTemplateRepository : DefaultRepository<FiveMinuteTemplate>, IFiveMinuteTemplateRepository {
		private readonly IFiveMinuteTestRepository fMTestRepository;

		public FiveMinuteTemplateRepository(ApplicationDbContext context,
		                                    IFiveMinuteTestRepository fiveMinuteTestRepository)
			: base(context) {
			fMTestRepository = fiveMinuteTestRepository;
		}

		public async Task<FiveMinuteTemplate?> GetByIdAsync(int id) {
			return await context.FiveMinuteTemplates
			                    .Include(x => x.UserOwner)
			                    .Include(x => x.Questions)
			                    .ThenInclude(q => q.AnswerOptions)
			                    .FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id) {
			return await context.FiveMinuteTemplates
			                    .Include(x => x.UserOwner) // если мы хотим в FiveMinuteTemplate знать об AppUser
			                    .Include(x => x.Questions)
			                    .ThenInclude(q => q.AnswerOptions)
			                    .AsNoTracking()
			                    .FirstOrDefaultAsync(x => x.Id == id);
		}


		public IEnumerable<FiveMinuteTemplate> GetAllFromUserId(string userId) {
			return context.Users.Include(x => x.FMTemplates).FirstOrDefault(x => x.Id == userId)?.FMTemplates;
		}

		public async Task<bool> Update(FiveMinuteTemplate existingTemplate, FiveMinuteTemplate newTemplate) {
			context.FiveMinuteTemplates.Attach(existingTemplate);
			existingTemplate.Name = newTemplate.Name;
			existingTemplate.ShowInProfile = newTemplate.ShowInProfile;
			existingTemplate.LastModificationTime = DateTime.UtcNow;
			existingTemplate.Questions = newTemplate.Questions;
			context.Entry(existingTemplate).State = EntityState.Modified;
			return await Save();
		}

		public override async Task<ResultOperationInDatabase> Delete(FiveMinuteTemplate template) {
			var relatedTests = context.FiveMinuteTests.Include(x => x.Results)
				.ThenInclude(q => q.Answers)
			                          .Where(x => x.FiveMinuteTemplateId == template.Id);
			foreach (var test in relatedTests) {
				await fMTestRepository.Delete(test);
			}

			context.Remove(template!);
			return await Save();
		}
	}
}