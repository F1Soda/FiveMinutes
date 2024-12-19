using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FiveMinute.ViewModels;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using System.Net;
using FiveMinute.Utils;


namespace FiveMinute.Controllers
{
	public class FiveMinuteTestController(
		UserManager<AppUser> userManager,
		IUserRepository userRepository,
		IFiveMinuteTestRepository fiveMinuteTestRepository,
		IFiveMinuteResultsRepository fiveMinuteResultsRepository,
		IChecker fmtChecker)
		: Controller
	{
		public async Task<IActionResult> Edit(int testId)
		{
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testId);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (fmTest == null)
				return View("NotFound");

			var fmTestViewModel = FiveMinuteTestEditViewModel.CreateByModel(fmTest);

			ViewData["TemplateId"] = fmTest.Id;
			return View(fmTestViewModel);
		}

		public IActionResult Passed()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(FiveMinuteTestEditViewModel fmTestEditViewModel)
		{
			var existingFMTest = await fiveMinuteTestRepository.GetByIdAsync(fmTestEditViewModel.Id);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null)
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			

			var updatedTest = FiveMinuteTestEditViewModel.CreateByView(fmTestEditViewModel);
			updatedTest.Status = existingFMTest.Status;
			updatedTest.Results = existingFMTest.Results;


			await fiveMinuteTestRepository.Update(updatedTest);

			return RedirectToAction("Detail", new { id = existingFMTest.Id });
		}

		public async Task<IActionResult> Detail(int testId)
		{
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testId);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (fmTest == null)
				return View("NotFound");

			return View(FiveMinuteTestDetailViewModel.CreateByModel(fmTest));
		}

		public async Task<IActionResult> Create(int templateId)
		{
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));

			ViewData["templateId"] = templateId;
			var user =await userRepository.GetUserById(currentUser.Id);
			var model = UserDetailViewModel.CreateByModel(user);
			
			return View(model);
		}
	
		[HttpPost]
		public async Task<IActionResult> Create(FiveMinuteTestDetailViewModel fmTestEditViewModel)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));
			
			var user = await userRepository.GetUserById(currentUser.Id);
			var attachedTemplate = user.FMTTemplates.FirstOrDefault(x => x.Id == fmTestEditViewModel.AttachedFMTId);

			var test = FiveMinuteTestDetailViewModel.CreateByView(fmTestEditViewModel);
			test.Status = Data.TestStatus.Started;
			test.IdToUninclude = new List<int>();
			test.UserOrganizerId = user.Id;
			test.UserOrganizer = user;
			test.FiveMinuteTemplate = attachedTemplate;
			test.FiveMinuteTemplateId = fmTestEditViewModel.AttachedFMTId;
			test.Results = new List<FiveMinuteTestResult>();
			test.CreationTime = DateTime.UtcNow;
			await fiveMinuteTestRepository.Add(test);
			return RedirectToAction("Detail", new { testId = test.Id});
		}

		public async Task<IActionResult> Pass(string encryptedId)
		{
			var testId = UrlEncryptor.Decrypt(encryptedId);
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testId);
			if (fmTest is null)
			{
				return View("NotFound");
			}

			var fmt = fmTest.FiveMinuteTemplate;
			
			var currentUser = await userManager.GetUserAsync(User);
			
			if (!fmTest.CanPass(currentUser))
				return Forbid();
			var test = FiveMinuteTestViewModel.CreateByModel(fmTest);
			return View(test);
		}

		[HttpPost]
		public async Task<IActionResult> SendTestResults(TestResultViewModel testResultViewModel)
		{
			var currentUser = await userManager.GetUserAsync(User);
			
			if (!await fmtChecker.CheckAndSave(currentUser, testResultViewModel))
				return View("Error", new ErrorViewModel($"Something is wrong. Could not save your answers")); ;
			await userRepository.Save();

			return RedirectToAction("Passed");
		}
		
		[HttpPost]
		public async Task<IActionResult> UpdateTestSettings(FiveMinuteTestDetailViewModel FMTestDetailView)
		{
			Console.WriteLine(FMTestDetailView.StartPlanned);

			var existingFMTest = await fiveMinuteTestRepository.GetByIdAsync(FMTestDetailView.Id);
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null) 
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (existingFMTest == null)
				return View("NotFound");
			var updatedTest = FiveMinuteTestDetailViewModel.CreateByView(FMTestDetailView);
			updatedTest.FiveMinuteTemplate = existingFMTest.FiveMinuteTemplate;
			updatedTest.FiveMinuteTemplateId = existingFMTest.FiveMinuteTemplate.Id;
			updatedTest.Results = existingFMTest.Results;
			if(!await fiveMinuteTestRepository.Update(updatedTest))
				return View("Error");
			
			return RedirectToAction("Detail", new { testId = updatedTest.Id});
		}
		
		public async Task<IActionResult> FiveMinuteResult(int resultId)
		{
			var currentUser = await userManager.GetUserAsync(User);
			var result = fiveMinuteResultsRepository.GetById(resultId).Result;
			var FMTest = fiveMinuteTestRepository.GetByIdAsync(result.FiveMinuteTestId).Result;
			var fiveMinuteTestResultViewModel = FiveMinuteTestResultViewModel.CreateByModel(FMTest);
			fiveMinuteTestResultViewModel.FiveMinuteTestResult = result;

			if (result is null || currentUser is null || (FMTest.UserOrganizerId != currentUser.Id && result.UserId != currentUser.Id))
				return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));

			return View(fiveMinuteTestResultViewModel);
		}
		
		[HttpPost]
		public async Task<IActionResult> UpdateAnswerCorrectness([FromBody] CheckTextAnswerCorrectnessViewModel model)
		{
			return Json(new { success = true });
		}
	}
}