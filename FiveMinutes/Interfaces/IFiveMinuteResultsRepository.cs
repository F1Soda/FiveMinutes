using FiveMinutes.Models;

namespace FiveMinutes.Interfaces;

public interface IFiveMinuteResultsRepository : IDefaultRepository<FiveMinuteResult>
{
    Task<ICollection<FiveMinuteResult?>> GetByIdAsync(int fiveMinuteId);
}