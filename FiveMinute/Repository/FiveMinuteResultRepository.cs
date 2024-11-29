using FiveMinute.Database;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository.DefaultRepository;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Repository;

public class FiveMinuteResultRepository : DefaultRepository<FiveMinuteResult?>, IFiveMinuteResultsRepository
{
	public FiveMinuteResultRepository(ApplicationDbContext context) : base(context)
	{
	}


	public void UpdateQuestions(FiveMinuteTemplate template, List<Question> questions)
	{
		template.Questions = questions;
		context.Entry(template).State = EntityState.Modified;
	}
	public async Task<ICollection<FiveMinuteResult?>> GetByFMTIdAsync(int fiveMinuteId)
	{
		return await context.FiveMinuteResults.Include(x => x.Answers).Where(x => x.FiveMinuteTemplateId == fiveMinuteId)
			.ToListAsync();
	}

	public async Task<FiveMinuteResult?> GetById(int resultId)
	{
		return await context.FiveMinuteResults
			.Include(x => x.Answers)
			.Include(x => x.FiveMinuteTemplate)
			.ThenInclude(x => x.Questions)
			.ThenInclude(x => x.AnswerOptions)
			.FirstOrDefaultAsync(x => x.Id == resultId);
	}
}