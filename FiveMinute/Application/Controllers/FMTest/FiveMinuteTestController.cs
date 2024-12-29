using FiveMinute.Repository.FMTestRepository;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FiveMinute.ViewModels;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Utils;
using System.Net;
using FiveMinute.Data;

namespace FiveMinute.Controllers {
	public partial class FiveMinuteTestController : Controller {
		private readonly UserManager<AppUser> _userManager;
		private readonly IUserRepository _userRepository;
		private readonly IFiveMinuteTestRepository _fiveMinuteTestRepository;
		private readonly IFiveMinuteResultsRepository _fiveMinuteResultsRepository;
		private readonly IChecker _fmtChecker;

		public FiveMinuteTestController(
			UserManager<AppUser> userManager,
			IUserRepository userRepository,
			IFiveMinuteTestRepository fiveMinuteTestRepository,
			IFiveMinuteResultsRepository fiveMinuteResultsRepository,
			IChecker fmtChecker) {
			_userManager = userManager;
			_userRepository = userRepository;
			_fiveMinuteTestRepository = fiveMinuteTestRepository;
			_fiveMinuteResultsRepository = fiveMinuteResultsRepository;
			_fmtChecker = fmtChecker;
		}

		public IActionResult Passed() => View();

		public async Task<IActionResult> Detail(int testId) {
			var (currentUser, fmTest) = await GetCurrentUserAndTest(testId);
			if (currentUser == null || !currentUser.canCreate) return UnauthorizedAccessError();

			if (fmTest == null) return NotFoundError();

			return View(FiveMinuteTestDetailViewModel.CreateByModel(fmTest));
		}

		public async Task<IActionResult> Create(int templateId) {
			var currentUser = await GetCurrentUser();
			if (currentUser == null || !currentUser.canCreate) return UnauthorizedAccessError();

			ViewData["TemplateId"] = templateId;
			var user = await _userRepository.GetFullUserDataById(currentUser.Id);

			return View(user!.FMTemplates);
		}

		[HttpPost]
		public async Task<IActionResult> Create(FiveMinuteTestDetailViewModel viewModel) {
			var currentUser = await GetCurrentUser();
			if (currentUser == null || !currentUser.canCreate) return UnauthorizedAccessError();

			var user = await _userRepository.GetFullUserDataById(currentUser.Id);
			var attachedTemplate = user!.FMTemplates.FirstOrDefault(x => x.Id == viewModel.AttachedFMTId);

			var newTest = CreateNewTest(viewModel, user, attachedTemplate!);
			await _fiveMinuteTestRepository.Add(newTest);
			return RedirectToAction("Detail", new { testId = newTest.Id });
		}

		public async Task<IActionResult> Pass(string encryptedId) {
			var testId = UrlEncryptor.Decrypt(encryptedId);
			var fmTest = await _fiveMinuteTestRepository.GetByIdAsync(testId);
			if (fmTest == null) return NotFoundError();

			var currentUser = await _userManager.GetUserAsync(User);
			if (!fmTest.CanPass(currentUser)) 
				return View("Error", new ErrorViewModel($"Невозможно пройти пятиминутку, так как она закончилась, либо еще не началась"));

			var viewModel = FMTestPassingViewModel.CreateByModel(fmTest);
			if (currentUser != null && User.Identity!.IsAuthenticated) {
				viewModel.UserData = currentUser.UserData;
				viewModel.userId = currentUser.Id;
			}

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> SendTestResults(TestResultViewModel viewModel) {
			if (!await _fmtChecker.CheckAndSave(viewModel))
				return View("Error", new ErrorViewModel("Something is wrong. Could not save your answers"));
			return RedirectToAction("Passed");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateTestSettings(FiveMinuteTestDetailViewModel viewModel) {
			var (currentUser, existingFmTest) = await GetCurrentUserAndTest(viewModel.Id);
			if (currentUser == null || !currentUser.canCreate) return UnauthorizedAccessError();

			if (existingFmTest == null) return NotFoundError();

			var updatedTest = UpdateTestFromViewModel(viewModel, existingFmTest);
			var status = await _fiveMinuteTestRepository.Update(updatedTest);
			if (!status) return View("Error", new ErrorViewModel($"ERROR: {status.Exception}"));

			return RedirectToAction("Detail", new { testId = updatedTest.Id });
		}
		
		[HttpPost]
		public async Task<IActionResult> ActivateTest([FromBody] SwitchStatusRequestId switchStatusRequestId) {
			var (currentUser, existingFmTest) = await GetCurrentUserAndTest(switchStatusRequestId.Id);
			if (currentUser == null) return Json(new { success = false, exception = "Not allowed"});

			if (existingFmTest == null) return Json(new { success = false, exception = "Not Found"});
			
			existingFmTest.Status = TestStatus.Started;
			var status = await _fiveMinuteTestRepository.Update(existingFmTest);
			if (!status) return Json(new { success = false, exception = status.Exception});

			return Json(new { success = true, exception = ""});
		}
		
		[HttpPost]
		public async Task<JsonResult> DeactivateTest([FromBody] SwitchStatusRequestId switchStatusRequestId) {
			var (currentUser, existingFmTest) = await GetCurrentUserAndTest(switchStatusRequestId.Id);
			if (currentUser == null) return Json(new { success = false, exception = "Not allowed"});

			if (existingFmTest == null) return Json(new { success = false, exception = "Not Found"});
			
			existingFmTest.Status = TestStatus.Completed;
			foreach (var question in existingFmTest.FiveMinuteTemplate.Questions) {
				if (question.ResponseType == ResponseType.Text) {
					existingFmTest.Status = TestStatus.InRechekingProcess;
					break;
				}
			}
			var status = await _fiveMinuteTestRepository.Update(existingFmTest);
			if (!status) return Json(new { success = false, exception = status.Exception});

			return Json(new { success = true, exception = ""});
		}

		public async Task<IActionResult> FiveMinuteResult(int resultId) {
			var currentUser = await GetCurrentUser();
			var result = await _fiveMinuteResultsRepository.GetById(resultId);
			var fmTest = await _fiveMinuteTestRepository.GetByIdAsync(result?.FiveMinuteTestId ?? 0);

			if (result == null || currentUser == null ||
			    (fmTest?.UserOrganizerId != currentUser.Id && result.UserId != currentUser.Id)) {
				return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));
			}

			var viewModel = FiveMinuteTestResultViewModel.CreateByModel(fmTest!);
			viewModel.FiveMinuteTestResult = result;

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAnswerCorrectness([FromBody] CheckTextAnswerCorrectnessViewModel model) {
			var test = await _fiveMinuteTestRepository.GetByIdAsync(model.TestId);
			var answers = test?.Results
			                  .SelectMany(r => r.Answers)
			                  .Where(a => a.Text == model.Text);

			if (answers != null) {
				foreach (var answer in answers) {
					answer.IsCorrect = model.IsCorrect;
				}

				await _fiveMinuteTestRepository.Save();
			}

			return Json(new { success = true });
		}
	}
}