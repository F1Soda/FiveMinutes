using FiveMinutes.Models;
using System.Diagnostics;

namespace FiveMinutes.Interfaces
{
    public interface IFiveMinuteTemplateRepository
    {
        Task<FiveMinuteTemplate?> GetByIdAsync(int id);

        Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id);

        Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUserId(int userId);

        Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUser(AppUser user);

        bool Add(FiveMinuteTemplate fmt);

        bool Update(FiveMinuteTemplate fmt);

        bool Delete(FiveMinuteTemplate fmt);

        bool Save();
    }
}
