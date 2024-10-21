using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Question> Questions {  get;set; }
		public DbSet<Answer> Answers {  get;set; }
        public DbSet<EducationTest> EducationTests { get; set; }
        public DbSet<FiveMinuteTemplate> FiveMinuteTemplate { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
	}
}
