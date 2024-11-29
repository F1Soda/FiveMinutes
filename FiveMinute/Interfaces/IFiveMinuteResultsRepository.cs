using FiveMinute.Models;
using FiveMinute.Repository.DefaultRepository;

namespace FiveMinute.Interfaces;

public interface IFiveMinuteResultsRepository : IDefaultRepository<FiveMinuteResult>
{
    Task<ICollection<FiveMinuteResult?>> GetByFMTIdAsync(int fiveMinuteId);
    Task<FiveMinuteResult> GetById(int fiveMinuteId);
}