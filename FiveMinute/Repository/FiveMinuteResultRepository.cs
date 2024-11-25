using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Repository;

public class FiveMinuteResultRepository: DefaultRepository<FiveMinuteResult?>, IFiveMinuteResultsRepository
{
    public FiveMinuteResultRepository(ApplicationDbContext context) : base(context)
    {
    }


    public void UpdateQuestions(FiveMinuteTemplate template, List<Question> questions)
    {
        template.Questions = questions;
        context.Entry(template).State = EntityState.Modified;
    }
    public async Task<ICollection<FiveMinuteResult?>> GetByIdAsync(int fiveMinuteId)
    {
        return await context.FiveMinuteResults.Include(x => x.Answers).Where(x => x.FiveMinuteTemplateId == fiveMinuteId)
            .ToListAsync();
    }
}