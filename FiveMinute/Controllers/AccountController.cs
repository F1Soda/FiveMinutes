using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers
{
    public class AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IUserRepository userRepository,
        IFiveMinuteTestRepository fiveMinuteTestRepository)
        : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }   
                TempData["Error"] = "Wrond credentials. Please, try again";
                return View(loginViewModel);
            }
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This emal address is already is use";
                return View(registerViewModel);
            }
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                TempData["Error"] = "The passwords are not the same";
                return View(registerViewModel);
            }

            var newUser = new AppUser
            {
                UserRole = UserRoles.Student,
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
                UserData = new UserData(registerViewModel.FirstName, registerViewModel.LastName, registerViewModel.Group)
            };
            var newUserResponse = await userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, UserRoles.Student);
				await signInManager.SignInAsync(newUser, isPersistent: false);

				return RedirectToAction("Index", "Home");
            }
            else if (newUserResponse.Errors.First().Description.Contains("Password"))
            {
                TempData["Error"] = "Your password is too short";
                return View(registerViewModel);
            }
            else
            {
                TempData["Error"] = "Enter the correct email";
                return View(registerViewModel);
            }
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
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (currentUser.UserRole == UserRoles.Student && currentUser.Id != userId)
            {
                return View("Error", new ErrorViewModel("You can't view someone else's profile"));
            }

            var currentUserRoles = await userManager.GetRolesAsync(currentUser);

            bool isAdmin = currentUserRoles.Contains(UserRoles.Admin);
            bool isTeacher = currentUserRoles.Contains(UserRoles.Teacher);
            bool isOwner = currentUser.Id == userId;

            if (!(isAdmin || isTeacher || isOwner)) {
                return Forbid();
            }

            var user = await userRepository.GetUserById(userId);
            if (user == null)
            {
                return View("NotFound");
            }

            var model = UserDetailViewModel.CreateByModel(user);
            foreach (var result in model.PassedTestResults)
            {
                var fmtest = await fiveMinuteTestRepository.GetByIdAsync(result.FiveMinuteTestId);
                result.FMTestName = fmtest.Name;
			}

            model.UserRole = currentUser.UserRole;
            model.IsOwner = isOwner;
            return View(model);
        }

		public async Task<IActionResult> All()
		{
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null)
			{
				return Forbid();
			}

			var currentUserRoles = await userManager.GetRolesAsync(currentUser);

            if (currentUserRoles.Contains(UserRoles.Admin))
                return Forbid();
            
			var users = userManager.Users.ToList();


            var allUsersViewModel = new AllUsersViewModel
            {
                Admins = users.Where(u => u.UserRole == UserRoles.Admin).Select(u => new UserIdNameEmailViewModel (u.Id, u.UserName, u.Email)).ToList(),
                Teachers = users.Where(u => u.UserRole == UserRoles.Teacher).Select(u => new UserIdNameEmailViewModel (u.Id, u.UserName, u.Email)).ToList(),
                Students = users.Where(u => u.UserRole == UserRoles.Student).Select(u => new UserIdNameEmailViewModel (u.Id, u.UserName, u.Email)).ToList(),

            };

            return View(allUsersViewModel);
        }

        public class DeleteUserRequest
        {
            public string UserId { get; set; }
        }

        [HttpPost]
        public async Task<JsonResult> Delete([FromBody] DeleteUserRequest request)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.UserRole != UserRoles.Admin)
            {
                return Json(new
                {
                    success = false,
                    description = "Not Allowed"
                });
            }

            var userToDelete = await userManager.FindByIdAsync(request.UserId);

            if (userToDelete == null)
            {
                return Json(new
                {
                    success = false,
                    description = "Not Found"
                });
            }

            var res = await userManager.DeleteAsync(userToDelete);
            if (res.Succeeded)
            {
                return Json(new
                {
                    success = true
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    description = "Fail to delete user from db"
                });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditUser(UserDataChangeViewModel userDataChange)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
                return Forbid();

            currentUser.UserData.FirstName = userDataChange.FirstName;
            currentUser.UserData.LastName = userDataChange.LastName;
            currentUser.UserData.Group = userDataChange.Group;
            currentUser.Email = userDataChange.Email;
            
            await userRepository.Save();
            return RedirectToAction("Detail", "Account", new { userId = currentUser.Id });
        }
	}
}
