using FiveMinutes.Data;
using FiveMinutes.Models;
using FiveMinutes.ViewModels;
using FiveMinutes.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Controllers
{
    public class AccountController : Controller
    { 
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Типо прикол в том, что создав отдельную переменную, мы избавили пользователя от случая
            // когда он случайно во время ввода может перезагрузить страницу и его пароли снова слетят
            // и надо будет заново вводить 
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
                // TODO: Сделать позже это поле
                // loginViewModel.PasswordIsCorrect = false;
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

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };
            var newUserResponse = await userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, UserRoles.Student);
                return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Login", "Account"); // Redirect to login if the user is not authenticated
            }

            // Get current user roles
            var currentUserRoles = await userManager.GetRolesAsync(currentUser);

            // Allow access if the current user is Admin or Teacher, or if they are viewing their own profile
            bool isAdmin = currentUserRoles.Contains(UserRoles.Admin);
            bool isTeacher = currentUserRoles.Contains(UserRoles.Teacher);
            bool isOwner = currentUser.Id == userId;

            if (!(isAdmin || isTeacher || isOwner))
            {
                // Redirect students trying to view other users' profiles
                return Forbid(); // or RedirectToAction("AccessDenied") if you have an Access Denied page
            }

            // Fetch the user being viewed
            var user = await context.Users.Include(x => x.FMTs).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return RedirectToAction("NotFound");
            }

            // Get the role of the user being viewed (if needed)

            var model = new UserDetailViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FMTs = user.FMTs,
                UserRole = currentUser.UserRole,
                IsOwner = isOwner,

            };

            return View(model);
        }

		public async Task<IActionResult> All()
		{
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null)
			{
				return Forbid(); // Redirect to login if the user is not authenticated
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

        public async Task<JsonResult> Delete(string userId)
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

            var userToDelete = await userManager.GetUserIdAsync(User);

            userManager.DeleteAsync

        }
	}
}
