using FiveMinute.Models;

namespace FiveMinute.Interfaces;

public interface IQuestionRepository: IDefaultRepository<Question>
{
    public Task<Question> GetById(int id);
    public Task<Question> GetByIdAsyncNoTracking(int id);

    public Task<bool> DeleteByFMT(FiveMinuteTemplate fiveMinuteTemplate);
}