using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FiveMinute.Models;
using FiveMinute.ViewModels.AccountViewModels;
using FiveMinute.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FiveMinute.ViewModels.HomeViewModels;
using FiveMinute.ViewModels.FMTEditViewModels;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;

namespace FiveMinute.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly UserManager<AppUser> userManager;

		public HomeController(ApplicationDbContext context, UserManager<AppUser> userManager)
		{
			this.context = context;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var currentUser = await userManager.GetUserAsync(User);


			IndexViewModel model = null!;
			if (currentUser != null)
			{
				// Тут возможно двойная работа, но пускай так будет
				var user = await context.Users.Include(x => x.FMTTemplates)
						.ThenInclude(x => x.Questions)
							
					.Include(x => x.FMTests)
					.FirstOrDefaultAsync(x => x.Id == currentUser.Id);

				model = new IndexViewModel
				{
					UserName = user.UserName,
					Email = user.Email,
					FMTemplates = user.FMTTemplates.Select(x => FMTemplateIndexViewModel.CreateByModel(x)).ToList(),
					FMTests = user.FMTests.Select(x => FMTestIndexViewModel.CreateByModel(x)).ToList(),
					UserRole = currentUser.UserRole
				};

			}
			return View(model);
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
