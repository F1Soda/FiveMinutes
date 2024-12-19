using Microsoft.EntityFrameworkCore;
using FiveMinute.Data;
using FiveMinute.Models;

namespace FiveMinute.Repository.FiveMinuteTestRepository;

public class FiveMinuteTestRepository : DefaultRepository<FiveMinuteTest>, IFiveMinuteTestRepository
{
	public FiveMinuteTestRepository(ApplicationDbContext context) : base(context)
	{
	}
	public async Task<bool> AddResultToTest(int testId, FiveMinuteTestResult testResults)
	{
		var FMTest = await GetByIdAsync(testId);

		if (FMTest == null)
		{
			throw new Exception($"Test Is not exist: TestID: {testId}");
		}

		FMTest.Results.Add(testResults);

		return await Save();
	}

	public IEnumerable<FiveMinuteTest> GetAllFromUserId(string userId)
	{
		var user = context.Users.Include(x => x.FMTests).FirstOrDefault(x => x.Id == userId);
		return user.FMTests;
	}

	public async Task<FiveMinuteTest?> GetByIdAsync(int id)
	{
		return await context.FiveMinuteTests
			.Include(x => x.UserOrganizer)
			.Include(x => x.FiveMinuteTemplate)
				.ThenInclude(x => x.Questions)
				.ThenInclude(x => x.AnswerOptions)
			.Include(x => x.Results)
				.ThenInclude(x => x.Answers)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<bool> Update(FiveMinuteTest updatedTest)
	{
		var existingTest = await GetByIdAsync(updatedTest.Id);
		if (existingTest == null)
			throw new Exception();
		
		context.FiveMinuteTests.Attach(existingTest);
		existingTest.Name = updatedTest.Name == null ? existingTest.Name : updatedTest.Name;

		existingTest.FiveMinuteTemplateId =
			updatedTest.FiveMinuteTemplateId == null
				? existingTest.FiveMinuteTemplateId
				: updatedTest.FiveMinuteTemplateId;
		existingTest.FiveMinuteTemplate = updatedTest.FiveMinuteTemplate == null
			? existingTest.FiveMinuteTemplate
			: updatedTest.FiveMinuteTemplate;

		existingTest.Status = updatedTest.Status == null ? existingTest.Status : updatedTest.Status;
		existingTest.IdToUninclude =
			updatedTest.IdToUninclude == null ? existingTest.IdToUninclude : updatedTest.IdToUninclude;
		existingTest.StartPlanned =
			updatedTest.StartPlanned == null ? existingTest.StartPlanned : updatedTest.StartPlanned;
		existingTest.EndPlanned = updatedTest.EndPlanned == null ? existingTest.EndPlanned : updatedTest.EndPlanned;
		existingTest.StartTime = updatedTest.StartTime == null ? existingTest.StartTime : updatedTest.StartTime;
		existingTest.EndTime = updatedTest.EndTime == null ? existingTest.EndTime : updatedTest.EndTime;

		existingTest.Results = updatedTest.Results == null ? existingTest.Results : updatedTest.Results;

		return await Save();
	}
}