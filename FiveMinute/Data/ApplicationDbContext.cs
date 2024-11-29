using FiveMinute.Models;
using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public DbSet<Answer> Answers { get; set; }
		public DbSet<FiveMinuteTemplate> FiveMinuteTemplates { get; set; }
		public DbSet<FiveMinuteTest> FiveMinuteTests { get; set; }
		public DbSet<Folder> Folders { get; set; }

		public DbSet<FiveMinuteResult> FiveMinuteResults { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
	}
}