using Microsoft.AspNetCore.Mvc;
using FiveMinute.Models;
using FiveMinute.ViewModels.HomeViewModels;
using FiveMinute.ViewModels.FMTEditViewModels;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FiveMinute.Controllers;

public partial class HomeController {
	public class DeleteRequestId {
		public int Id { get; set; }
	}

	private async Task<IActionResult> TryDeleteEntity(int entityId, AppUser currentUser, dynamic repository) {
		var test = await repository.GetByIdAsync(entityId);
		if (test == null)
			return Json(new { success = false, reason = $"Where is no element with id {entityId}" });
		if (await repository.Delete(test)) {
			var (templatesHtml, testsHtml, testCardsRowHtml) = await GetUpdatedHmtlTableString(currentUser);
			return Json(new { success = true, templatesHtml, testsHtml, testCardsRowHtml });
		}

		return Json(new { success = false, reason = "Occur some error while cascade element" });
	}

	private async Task<(string templatesHtml, string testsHtml, string testCardsRowHtml)> GetUpdatedHmtlTableString(
		AppUser currentUser) {
		var templates = fiveMinuteTemplateRepository.GetAllFromUserId(currentUser.Id)
		                                            ?.OrderByDescending(x => x.LastModificationTime)
		                                            ?.Select(FMTemplateIndexViewModel.CreateByModel)?.ToList();
		var tests = fiveMinuteTestRepository.GetAllFromUserId(currentUser.Id)
		                                    ?.OrderByDescending(x => x.CreationTime)
		                                    ?.Select(FMTestIndexViewModel.CreateByModel)?.ToList();

		var templatesHtml = await RenderPartialViewToString("_TemplatesTable", templates);
		var testsHtml = await RenderPartialViewToString("_TestsTable", tests);
		var testCardsRowHtml = await RenderPartialViewToString("_TestCardsRow", tests);
		return (templatesHtml, testsHtml, testCardsRowHtml);
	}

	private async Task<string> RenderPartialViewToString(string viewName, object model) {
		ViewData.Model = model;

		using (var sw = new StringWriter()) {
			var viewResult = viewEngine.FindView(ControllerContext, viewName, false);
			if (!viewResult.Success) {
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

	private async Task<IndexViewModel> PrepareUserDataIndexViewModel(AppUser currentUser) {
		var fullUserData = await userRepository.GetFullUserDataById(currentUser.Id);
		var model = IndexViewModel.CreateByModel(fullUserData);
		foreach (var result in model.FMTResults) {
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(result.FiveMinuteTestId);
			result.FMTestName = fmTest!.Name;
			result.FMTestOrganizer =
				$"{fmTest.UserOrganizer!.UserData.FirstName} {fmTest.UserOrganizer.UserData.LastName}";
		}

		return model;
	}
}