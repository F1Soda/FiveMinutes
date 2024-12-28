using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.Interfaces;

namespace FiveMinute.Repository.FMTestRepository
{
	public interface IFiveMinuteTestRepository : IDefaultRepository<FiveMinuteTest>
	{
		public Task<FiveMinuteTest?> GetByIdAsync(int id);

		public Task<ResultOperationInDatabase> Update(FiveMinuteTest updatedTest);

		public Task<ResultOperationInDatabase> AddResultToTest(int testId, FiveMinuteTestResult testResults);

		IEnumerable<FiveMinuteTest> GetAllFromUserId(string userId);
	}
}