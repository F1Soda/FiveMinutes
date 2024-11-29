using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.Models;
using FiveMinute.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FiveMinute.Data;
using FiveMinute.Migrations;
using FiveMinute.ViewModels.AccountViewModels;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Controllers
{
	public class FiveMinuteTestController : Controller
	{
		private readonly UserManager<AppUser> userManager;
		private readonly ApplicationDbContext context;

		private readonly IFiveMinuteTestRepository fiveMinuteTestRepository;

		public FiveMinuteTestController(UserManager<AppUser> userManager, ApplicationDbContext context)
		{
			this.userManager = userManager;
			fiveMinuteTestRepository = new FiveMinuteTestRepository(context);
			this.context = context;
		}

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

		[HttpPost]
		public async Task<IActionResult> Edit(FiveMinuteTestEditViewModel fmTestEditViewModel)
		{
			var existingFMTest = await fiveMinuteTestRepository.GetByIdAsync(fmTestEditViewModel.Id);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (existingFMTest == null)
				return View("NotFound");

			var updatedTest = new FiveMinuteTest
			{
				Id = fmTestEditViewModel.Id,
				Name = fmTestEditViewModel.Name,
				AttachedFMT = fmTestEditViewModel.AttachedFMT,
				AttachedFMTId = fmTestEditViewModel.AttachedFMT.Id,
				Status = existingFMTest.Status,
				StartPlanned = fmTestEditViewModel.StartPlanned,
				StartTime = fmTestEditViewModel.StartTime,
				EndPlanned = fmTestEditViewModel.EndPlanned,
				EndTime = fmTestEditViewModel.EndTime,
				Results = existingFMTest.Results,
			};

			await fiveMinuteTestRepository.Update(existingFMTest, updatedTest);

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
			
			var user = await context.Users.Include(x => x.FMTTemplates).FirstOrDefaultAsync(x => x.Id == currentUser.Id);
			var attachedTemplate = user.FMTTemplates.FirstOrDefault(x => x.Id == fmTestEditViewModel.AttachedFMTId);
			var test = new FiveMinuteTest
			{
				Name = fmTestEditViewModel.Name,
				AttachedFMT = attachedTemplate,
				AttachedFMTId = fmTestEditViewModel.Id,
				UserOrganizer = user,
				UserOrganizerId = user.Id,
			};
			
			context.FiveMinuteTests.Add(test);
			await context.SaveChangesAsync();
			return RedirectToAction("Detail", "Account");
		}


		//public async Task<IActionResult> Pass(int testId)
		//{

		//}
	}
}