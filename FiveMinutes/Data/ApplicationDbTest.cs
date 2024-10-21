using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Data;

public class ApplicationDbTest : DbContext
{
    public ApplicationDbTest(DbContextOptions<ApplicationDbTest> options) : base(options) { }
}