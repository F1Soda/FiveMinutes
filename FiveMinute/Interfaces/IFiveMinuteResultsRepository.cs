using FiveMinute.Models;

namespace FiveMinute.Interfaces;

public interface IFiveMinuteResultsRepository : IDefaultRepository<FiveMinuteTestResult>
{
    Task<ICollection<FiveMinuteTestResult?>> GetByTestIdAsync(int testId);
    Task<FiveMinuteTestResult> GetById(int fiveMinuteId);
}