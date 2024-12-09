using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FiveMinute.Data;
using FiveMinute.ViewModels.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using FiveMinute.ViewModels;
using FiveMinute.Interfaces;
using System.Net;
using System.Runtime.Intrinsics.X86;

namespace FiveMinute.Controllers
{
	public class FiveMinuteTestController(
		UserManager<AppUser> userManager,
		ApplicationDbContext context,
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

			if (currentUser == null) // || !canCreate
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (existingFMTest == null)
				return View("NotFound");

			var updatedTest = FiveMinuteTestEditViewModel.CreateByView(fmTestEditViewModel);
			updatedTest.Status = existingFMTest.Status;
			updatedTest.Results = existingFMTest.Results;


			await fiveMinuteTestRepository.Update(existingFMTest,updatedTest);//не кулл надо бы update переделать так, чтобы одну принимал модель

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
			
			var user = await context.Users.Include(x => x.FMTTemplates).FirstOrDefaultAsync(x => x.Id == currentUser.Id);
			var model = new UserDetailViewModel
			{
				UserName = user.UserName,
				Email = user.Email,
				FMTs = user.FMTTemplates,
				UserRole = user.UserRole,
				IsOwner = true,
			};
			
			return View(model);
		}
	
		[HttpPost]
		public async Task<IActionResult> Create(FiveMinuteTestDetailViewModel fmTestEditViewModel)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));
			
			var user = await context.Users.Include(x => x.FMTTemplates).ThenInclude(x => x.Questions).FirstOrDefaultAsync(x => x.Id == currentUser.Id);
			var attachedTemplate = user.FMTTemplates.FirstOrDefault(x => x.Id == fmTestEditViewModel.AttachedFMTId);

			var test = FiveMinuteTestDetailViewModel.CreateByView(fmTestEditViewModel);//#Ы Мб стоит создать другую view модель
			test.IdToUninclude = new List<int>();
			test.FiveMinuteTemplate = attachedTemplate;
			test.FiveMinuteTemplateId = fmTestEditViewModel.AttachedFMTId;
			test.Results = new List<FiveMinuteTestResult>();
			test.CreationTime = DateTime.UtcNow;
			context.FiveMinuteTests.Add(test);
			await context.SaveChangesAsync();
			return RedirectToAction("Detail", new { testId = test.Id});
		}

		public async Task<IActionResult> Pass(int testId)
		{
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
			await context.SaveChangesAsync();

			return RedirectToAction("Passed");
		}
		
		[HttpPost]
		public async Task<IActionResult> UpdateTestSettings(FiveMinuteTestDetailViewModel FMTestDetailView)
		{
			var existingFMTest = await fiveMinuteTestRepository.GetByIdAsync(FMTestDetailView.Id);
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null) // || !canCreate
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (existingFMTest == null)
				return View("NotFound");
			var updatedTest = FiveMinuteTestDetailViewModel.CreateByView(FMTestDetailView);
			updatedTest.FiveMinuteTemplate = existingFMTest.FiveMinuteTemplate;
			updatedTest.FiveMinuteTemplateId = existingFMTest.FiveMinuteTemplate.Id;
			updatedTest.Results = existingFMTest.Results;
			if(!await fiveMinuteTestRepository.Update(existingFMTest,updatedTest))
				return View("Error");
			
			return RedirectToAction("Detail", new { testId = updatedTest.Id});
		}
		
		// Statistics
		// public async Task<IActionResult> ShowResults(int testId)
		// {
		// 	
		// }

		public async Task<IActionResult> FiveMinuteResult(int resultId)
		{
			var currentUser = await userManager.GetUserAsync(User);
			var result = fiveMinuteResultsRepository.GetById(resultId).Result;
			var FMTest = fiveMinuteTestRepository.GetByIdAsync(result.FiveMinuteTestId).Result;
			var fiveMinuteTestResultViewModel = FiveMinuteTestResultViewModel.CreateByModel(FMTest);
			fiveMinuteTestResultViewModel.FiveMinuteTestResult = result;

			if (result is null || currentUser is null || FMTest.UserOrganizerId != currentUser.Id)
				return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));

			return View(fiveMinuteTestResultViewModel);
		}
	}
}