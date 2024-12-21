using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Repository;

public class FiveMinuteResultRepository: DefaultRepository<FiveMinuteTestResult?>, IFiveMinuteResultsRepository
{
    public FiveMinuteResultRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ICollection<FiveMinuteTestResult?>> GetByTestIdAsync(int testId)
    {
        return await context.FiveMinuteResults
            .Include(x => x.Answers)
            .Where(x => x.FiveMinuteTestId == testId)
            .ToListAsync();
    }

    public async Task<FiveMinuteTestResult?> GetById(int resultId)
    {
        return await context.FiveMinuteResults
            .Include(x => x.Answers)
            .FirstOrDefaultAsync(x => x.Id == resultId);
    }
}