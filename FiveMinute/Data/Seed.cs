using FiveMinute.Models;
using Microsoft.AspNetCore.Identity;

namespace FiveMinute.Data
{
    public class Seed
    { 
        public static async Task AddRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                if (!await roleManager.RoleExistsAsync(UserRoles.Teacher))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Teacher));
            }
        }

        public static async Task AddUser(UserManager<AppUser> userManager, string email, string name, string password, string role, UserData userData)
        {
            var adminUser = await userManager.FindByEmailAsync(email);
            if (adminUser == null)
            {
                var newUser = new AppUser()
                {
                    UserName = name,
                    Email = email,
                    EmailConfirmed = true,
                    UserRole = role,
                    UserData = userData
                };
                var res = await userManager.CreateAsync(newUser, password);
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, role);
                }
                else
                {
                    foreach (var error in res.Errors)
                        Console.WriteLine(error.Description);
                }
            }
                
        }

        public static async Task SeedUsersDefailt(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                await AddRoles(applicationBuilder);
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                await AddUser(userManager, "golik.tima@gmail.com", "golikdev", "Coding@13421212?", UserRoles.Admin, new UserData("Тимофей", "Голик", "ФТ-204"));
				await AddUser(userManager, "michail.zukov@kontur.ru", "Micha", "123456", UserRoles.Student, new UserData("Михаил", "Зюков", "ФТ-203"));
                await AddUser(userManager, "maria.filatova@mail.ru", "Maria", "123456", UserRoles.Teacher, new UserData("Мария", "Филатовна", "-"));
            }
        }

        public static async Task SeedAdmins(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "golik.tima@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserRole = UserRoles.Admin,
                        UserName = "golikdev",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        UserData = new UserData("Тимофей", "Голик", "ФТ-204")
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            }
        }
    }
}
