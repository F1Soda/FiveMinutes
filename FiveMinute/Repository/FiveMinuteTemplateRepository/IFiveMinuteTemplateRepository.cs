using FiveMinute.Models;
using FiveMinute.Repository.DefaultRepository;
using System.Diagnostics;

namespace FiveMinute.Repository.FiveMinuteTemplateRepository
{
	public interface IFiveMinuteTemplateRepository : IDefaultRepository<FiveMinuteTemplate>
	{
		Task<FiveMinuteTemplate?> GetByIdAsync(int id);

		Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id);
		Task<bool> Update(FiveMinuteTemplate fmt, FiveMinuteTemplate newFmt);

		// Возможно лишнее, но пока оставлю
		Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUserId(string userId);
	}
}
