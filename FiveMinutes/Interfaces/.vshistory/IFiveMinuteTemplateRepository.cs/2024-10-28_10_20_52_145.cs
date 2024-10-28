using FiveMinutes.Models;
using System.Diagnostics;

namespace FiveMinutes.Interfaces
{
    public interface IFiveMinuteTemplateRepository : IDefaultRepository<FiveMinuteTemplate>
    {
        Task<FiveMinuteTemplate?>  GetByIdAsync(int id);

        Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id);        

        // Возможно лишнее, но пока оставлю
        Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUserId(int userId);
    }   
}
