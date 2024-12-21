using FiveMinute.Data;
using Microsoft.AspNetCore.Mvc;
using FiveMinute.Models;

namespace FiveMinute.Controllers;

public partial class FiveMinuteTemplateController {
	private async Task<AppUser?> GetCurrentUser() {
		return await _userManager.GetUserAsync(User);
	}

	private IActionResult UnauthorizedAccessError(string message) {
		return View("Error", new ErrorViewModel(message));
	}

	private IActionResult NotFoundError() {
		return View("NotFound");
	}

	private IActionResult DatabaseOperationError(string message) {
		return View("Error", new ErrorViewModel(message));
	}

	private JsonResult JsonError(IEnumerable<string> errors) {
		return Json(new { success = false, errors });
	}

	private JsonResult JsonError(string error) {
		return Json(new { success = false, errors = new[] { error } });
	}

	private async Task<bool> IsStudent(AppUser currentUser) {
		var roles = await _userManager.GetRolesAsync(currentUser);
		return roles.Contains(UserRoles.Student);
	}
}