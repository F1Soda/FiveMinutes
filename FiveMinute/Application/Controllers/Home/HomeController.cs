using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FiveMinute.Models;
using Microsoft.AspNetCore.Identity;
using FiveMinute.ViewModels.HomeViewModels;
using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using FiveMinute.Utils;

namespace FiveMinute.Controllers
{
	public partial class HomeController(
		UserManager<AppUser> userManager,
		IUserRepository userRepository,
		IFiveMinuteTestRepository fiveMinuteTestRepository,
		IFiveMinuteTemplateRepository fiveMinuteTemplateRepository,
		ICompositeViewEngine viewEngine)
		: Controller
	{
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
		
		public async Task<IActionResult> Index()
		{
			var currentUser = await userManager.GetUserAsync(User);
			
			var viewModel = new IndexViewModel();
			if (currentUser != null)
				viewModel = await PrepareUserDataIndexViewModel(currentUser);
			viewModel.Quotes = QuotesHandler.GetDailyQuotes();
			return View(viewModel);
		}
		
		[HttpPost]
		public async Task<IActionResult> DeleteTemplate([FromBody] DeleteRequestId deleteTemplateRequest)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));
			
			return await TryDeleteEntity(deleteTemplateRequest.Id, currentUser, fiveMinuteTemplateRepository);
		}
		
		[HttpPost]
		public async Task<IActionResult> DeleteTest([FromBody] DeleteRequestId deleteTemplateRequest)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));
			
			return await TryDeleteEntity(deleteTemplateRequest.Id, currentUser, fiveMinuteTestRepository);
		}

	}
}
