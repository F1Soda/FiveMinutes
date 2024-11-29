using FiveMinute.Interfaces;
using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.Repository;

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
