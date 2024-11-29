using FiveMinute.Interfaces;
using FiveMinute.Models;

namespace FiveMinute.Interfaces
{
    public interface IAnswerRepository : IDefaultRepository<Answer>
    {
        Task<ICollection<Answer>> GetByIdFMTAsync(int fiveMinuteId);
    }
}
