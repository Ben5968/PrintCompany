using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace PrintCompany

{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            {
                //adding customs roles
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string[] roleNames = { "Admin", "Manager", "Member" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    // creating the roles and seeding them to the database
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
                // creating a super user who could maintain the web app
            var poweruser = new IdentityUser
            {
                UserName = "yusuf.gebologlu@googlemail.com",
                Email = "yusuf.gebologlu@googlemail.com"
                //UserName = "ellie@logosports.co.uk",
                //Email = "ellie@logosports.co.uk",

                //UserName = Configuration.GetSection("AppSettings")["UserEmail"],                
                //Email = Configuration.GetSection("AppSettings")["UserEmail"]
            };
                //string userPassword = Configuration.GetSection("AppSettings")["UserPassword"];
                string userPassword = "P@ssw0rd";
                //var user = await UserManager.FindByEmailAsync(Configuration.GetSection("AppSettings")["UserEmail"]);
                var user = await UserManager.FindByEmailAsync(poweruser.Email);
                if (user == null)
                {
                    var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                    if (createPowerUser.Succeeded)
                    {
                        // here we assign the new user the "Admin" role 
                        await UserManager.AddToRoleAsync(poweruser, "Admin");
                    }
                }
            }
        }
    }
}
