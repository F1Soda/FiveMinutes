using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository.FMTestRepository;
using FiveMinute.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers
{
    public partial class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IUserRepository userRepository;
        private readonly IFiveMinuteTestRepository fiveMinuteTestRepository;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IUserRepository userRepository,
            IFiveMinuteTestRepository fiveMinuteTestRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userRepository = userRepository;
            this.fiveMinuteTestRepository = fiveMinuteTestRepository;
        }

        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginViewModel.Password))
            {
                SetTempDataError("Нерпавильные учетные данные. Попробуйте снова");
                return View(loginViewModel);
            }

            var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            SetTempDataError("Нерпавильные учетные данные. Попробуйте снова");
            return View(loginViewModel);
        }

        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var userExists = await userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (userExists != null)
            {
                SetTempDataError("Данный электронный адрес уже используется.");
                return View(registerViewModel);
            }

            if (!PasswordsMatch(registerViewModel.Password, registerViewModel.ConfirmPassword))
            {
                SetTempDataError("Пароли не одинаковые.");
                return View(registerViewModel);
            }

            var newUser = CreateNewUser(registerViewModel);
            var createUserResult = await userManager.CreateAsync(newUser, registerViewModel.Password);

            if (createUserResult.Succeeded)
            {
                await AssignStudentRoleAndSignIn(newUser);
                return RedirectToAction("Index", "Home");
            }

            HandleUserCreationErrors(createUserResult);
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Detail(string userId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            if (!CanViewUserProfile(currentUser, userId))
                return View("Error", new ErrorViewModel("Вы не можете просматривать профили других пользователей"));
            
            var user = await userRepository.GetFullUserDataById(userId);
            if (user == null) return View("NotFound");

            var model = await BuildUserDetailViewModel(user, currentUser, userId);
            return View(model);
        }

		[HttpPost]
		public async Task<JsonResult> EditUser([FromBody] UserDataChangeViewModel userDataChange)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null) return Json(new { success = false, exception = "Not allowed" });

			UpdateUserData(currentUser, userDataChange);

			var res = await userRepository.Save();
			if (!res)
			{
				return Json(new { success = false, exception = res.Exception });
			}

			// Return success JSON response
			return Json(new { success = true, message = "User data updated successfully!" });
		}
	}
}
