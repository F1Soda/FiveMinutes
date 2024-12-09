using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository;
using FiveMinute.ViewModels.FMTEditViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers
{
	public class FiveMinuteTemplateController(
		UserManager<AppUser> userManager,
		IFiveMinuteTemplateRepository fmTemplateReposity,
		IUserRepository userRepository)
		: Controller
	{
		private readonly ILogger<HomeController> _logger;

		public IActionResult Index()
		{
			return View();
		}


		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights to create a five-minute"));

			var newFMT = FiveMinuteTemplate.CreateDefault(currentUser);
			if (fmTemplateReposity.Add(newFMT).Result)
			{
				await userRepository.AddFMTtoUser(newFMT, currentUser);

				return RedirectToAction("Edit", new { newFMT.Id });
			}
			return View("Error", new ErrorViewModel("Fail to add FMT to db"));
		}


		public async Task<IActionResult> Edit(int id)
		{
			var fmt = await fmTemplateReposity.GetByIdAsync(id);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights to create a five-minute"));

			if (fmt == null)
				return View("NotFound");

			var fmtViewModel = FiveMinuteTemplateEditViewModel.CreateByModel(fmt);

			// Change !!!
			HttpContext.Session.SetInt32("FmtViewModel", fmt.Id);
			return View(fmtViewModel);
		}

		public IActionResult TestCreation()
		{
			return View();
		}

		public IActionResult AllFiveMinuteTemplates()
		{
			return View();
		}

		public IActionResult FiveMinuteFolder()
		{
			return View();
		}

		public async Task<JsonResult> Save([FromBody] FiveMinuteTemplateEditViewModel fmt)
		{
			if (!ModelState.IsValid)
			{
				return Json(new
				{
					success = false
				});
			}

			var currentFMTId = HttpContext.Session.GetInt32("FmtViewModel");
			if (currentFMTId is null)
			{
				return Json(new
				{
					success = false,
					errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
				});
			}

            var existingFmt = await fmTemplateReposity.GetByIdAsyncNoTracking(currentFMTId.Value);
            if (existingFmt is null)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                }); //тут какая-то другая ошибка должна быть
            }
            var template = FiveMinuteTemplateEditViewModel.CreateByView(fmt);//#Ы в view модели у всех вопросов id =0,скорее всего гадость с фронта приходит 
            await fmTemplateReposity.Update(existingFmt, template);
            return Json(new { success = true, id = fmt.Id });
        }
		public async Task<IActionResult> Copy(int testId)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null)
			{
				return RedirectToAction("Login", "Account"); // Redirect to login if the user is not authenticated
			}

			var currentUserRoles = await userManager.GetRolesAsync(currentUser);

			bool isStudent = currentUserRoles.Contains(UserRoles.Student);

			if (isStudent)
			{
				// Redirect students trying to view other users' profiles
				return Forbid(); // or RedirectToAction("AccessDenied") if you have an Access Denied page
			}

			var fmt = await fmTemplateReposity.GetByIdAsync(testId);

			if (fmt == null)
			{
				return RedirectToAction("NotFound");
			}

			var copyFMT = fmt.GetCopyToUser(currentUser);

			if (fmTemplateReposity.Add(copyFMT).Result)
			{
				await userRepository.AddFMTtoUser(copyFMT, currentUser);
				return RedirectToAction("Edit", new { copyFMT.Id });
			}
			return View("Error", new ErrorViewModel("Fail to add FMT to db"));
		}
	}
}
