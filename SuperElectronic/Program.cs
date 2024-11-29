using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SuperElectronic.Data;
using SuperElectronic.Models;

namespace SuperElectronic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Burda dependency Injection Ile Dbyi Projeye Ekledim.
            builder.Services.AddDbContext<SuperElectronicDbContext>(options =>
            {
                var connectionString =
                builder.Configuration.GetConnectionString("SuperElectronicConnectionString");
                options.UseSqlServer(connectionString);
            });


            var app = builder.Build();

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

            app.Run();
        }
    }
}
