using Microsoft.EntityFrameworkCore;
using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels;

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

		FMTest.Results.Append(testResults);

		return await Save();
	}

	public async Task<FiveMinuteTest?> GetByIdAsync(int id)
	{
		return await context.FiveMinuteTests
			.Include(x => x.UserOrganizer)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<bool> Update(FiveMinuteTest existingTest, FiveMinuteTest updatedTest)
	{
		context.FiveMinuteTests.Attach(existingTest);
		existingTest.Name = updatedTest.Name;

		existingTest.AttachedFMTId = updatedTest.AttachedFMTId;
		existingTest.AttachedFMT = updatedTest.AttachedFMT;

		existingTest.Status = updatedTest.Status;

		existingTest.StartPlanned = updatedTest.StartPlanned;
		existingTest.EndPlanned = updatedTest.EndPlanned;
		existingTest.StartTime = updatedTest.StartTime;
		existingTest.EndTime = updatedTest.EndTime;

		existingTest.Results = updatedTest.Results;

		return await Save();
	}



}