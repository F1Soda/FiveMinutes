using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using System.Diagnostics;

namespace FiveMinutes.Repository
{
	public class FiveMinuteTemplateRepository : DefaultRepository<FiveMinuteTemplate> , IFiveMinuteTemplateRepository
	{

		public FiveMinuteTemplateRepository(ApplicationDbContext context) : base(context) { }

		public Task<IEnumerable<FiveMinuteTemplate>> GetAllFromUserId(int userId)
		{
			throw new NotImplementedException();
		}
		
	}
}
