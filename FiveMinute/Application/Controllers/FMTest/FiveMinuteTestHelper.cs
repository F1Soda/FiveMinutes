using FiveMinute.Models;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers;

public partial class FiveMinuteTestController {
	public class SwitchStatusRequestId {
		public int Id { get; set; }
	}

	
	private async Task<AppUser?> GetCurrentUser() {
		return await _userManager.GetUserAsync(User);
	}

	private async Task<(AppUser? currentUser, FiveMinuteTest? fmTest)> GetCurrentUserAndTest(int testId) {
		var currentUser = await GetCurrentUser();
		var fmTest = await _fiveMinuteTestRepository.GetByIdAsync(testId);
		return (currentUser, fmTest);
	}

	private IActionResult UnauthorizedAccessError() {
		return View("Error", new ErrorViewModel("You don't have the rights to this action"));
	}

	private IActionResult NotFoundError() {
		return View("NotFound");
	}
	
	private FiveMinuteTest CreateNewTest(FiveMinuteTestDetailViewModel viewModel, AppUser user,
	                                     FiveMinuteTemplate attachedTemplate) {
		return new FiveMinuteTest {
			Status = Data.TestStatus.Started,
			IdToUninclude = new List<int>(),
			UserOrganizerId = user.Id,
			UserOrganizer = user,
			Name = viewModel.Name,
			FiveMinuteTemplate = attachedTemplate,
			FiveMinuteTemplateId = viewModel.AttachedFMTId,
			Results = new List<FiveMinuteTestResult>(),
			CreationTime = DateTime.UtcNow
		};
	}

	private FiveMinuteTest UpdateTestFromViewModel(FiveMinuteTestDetailViewModel viewModel,
	                                               FiveMinuteTest existingFmTest) {
		var updatedTest = FiveMinuteTestDetailViewModel.CreateByView(viewModel);
		updatedTest.FiveMinuteTemplate = existingFmTest.FiveMinuteTemplate;
		updatedTest.FiveMinuteTemplateId = existingFmTest.FiveMinuteTemplate.Id;
		updatedTest.Results = existingFmTest.Results;
		updatedTest.Name = viewModel.kekForKek;

		if (viewModel.StartPlanned || viewModel.EndPlanned)
		{
			updatedTest.Status = Data.TestStatus.Planned;
		}
		else
		{
			updatedTest.Status = Data.TestStatus.Closed;
		}

		return updatedTest;
	}
}