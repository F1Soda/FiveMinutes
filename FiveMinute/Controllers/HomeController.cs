using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FiveMinute.Models;
using FiveMinute.ViewModels.AccountViewModels;
using FiveMinute.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

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


			UserDetailViewModel model = null!;
			if (currentUser != null)
			{
				// Тут возможно двойная работа, но пускай так будет
				var user = await context.Users.Include(x => x.FMTTemplates)
					.Include(x => x.FMTests)
					.FirstOrDefaultAsync(x => x.Id == currentUser.Id);

				model = new UserDetailViewModel
				{
					UserName = user.UserName,
					Email = user.Email,
					FMTs = user.FMTTemplates,
					Tests = user.FMTests,
					UserRole = currentUser.UserRole,
					IsOwner = true,

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
