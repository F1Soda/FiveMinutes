using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public DbSet<Answer> Answers;
		public DbSet<FiveMinuteTemplate> FiveMinuteTemplates;
		public DbSet<FiveMinuteTest> FiveMinuteTests;
		public DbSet<Folder> Folders;
		public DbSet<Question> Questions;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
	}
}