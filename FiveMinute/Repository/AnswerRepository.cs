using FiveMinute.Interfaces;
using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;

namespace FiveMinute.Repository
{
    public class AnswerRepository : DefaultRepository<Answer>, IAnswerRepository
    {
        private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;
        public AnswerRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public Task<ICollection<Answer>> GetByIdFMTAsync(int fiveMinuteId)
        {
            throw new NotImplementedException();
        }
    }
}
