using FiveMinutes.Models;

namespace FiveMinutes.Interfaces;

public interface IFiveMinuteResultsRepository : IDefaultRepository<FiveMinuteResult>
{
    Task<ICollection<FiveMinuteResult?>> GetByFMTIdAsync(int fiveMinuteId);
    Task<FiveMinuteResult> GetById(int fiveMinuteId);
}