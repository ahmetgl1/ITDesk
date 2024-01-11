using ITDeskServer.Context;
using ITDeskServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITDeskServer.Middleware;

public static class ExtensionsMiddleware
{

    public static void AutoMigration( WebApplication app)
    {

        using (var scoped = app.Services.CreateScope())
        {
            var context = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

        }
    }


    public static async Task CreateFirstUserAsync(this WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    Email = "admin@admin",
                    UserName = "Ahmetgl1",
                    LastName = "Güzeller",
                    FirstName = "Ahmet Fatih"
                };

                await userManager.CreateAsync(user, "Password*12");
            }
        }
    }



     public static void CreateRoles(this WebApplication app)
     {
         using (var scoped = app.Services.CreateScope())
         {
             var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    
             if (!roleManager.Roles.Any())
             {
    
                 roleManager.CreateAsync(new AppRole()
                 {
    
                     Id = Guid.NewGuid(),
                     Name = "Admin"
    
                 }).Wait();
             }
    
    
         }
     }



       public static void CreateUserRole(this WebApplication app)
       {
    
           using(var scoped = app.Services.CreateScope())
           {
    
             var context =  scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
             var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>(); 
             var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    
    
               AppUser? user = userManager.Users.Where(o => o.Email == "admin@admin").FirstOrDefault();
    
               if(user is not null)
               {
                   
    
                   AppRole? role = roleManager.Roles.Where(o => o.Name == "ADMİN").FirstOrDefault();
                   if(role is not null)
                   {
    
                       bool existRole = context.AppUserRoles.Any(o => o.RoleId ==  role.Id &&  o.UserId == user.Id);
    
                       if (!existRole)
                       {
    
                           AppUserRole userRole = new()
                           {
                               Id =  Guid.NewGuid(),
                               RoleId = role.Id,
                               UserId = user.Id
    
                           };
                       context.Add(userRole);
                       context.SaveChanges();
                       }
    
    
                   }
               }
    
           }



    }




}
