using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Skp_ProjektDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            //
            //Needs DbContext For Claims


            //var services = host.Services.CreateScope();
            //var userManager = services.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //// Adds the user claims
            //var adminClaim = new Claim("Role", "Admin");
            //var adminUser = new IdentityUser() { UserName = "Admin", Email = "admin@zbc.dk" };

            ////Adds the user and claim to user manager
            //userManager.AddClaimAsync(adminUser, adminClaim);

            host.Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
