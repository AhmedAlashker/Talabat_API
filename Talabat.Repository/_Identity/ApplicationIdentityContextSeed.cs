﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastructure._Identity
{
    public static class ApplicationIdentityContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any()) 
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Ahmed Medhat",
                    Email = "ahmed.medhat@gmail.com",
                    UserName = "ahmed.medhat",
                    PhoneNumber = "01289157142",
                }; 

                await userManager.CreateAsync(user, "P@ssw0rd");

            }
        }
    }
}
