using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SpeakingInBits.Models
{
    public static class IdentityHelper
    {
        //Role Names
        public const string Instructor = "Instructor";
        public const string Student = "Stu";

        public static void SetIdentityOptions(IdentityOptions options)
        {
            //Setting signin options
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            //Set password strength
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;

            //Set Lockout options
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 5;
        }

        public static async Task CreateRoles(IServiceProvider provider, params string[] roles)
        {
            RoleManager<IdentityRole> roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            //Create role if it does not exist
            foreach (string role in roles)
            {
                bool doesRoleExist = await roleManager.RoleExistsAsync(role);
                if (!doesRoleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task CreateDefualtInstructor(IServiceProvider serviceProvider)
        {
            const string email = "computerprogramming@cptc.edu";
            const string username = "instructor";
            const string password = "Programming";

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Check if any users are in database
            if(userManager.Users.Count() == 0)
            {
                IdentityUser instructor = new IdentityUser()
                {
                    Email = email,
                    UserName = username,
                };

                //Create instructor
                await userManager.CreateAsync(instructor, password);
                // Add to instructor role
                await userManager.AddToRoleAsync(instructor, Instructor);
            }
        }
    }
}
