using FiveMinute.Models;
using FiveMinute.Interfaces;

namespace FiveMinute.Repository.FiveMinuteTestRepository
{
	public interface IFiveMinuteTestRepository : IDefaultRepository<FiveMinuteTest>
	{
		public Task<FiveMinuteTest> GetByIdAsync(int id);

		public Task<bool> Update(FiveMinuteTest updatedTest);

		public Task<bool> AddResultToTest(int testId, FiveMinuteTestResult testResults);

		IEnumerable<FiveMinuteTest> GetAllFromUserId(string userId);
	}
}