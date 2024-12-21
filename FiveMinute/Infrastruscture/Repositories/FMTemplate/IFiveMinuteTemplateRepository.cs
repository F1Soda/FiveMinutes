using FiveMinute.Models;

namespace FiveMinute.Interfaces
{
	public interface IFiveMinuteTemplateRepository : IDefaultRepository<FiveMinuteTemplate>
	{
		Task<FiveMinuteTemplate?> GetByIdAsync(int id);

		Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id);
		Task<bool> Update(FiveMinuteTemplate fmt, FiveMinuteTemplate newFmt);

		IEnumerable<FiveMinuteTemplate> GetAllFromUserId(string userId);

		Task<bool> DeleteCascade(FiveMinuteTemplate template);

	}
}
