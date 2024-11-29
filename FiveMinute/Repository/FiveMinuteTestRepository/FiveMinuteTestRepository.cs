using FiveMinute.Database;
using FiveMinute.Repository.DefaultRepository;
using FiveMinute.Models;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Repository.FiveMinuteTestRepository;

public class FiveMinuteTestRepository : DefaultRepository<FiveMinuteTest>, IFiveMinuteTestRepository
{
	public FiveMinuteTestRepository(ApplicationDbContext context) : base(context)
	{
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