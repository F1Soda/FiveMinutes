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

			var updatedTest = new FiveMinuteTest
			{
				Id = fmTestEditViewModel.Id,
				Name = fmTestEditViewModel.Name,
				FiveMinuteTemplate = fmTestEditViewModel.AttachedFMT,
				FiveMinuteTemplateId = fmTestEditViewModel.AttachedFMT.Id,
				Status = existingFMTest.Status,
				StartPlanned = fmTestEditViewModel.StartPlanned,
				StartTime = fmTestEditViewModel.StartTime,
				EndPlanned = fmTestEditViewModel.EndPlanned,
				EndTime = fmTestEditViewModel.EndTime,
				Results = existingFMTest.Results,
			};

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

			var test = new FiveMinuteTest
			{
				Name = fmTestEditViewModel.Name,
				FiveMinuteTemplate = attachedTemplate,
				FiveMinuteTemplateId = fmTestEditViewModel.AttachedFMTId,
				UserOrganizer = user,
				UserOrganizerId = user.Id,
				Results = new List<FiveMinuteTestResult>(),
				CreationTime = DateTime.UtcNow,
				IdToUninclude = new List<int>(),
			};
			
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
			var test = new FiveMinuteTestViewModel
			{
				Name = fmt.Name,
				FMTestId = fmTest.Id,
				Questions = fmt.Questions.Where(x => !fmTest.IdToUninclude.Contains(x.Id))
										 .Select(x => new QuestionViewModel
				{
					Id = x.Id,
					Position = x.Position,
					QuestionText = x.QuestionText,
					ResponseType = x.ResponseType,
					AnswerOptions = x.AnswerOptions.Select(answer => new AnswerViewModel()
					{
						Id = answer.Id,
						QuestionId = answer.QuestionId,
						Position = answer.Position,
						Text = answer.Text,
					}).ToList(),
				}),
			};
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
		public async Task<IActionResult> UpdateTestSettings(FiveMinuteTestDetailViewModel FMTestDetailViewModel)
		{
			var existingFMTest = await fiveMinuteTestRepository.GetByIdAsync(FMTestDetailViewModel.Id);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null) // || !canCreate
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (existingFMTest == null)
				return View("NotFound");

			var updatedTest = new FiveMinuteTest
			{
				Id = FMTestDetailViewModel.Id,
				Name =FMTestDetailViewModel.Name!=null?FMTestDetailViewModel.Name:existingFMTest.Name,
				FiveMinuteTemplate = existingFMTest.FiveMinuteTemplate,
				FiveMinuteTemplateId = existingFMTest.FiveMinuteTemplate.Id,
				Status = FMTestDetailViewModel.Status!=null?FMTestDetailViewModel.Status:existingFMTest.Status,
				StartPlanned = FMTestDetailViewModel.StartPlanned!=null?FMTestDetailViewModel.StartPlanned:existingFMTest.StartPlanned,
				StartTime = FMTestDetailViewModel.StartTime!=null?FMTestDetailViewModel.StartTime:existingFMTest.StartTime,
				EndPlanned = FMTestDetailViewModel.EndPlanned!=null?FMTestDetailViewModel.EndPlanned:existingFMTest.EndPlanned,
				EndTime = FMTestDetailViewModel.EndTime!=null?FMTestDetailViewModel.EndTime:existingFMTest.EndTime,
				IdToUninclude =FMTestDetailViewModel.IdToUninclude,
				Results = existingFMTest.Results
			};
			await fiveMinuteTestRepository.Update(existingFMTest,updatedTest);
			return RedirectToAction("Detail");
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
			var fiveMinuteTestResultViewModel = new FiveMinuteTestResultViewModel
			{
				FiveMinuteTestName = FMTest.Name,
				FiveMinuteTestResult = result,
				Questions = FMTest.FiveMinuteTemplate.Questions.ToList()
			};

			if (result is null || currentUser is null || FMTest.UserOrganizerId != currentUser.Id)
				return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));

			return View(fiveMinuteTestResultViewModel);
		}
	}
}