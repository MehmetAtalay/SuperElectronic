using Microsoft.AspNetCore.Identity;
using SuperElectronic.Models;

namespace SuperElectronic.Data
{
    public class DatabaseInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManager,
            RoleManager<IdentityRole>? roleManager)
        {
            if (userManager == null || roleManager == null)
            {
                Console.WriteLine("userManager or roleManager is null => exit");
                return;
            }
            //admin varmi yokmu bakalim
            var exists = await roleManager.RoleExistsAsync("admin");
            if (!exists)
            {
                Console.WriteLine("admin yok, yeni bir admin olusturulcak");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            //seller yoksa yeni bir seller olusturuyor
            exists = await roleManager.RoleExistsAsync("seller");
            if (!exists)
            {
                Console.WriteLine("Satici yok yeni bir satici olusturulacak");
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }
            //client yoksa yeni bir client
            exists = await roleManager.RoleExistsAsync("client");
            if (!exists)
            {
                Console.WriteLine("Client yok yeni bir satici olusturulacak");
                await roleManager.CreateAsync(new IdentityRole("client"));
            }
            var adminUsers = await userManager.GetUsersInRoleAsync("admin");
            if (adminUsers.Any())
            {
                //Admin var
                Console.WriteLine("admin var =>exit");
                return;
            }
          
            var user = new ApplicationUser()
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin@admin.com", // Kullanici adi kullaniciyi dogrulamakta kullanilicak
                Email = "admin@admin.com",
                CreatedAt = DateTime.Now

            };
            string initialPassword = "admin123";

            var result = await userManager.CreateAsync(user, initialPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "admin");
                Console.WriteLine("Admin olusturuldu , lutfen sifreyi deistiriniz");
                Console.WriteLine("email:" + user.Email);
                Console.WriteLine("Initial password :" + initialPassword);
            }
        }
    }
}

