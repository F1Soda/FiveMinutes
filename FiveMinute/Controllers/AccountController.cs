﻿using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.AccountViewModels;
using FiveMinute.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Controllers
{
    public class AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ApplicationDbContext context)
        : Controller
    {
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

            var newUser = new AppUser
            {
                UserRole = UserRoles.Student,
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
                UserData = new UserData(registerViewModel.FirstName, registerViewModel.LastName, "")
            };
            var newUserResponse = await userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, UserRoles.Student);
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
                return RedirectToAction("Login", "Account"); // Redirect to login if the user is not authenticated
            }

            if (currentUser.UserRole == UserRoles.Student && currentUser.Id != userId)
            {
                return View("Error", new ErrorViewModel("You can't view someone else's profile"));
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
            var user = await context.Users.Include(x => x.FMTTemplates)
                .Include(x => x.FMTests)
                .Include(appUser => appUser.PassedTestResults)
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return View("NotFound");
            }
            
            // Get the role of the user being viewed (if needed)
            var results = user.PassedTestResults
                .Select(result => new FiveMinuteTestResultViewModel
                {
                    FiveMinuteTestName = context.FiveMinuteTests
                        .FirstOrDefault(test => test.Id == result.FiveMinuteTestId)?.Name,
                    FiveMinuteTestResult = result,
                    Questions = context.FiveMinuteTests
                        .Include(test => test.FiveMinuteTemplate)
                            .ThenInclude(template => template.Questions)
                        .FirstOrDefault(test => test.Id == result.FiveMinuteTestId)
                        ?.FiveMinuteTemplate.Questions.ToList(),
                })
                .ToList();
            var model = new UserDetailViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FMTs = user.FMTTemplates,
                Tests = user.FMTests,
                UserRole = currentUser.UserRole,
                IsOwner = isOwner,
                UserData = user.UserData,
                PassedTestResults = results,
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
        public async Task<IActionResult> EditUser(UserData userData)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
                return Forbid();
            // а я хуй его знает
            if (currentUser.UserData is null)
                currentUser.UserData = new();
            currentUser.UserData.FirstName = userData.FirstName;
            currentUser.UserData.LastName = userData.LastName;
            currentUser.UserData.Group = userData.Group;
            
            await context.SaveChangesAsync();
            return RedirectToAction("Detail", "Account", new { userId = currentUser.Id });
        }
	}
}
