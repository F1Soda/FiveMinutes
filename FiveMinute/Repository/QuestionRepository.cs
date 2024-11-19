using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using Microsoft.EntityFrameworkCore;

public class QuestionRepository : DefaultRepository<Question>, IQuestionRepository
{
    public QuestionRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<Question?> GetById(int id)
    {
        return await context.Questions
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Question?> GetByIdAsyncNoTracking(int id)
    {
        return await context.Questions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> DeleteByFMT(FiveMinuteTemplate fiveMinuteTemplate)
    {
        foreach (var question in fiveMinuteTemplate.Questions)
        {
            if (!Delete(question))
            {
                throw new Exception();//надо??
                return false;
            }
        }
        return true;
    }
}