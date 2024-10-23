using FiveMinutes.Data;
using FiveMinutes.Models;

namespace FiveMinutes.Repository;

public class EducationTestRepository: DefaultRepository<EducationTest>
{
    public EducationTestRepository(ApplicationDbContext context) : base(context)
    {
    }
}