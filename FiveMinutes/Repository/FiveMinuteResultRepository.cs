using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Repository;

public class FiveMinuteResultRepository:DefaultRepository<FiveMinuteResult>, IFiveMinuteResultsRepository
{
    public FiveMinuteResultRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<FiveMinuteResult?> GetByIdAsync(int id)
    {
        return await context.FiveMinuteResults
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}