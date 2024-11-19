using FiveMinutes.Data;
using FiveMinutes.Models;
using FiveMinutes.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Detail(string id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                // По хорошему сюда надо PageNotFounded
                return RedirectToAction("Index", "Home");
            }
            // И возможно не нужна сразу все пятиминутки с тестами отправлять
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = id,
                UserName = user.UserName,
                FMTs = user.FMTs,
                Tests = user.Tests
            };
            return View(userDetailViewModel);
        }
    }
}
