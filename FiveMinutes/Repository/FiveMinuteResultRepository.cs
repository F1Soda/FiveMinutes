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

    public async Task<ICollection<FiveMinuteResult?>> GetByIdAsync(int fiveMinuteId)
    {
        return await context.FiveMinuteResults.Where(x => x.FiveMinuteTemplateId == fiveMinuteId)
            .ToListAsync();
    }
}