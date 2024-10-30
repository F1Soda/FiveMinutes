using FiveMinutes.Models;

namespace FiveMinutes.Interfaces;

public interface IFiveMinuteResultsRepository : IDefaultRepository<FiveMinuteResult>
{
    Task<FiveMinuteResult?> GetByIdAsync(int id);
}