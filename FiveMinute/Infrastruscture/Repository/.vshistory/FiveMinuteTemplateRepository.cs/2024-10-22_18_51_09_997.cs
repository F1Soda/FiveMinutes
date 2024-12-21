using FiveMinutes.Interfaces;
using FiveMinutes.Models;

namespace FiveMinutes.Repository
{
	public class FiveMinuteTemplateRepository : IFiveMinuteTemplateRepository
	{
		public bool Add(FiveMinuteTemplate fmt)
		{
			throw new NotImplementedException();
		}

		public bool Delete(FiveMinuteTemplate fmt)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUserId(int userId)
		{
			throw new NotImplementedException();
		}

		public Task<FiveMinuteTemplate?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<FiveMinuteTemplate?> GetByIdAsyncNoTracking(int id)
		{
			throw new NotImplementedException();
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}

		public bool Update(FiveMinuteTemplate fmt)
		{
			throw new NotImplementedException();
		}
	}
}
