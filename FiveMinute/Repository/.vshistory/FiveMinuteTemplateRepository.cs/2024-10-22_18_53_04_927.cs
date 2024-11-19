using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using System.Diagnostics;

namespace FiveMinutes.Repository
{
	public class FiveMinuteTemplateRepository : IFiveMinuteTemplateRepository
	{
		private readonly ApplicationDbContext context;

		public FiveMinuteTemplateRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

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
			var saved = context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(FiveMinuteTemplate fmt)
		{
			context.Update(race);
			return Save();
		}
	}
}
