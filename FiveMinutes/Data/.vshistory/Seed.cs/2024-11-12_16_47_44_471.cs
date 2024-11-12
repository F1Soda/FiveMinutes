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

        public static async Task AddUser(IServiceScope serviceScope, string email, string name, string password, string role)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var adminUser = userManager.FindByEmailAsync(email);
            if (adminUser == null)
            {
                var newAdminUser = new AppUser()
                {
                    UserName = name,
                    Email = email,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(newAdminUser, password);
                await userManager.AddToRoleAsync(newAdminUser, role);
            }   
        }

        public static async Task SeedUsersDefailt(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                await AddUser(serviceScope, "golik.tima@gmail.com", "golikdev", "Coding@1234?", UserRoles.Admin);
                //AddUser(applicationBuilder, "ivan.ivanovich@yandex.ru", "Иван Иванович", "123456", UserRoles.Student);
                //AddUser(applicationBuilder, "michail.zukov@kontur.ru", "Михаил Зюков", "123456", UserRoles.Student);
                //AddUser(applicationBuilder, "maria.filatova@mail.ru", "Мария Филатова", "123456", UserRoles.Teacher);
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
