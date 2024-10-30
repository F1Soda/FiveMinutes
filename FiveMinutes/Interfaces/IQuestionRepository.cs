using FiveMinutes.Models;

namespace FiveMinutes.Interfaces;

public interface IQuestionRepository
{
    public Task<Question> GetById(int id);
    public Task<Question> GetByIdAsyncNoTracking(int id);
}