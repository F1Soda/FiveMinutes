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
			if (!ModelState.IsValid)
			{

			}
			return View(login);
		}
	}
}
