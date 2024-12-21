using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FiveMinute.Models;
using Microsoft.AspNetCore.Identity;
using FiveMinute.ViewModels.HomeViewModels;
using FiveMinute.ViewModels.FMTEditViewModels;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using FiveMinute.Utils;
using FiveMinute.ViewModels.AccountViewModels;

namespace FiveMinute.Controllers
{
	public class HomeController(
		UserManager<AppUser> userManager,
		IUserRepository userRepository,
		IFiveMinuteTestRepository fiveMinuteTestRepository,
		IFiveMinuteTemplateRepository fiveMinuteTemplateRepository,
		ICompositeViewEngine viewEngine)
		: Controller
	{
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => 
			View(new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
		
		public async Task<IActionResult> Index()
		{
			var currentUser = await userManager.GetUserAsync(User);
			
			var viewModel = new IndexViewModel();
			if (currentUser != null)
				viewModel = await PrepareUserDataIndexViewModel(currentUser);
			viewModel.Quotes = QuotesHandler.GetDailyQuotes();
			return View(viewModel);
		}

		private async Task<IndexViewModel> PrepareUserDataIndexViewModel(AppUser currentUser) {
			var fullUserData = await userRepository.GetFullUserDataById(currentUser.Id);
			var model = IndexViewModel.CreateByModel(fullUserData);
			foreach (var result in model.FMTResults)
			{
				var fmtest = await fiveMinuteTestRepository.GetByIdAsync(result.FiveMinuteTestId);
				result.FMTestName = fmtest.Name;
				result.FMTestOrganizer = $"{fmtest.UserOrganizer.UserData.FirstName} {fmtest.UserOrganizer.UserData.LastName}";
			}

			return model;
		}

		public class DeleteRequestId
		{
			public int Id { get; set; }
		}

		[HttpPost]
		public async Task<IActionResult> DeleteTemplate([FromBody] DeleteRequestId deleteTemplateRequest)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));
			
			var id = deleteTemplateRequest.Id;
			var template = await fiveMinuteTemplateRepository.GetByIdAsync(id);
			if (template == null)
				return Json(new { success = false, reason = $"Where is no element with id {id}" });
			if (await fiveMinuteTemplateRepository.DeleteCascade(template)) {
				var (templatesHtml, testsHtml, testCardsRowHtml) = await GetUpdatedHmtlTableString(currentUser);
				return Json(new { success = true, templatesHtml, testsHtml, testCardsRowHtml});
			}
			return Json(new { success = false, reason = "Occur some error while cascade element"});
		}
		

		[HttpPost]
		public async Task<IActionResult> DeleteTest([FromBody] DeleteRequestId deleteTemplateRequest)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));
			
			return await HandleDeleteElement(deleteTemplateRequest.Id, currentUser);
		}

		private async Task<IActionResult> HandleDeleteElement(int idObject, AppUser currentUser) {
			var id = idObject;
			var test = await fiveMinuteTestRepository.GetByIdAsync(id);
			if (test == null)
				return Json(new { success = false, reason = $"Where is no element with id {id}" });
			if (await fiveMinuteTestRepository.Delete(test)) {
				var (templatesHtml, testsHtml, testCardsRowHtml) = await GetUpdatedHmtlTableString(currentUser);
				return Json(new { success = true, templatesHtml, testsHtml, testCardsRowHtml });
			}
			return Json(new { success = false, reason = "Occur some error while cascade element"});
		}

		private async Task<(string templatesHtml, string testsHtml, string testCardsRowHtml)> GetUpdatedHmtlTableString(AppUser currentUser) {
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

			var templatesHtml = await RenderPartialViewToString("_TemplatesTable", templates);
			var testsHtml = await RenderPartialViewToString("_TestsTable", tests);
			var testCardsRowHtml = await RenderPartialViewToString("_TestCardsRow", tests);
			return (templatesHtml, testsHtml, testCardsRowHtml);
		}
		
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
	}
}
