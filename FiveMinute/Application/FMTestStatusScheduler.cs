using FiveMinute.Data;
using FiveMinute.Repository.FMTestRepository;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Application
{
	public class FMTestStatusScheduler : BackgroundService
	{
		private readonly IServiceScopeFactory scopeFactory;

		public FMTestStatusScheduler(IServiceScopeFactory scopeFactory)
		{
			this.scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = scopeFactory.CreateScope())
				{
					var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

					var testsToUpdate = context.FiveMinuteTests
						.Where(test => test.Status == TestStatus.Planned && test.EndPlanned && test.EndTime <= DateTime.UtcNow);

					foreach (var test in testsToUpdate)
					{
						test.Status = TestStatus.Closed;
						context.FiveMinuteTests.Update(test);
					}

					await context.SaveChangesAsync();
				}

				// Wait before checking again
				await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
			}
		}
	}
}
