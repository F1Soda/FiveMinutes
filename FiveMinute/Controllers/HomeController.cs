using FiveMinutes.Data;
using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FiveMinutes.Controllers
{
	public class HomeController : Controller
	{
        private readonly UserManager<AppUser> userManager;

		public HomeController(UserManager<AppUser> userManager)
		{
			this.userManager = userManager;		
		}

		public async Task<IActionResult> Index()
		{
			var currentUser = await userManager.GetUserAsync(User);

			List<FiveMinuteTest>? plannedTests = null;
			List<FiveMinuteTest>? activeTests = null;
			List<FiveMinuteTest>? Tests = null;
			List<FiveMinuteTemplate>? Templates = null;

			if (currentUser != null)
			{
				if (currentUser.canCreate)
				{
					plannedTests = currentUser?.FMTests
						.Where(test => test.Status == Data.TestStatus.Planned)
						.OrderBy(test => test.StartTime)
						.ToList();

					activeTests = currentUser?.FMTests
						.Where(test => test.Status == Data.TestStatus.Started)
						.ToList();

					Tests = currentUser?.FMTests.ToList();
					Templates = currentUser?.FMTemplates.ToList();
				}
			}



			var viewModel = new
			{
				UserName = currentUser?.UserName,
				PlannedTests = plannedTests,
				ActiveTests = activeTests,
				Tests = Tests,
				Templates = Templates
			};

			return View(userManager);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
		}
	}
}
