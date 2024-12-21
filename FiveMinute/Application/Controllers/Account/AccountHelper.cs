using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;


namespace FiveMinute.Controllers;

public partial class AccountController {
	private void SetTempDataError(string message) {
		TempData["Error"] = message;
	}

	private bool PasswordsMatch(string password, string confirmPassword) {
		return password == confirmPassword;
	}

	private AppUser CreateNewUser(RegisterViewModel registerViewModel) {
		return new AppUser {
			UserRole = UserRoles.Student,
			Email = registerViewModel.EmailAddress,
			UserName = registerViewModel.EmailAddress,
			UserData = new UserData(registerViewModel.FirstName, registerViewModel.LastName,
			                        registerViewModel.Group)
		};
	}

	private async Task AssignStudentRoleAndSignIn(AppUser newUser) {
		await userManager.AddToRoleAsync(newUser, UserRoles.Student);
		await signInManager.SignInAsync(newUser, isPersistent: false);
	}

	private void HandleUserCreationErrors(IdentityResult result) {
		var errorMessage = result.Errors.Any(e => e.Description.Contains("Password"))
			? "Паролль слишком короткий"
			: "Введите корректную электронную почту";

		SetTempDataError(errorMessage);
	}

	private bool CanViewUserProfile(AppUser currentUser, string userId) {
		if (currentUser.UserRole == UserRoles.Student && currentUser.Id != userId) return false;

		var roles = userManager.GetRolesAsync(currentUser).Result;
		return roles.Contains(UserRoles.Admin) || roles.Contains(UserRoles.Teacher) || currentUser.Id == userId;
	}

	private async Task<UserDetailViewModel> BuildUserDetailViewModel(AppUser user, AppUser currentUser, string userId) {
		var model = UserDetailViewModel.CreateByModel(user);
		foreach (var result in model.PassedTestResults) {
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(result.FiveMinuteTestId);
			result.FMTestName = fmTest!.Name;
		}

		model.UserRole = currentUser.UserRole;
		model.IsOwner = currentUser.Id == userId;

		return model;
	}

	private void UpdateUserData(AppUser currentUser, UserDataChangeViewModel userDataChange) {
		currentUser.UserData.FirstName = userDataChange.FirstName;
		currentUser.UserData.LastName = userDataChange.LastName;
		currentUser.UserData.Group = userDataChange.Group;
		currentUser.Email = userDataChange.Email;
	}
}