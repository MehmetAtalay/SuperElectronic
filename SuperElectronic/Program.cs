using FluentValidation;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperElectronic.Data;
using SuperElectronic.Models;
using SuperElectronic.Validators;

namespace SuperElectronic
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            
            //Http Client Factory

            builder.Services.AddHttpClient();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Burda dependency Injection Ile Dbyi Projeye Ekledim.
            builder.Services.AddDbContext<SuperElectronicDbContext>(options =>
            {
                var connectionString =
                builder.Configuration.GetConnectionString("SuperElectronicConnectionString");
                options.UseSqlServer(connectionString);
            });
            //ApplicationUser bizim olusturdugumuz model.
            //Identity servislerini servise ekledik
           
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                //Alphanumeric karakter istemesin
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<SuperElectronicDbContext>();

            
    
            var app = builder.Build();
            app.UseStatusCodePages(async options => {
            
            if(options.HttpContext.Response.StatusCode == 404)
                {
                    options.HttpContext.Response.Redirect("/ErrorPages/ErrorPage404");
                }
            });
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            using (var scope = app.Services.CreateScope()) 
            {
                var userManager = scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>))
                    as UserManager<ApplicationUser>;
                var roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))
                    as RoleManager<IdentityRole>;
                await DatabaseInitializer.SeedDataAsync(userManager, roleManager);
            }

            app.Run();
        }
    }
}
