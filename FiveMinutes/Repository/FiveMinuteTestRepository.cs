using FiveMinutes.Data;
using FiveMinutes.Models;

namespace FiveMinutes.Repository;

public class FiveMinuteTestRepository: DefaultRepository<FiveMinuteTest>
{
    public FiveMinuteTestRepository(ApplicationDbContext context) : base(context)
    {
    }
}