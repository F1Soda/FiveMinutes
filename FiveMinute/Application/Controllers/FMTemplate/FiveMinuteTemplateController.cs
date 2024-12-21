using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.ViewModels.FMTEditViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers {
	public partial class FiveMinuteTemplateController : Controller {
		private readonly UserManager<AppUser> _userManager;
		private readonly IFiveMinuteTemplateRepository _fmTemplateRepository;
		private readonly IUserRepository _userRepository;

		public FiveMinuteTemplateController(
			UserManager<AppUser> userManager,
			IFiveMinuteTemplateRepository fmTemplateRepository,
			IUserRepository userRepository) {
			_userManager = userManager;
			_fmTemplateRepository = fmTemplateRepository;
			_userRepository = userRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Create() {
			var currentUser = await GetCurrentUser();
			if (currentUser == null || !currentUser.canCreate)
				return UnauthorizedAccessError("У вас нет прав на создание пятиминуток.");

			var newFmTemplate = FiveMinuteTemplate.CreateDefault(currentUser);
			if (await _fmTemplateRepository.Add(newFmTemplate)) {
				await _userRepository.AddFmTtoUser(newFmTemplate, currentUser);
				return RedirectToAction("Edit", new { newFmTemplate.Id });
			}

			return DatabaseOperationError("Не удалось создать пятиминутку");
		}

		public async Task<IActionResult> Edit(int id) {
			var fmt = await _fmTemplateRepository.GetByIdAsync(id);
			var currentUser = await GetCurrentUser();

			if (currentUser == null || !currentUser.canCreate)
				return UnauthorizedAccessError("У вас нет прав на редактирование пятиминутки");

			if (fmt == null)
				return NotFoundError();

			var fmtViewModel = FiveMinuteTemplateEditViewModel.CreateByModel(fmt);
			HttpContext.Session.SetInt32("FmtViewModel", fmt.Id);
			return View(fmtViewModel);
		}

		[HttpPost]
		public async Task<JsonResult> Save([FromBody] FiveMinuteTemplateEditViewModel fmt) {
			if (!ModelState.IsValid) {
				return JsonError(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
			}

			var currentFmTemplateId = HttpContext.Session.GetInt32("FmtViewModel");
			if (currentFmTemplateId == null) {
				return JsonError("No template ID found in session.");
			}

			var existingFmt = await _fmTemplateRepository.GetByIdAsyncNoTracking(currentFmTemplateId.Value);
			if (existingFmt == null) {
				return JsonError("Template not found.");
			}

			var template = FiveMinuteTemplateEditViewModel.CreateByView(fmt);
			await _fmTemplateRepository.Update(existingFmt, template);
			return Json(new { success = true, id = fmt.Id });
		}
	}
}