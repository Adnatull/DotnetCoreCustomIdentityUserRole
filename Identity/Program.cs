using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
           

            var host = CreateHostBuilder(args).Build();
            try
            {
                var scope = host.Services.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<DataContext>();
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                ctx.Database.EnsureCreated();

                var superAdmin = new AppRole {
                    Name = "SuperAdmin",
                    Description = @"somebody with access to the site network administration features and all other features. See the Create a Network article"
                };
                var administrator = new AppRole {
                    Name = "Administrator",
                    Description = @"somebody who has access to all the administration features within a single site."
                };
                var editor = new AppRole {
                    Name = "Editor",
                    Description = "somebody who can publish and manage posts including the posts of other users."
                };
                var author = new AppRole {
                    Name = "Author",
                    Description = "somebody who can publish and manage their own posts."
                };                
                var contributor = new AppRole {
                    Name = "Contributor",
                    Description = "somebody who can write and manage their own posts but cannot publish them."
                };                
                var subscriber = new AppRole {
                    Name = "Subscriber",
                    Description = "somebody who can only manage their profile"
                };
                



                if (!ctx.Roles.Any())
                {
                    roleMgr.CreateAsync(superAdmin).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(administrator).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(editor).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(author).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(contributor).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(subscriber).GetAwaiter().GetResult();
                    
                    // Create A role
                }



                if (!ctx.Users.Any(u => u.UserName == "Adnatull"))
                {
                    // Create a admin
                    var adminUser = new AppUser
                    {
                        UserName = "Adnatull",
                        Email = "adnatull@dimikit.net",
                        FirstName = "Adnatull Al",
                        LastName = "Masum"
                    };
                    var result = userMgr.CreateAsync(adminUser, "adnan100").GetAwaiter().GetResult();

                    // Add a role
                    userMgr.AddToRoleAsync(adminUser, superAdmin.Name).GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, administrator.Name).GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, editor.Name).GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, author.Name).GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, contributor.Name).GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, subscriber.Name).GetAwaiter().GetResult();
                    

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
