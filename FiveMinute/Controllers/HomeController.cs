using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FiveMinute.Models;
using FiveMinute.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FiveMinute.ViewModels.HomeViewModels;
using FiveMinute.ViewModels.FMTEditViewModels;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FiveMinute.Controllers
{
	public class HomeController(
		ApplicationDbContext context,
		UserManager<AppUser> userManager,
		IFiveMinuteTestRepository fiveMinuteTestRepository,
		IFiveMinuteTemplateRepository fiveMinuteTemplateRepository,
		ICompositeViewEngine viewEngine)
		: Controller
	{
		public async Task<IActionResult> Index()
		{
			var currentUser = await userManager.GetUserAsync(User);


			IndexViewModel model = null!;
			if (currentUser != null)
			{
				// ��� �������� ������� ������, �� ������ ��� �����
				var user = await context.Users.Include(x => x.FMTTemplates)
					.ThenInclude(x => x.Questions)
					.Include(x => x.FMTests)
					.FirstOrDefaultAsync(x => x.Id == currentUser.Id);//не ну это пиздец
				model = IndexViewModel.CreateByModel(user);
			}
			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public class DeleteRequestId
		{
			public int Id { get; set; }
		}

		[HttpPost]
		public async Task<IActionResult> DeleteTemplate([FromBody] DeleteRequestId deleteTemplateRequest)
		{
			var id = deleteTemplateRequest.Id;
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));

			var template = await fiveMinuteTemplateRepository.GetByIdAsync(id); // Your repository method to fetch the template
			if (template == null)
			{
				return Json(new { success = false, reason = $"Where is no FMTemplate with id {id}" });
			}
			if (await fiveMinuteTemplateRepository.DeleteCascade(template))
			{
				// Fetch updated data for both tabs
				var templates = fiveMinuteTemplateRepository.GetAllFromUserId(currentUser.Id)
					?.OrderByDescending(x => x.LastModificationTime)
					?.Select(FMTemplateIndexViewModel.CreateByModel);
				var tests = fiveMinuteTestRepository.GetAllFromUserId(currentUser.Id)
					?.OrderByDescending(x => x.CreationTime)
					?.Select(FMTestIndexViewModel.CreateByModel);

				if (templates != null)
				{
					templates = (ICollection<FMTemplateIndexViewModel>)templates.ToList();
				}
				if (tests != null)
				{
					tests = (ICollection<FMTestIndexViewModel>)tests.ToList();
				}

				// Render updated HTML for both tables
				var templatesHtml = await RenderPartialViewToString("_TemplatesTable", templates);
				var testsHtml = await RenderPartialViewToString("_TestsTable", tests);

				return Json(new { success = true, templatesHtml, testsHtml });
			}
			return Json(new { success = false });
		}


		[HttpPost]
		public async Task<IActionResult> DeleteTest([FromBody] DeleteRequestId deleteTemplateRequest)
		{
			var id = deleteTemplateRequest.Id;
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));

			var test = await fiveMinuteTestRepository.GetByIdAsync(id); // Your repository method to fetch the test
			if (test == null)
			{
				return Json(new { success = false, reason = $"Where is no FMTest with id {id}" });
			}
			if (await fiveMinuteTestRepository.Delete(test))
			{
				// Fetch updated data for both tabs
				var templates = fiveMinuteTemplateRepository.GetAllFromUserId(currentUser.Id)
					?.OrderByDescending(x => x.LastModificationTime)
					?.Select(FMTemplateIndexViewModel.CreateByModel);
				var tests = fiveMinuteTestRepository.GetAllFromUserId(currentUser.Id)
					?.OrderByDescending(x => x.CreationTime)
					?.Select(FMTestIndexViewModel.CreateByModel);

				if (templates != null)
				{
					templates = (ICollection<FMTemplateIndexViewModel>)templates.ToList();
				}
				if (tests != null)
				{
					tests = (ICollection<FMTestIndexViewModel>)tests.ToList();
				}

				// Render updated HTML for both tables
				var templatesHtml = await RenderPartialViewToString("_TemplatesTable", templates);
				var testsHtml = await RenderPartialViewToString("_TestsTable", tests);

				return Json(new { success = true, templatesHtml, testsHtml });
			}
			return Json(new { success = false });
		}

		// Helper method to render partial views as strings
		private async Task<string> RenderPartialViewToString(string viewName, object model)
		{
			ViewData.Model = model;

			using (var sw = new StringWriter())
			{
				var viewResult = viewEngine.FindView(ControllerContext, viewName, false);
				if (!viewResult.Success)
				{
					throw new InvalidOperationException($"Could not find view: {viewName}");
				}

				var viewContext = new ViewContext(
					ControllerContext,
					viewResult.View,
					ViewData,
					TempData,
					sw,
					new HtmlHelperOptions()
				);
				await viewResult.View.RenderAsync(viewContext);
				return sw.GetStringBuilder().ToString();
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
		}
	}
}
