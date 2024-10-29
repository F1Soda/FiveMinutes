using FiveMinutes.Models;

namespace FiveMinutes.Interfaces
{
    public interface IFiveMinuteTestRepository : IDefaultRepository<FiveMinuteTest>
    {
        public Task<FiveMinuteTest> GetByIdAsync(int id);
    }
}
