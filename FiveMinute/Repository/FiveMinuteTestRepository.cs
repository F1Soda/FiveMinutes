using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Repository;

public class FiveMinuteTestRepository: DefaultRepository<FiveMinuteTest>, IFiveMinuteTestRepository
{
    public FiveMinuteTestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<FiveMinuteTest?> GetByIdAsync(int id)
    {
        // return await context.FiveMinuteTests
        //     .Include(x =>x.UserOwner) // если мы хотим в FiveMinuteTemplate знать об AppUse
        //     .FirstOrDefaultAsync(x => x.Id == id);
        throw new NotImplementedException();
    }
}