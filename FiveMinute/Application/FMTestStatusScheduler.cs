using FiveMinute.Data;
using FiveMinute.Models;
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

					foreach (var test in context.FiveMinuteTests)
					{
						if (test.Status == TestStatus.Open && test.EndPlanned && test.EndTime < DateTime.UtcNow)
						{
							test.Status = TestStatus.Closed;
							context.FiveMinuteTests.Update(test);
						}
						if (test.Status == TestStatus.Closed && test.StartPlanned && test.StartTime >= DateTime.UtcNow)
						{
							test.Status = TestStatus.Open;
							context.FiveMinuteTests.Update(test);
						}
					}

					await context.SaveChangesAsync();
				}

				// Тут нужно логику хорошую сделать
				await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
			}
		}
	}
}
