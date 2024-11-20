using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;

namespace FiveMinutes.Data
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

        public static async Task AddUser(UserManager<AppUser> userManager, string email, string name, string password, string role)
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
                };
                var res = await userManager.CreateAsync(newUser, password);
                if (res.Succeeded)
                {
                    Console.WriteLine($"YESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSs: {role}");
                    await userManager.AddToRoleAsync(newUser, role);
                }
                else
                {
                    Console.WriteLine("NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOo");
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
                await AddUser(userManager, "golik.tima@gmail.com", "golikdev", "Coding@13421212?", UserRoles.Admin);

                await AddUser(userManager, "g.t@gmail.com", "GolikTimofey", "Coding@1234?", UserRoles.Student);
				await AddUser(userManager, "michail.zukov@kontur.ru", "Micha", "123456", UserRoles.Student);
                await AddUser(userManager, "maria.filatova@mail.ru", "Maria", "123456", UserRoles.Teacher);
            }
        }

        public static async Task SeedAdmins(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                ////Roles
                //var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                //    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                //if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                //    await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                //if (!await roleManager.RoleExistsAsync(UserRoles.Teacher))
                //    await roleManager.CreateAsync(new IdentityRole(UserRoles.Teacher));

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
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            }
        }
    }
}
