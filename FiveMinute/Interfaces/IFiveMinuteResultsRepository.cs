using FiveMinute.Models;

namespace FiveMinute.Interfaces;

public interface IFiveMinuteResultsRepository : IDefaultRepository<FiveMinuteTestResult>
{
    Task<ICollection<FiveMinuteTestResult?>> GetByFMTIdAsync(int fiveMinuteId);
    Task<FiveMinuteTestResult> GetById(int fiveMinuteId);
}