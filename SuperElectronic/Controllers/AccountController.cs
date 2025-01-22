using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperElectronic.Models;

namespace SuperElectronic.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser>userManager,
            SignInManager<ApplicationUser>signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
               return View(registerDto);
            }
            var user = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                CreatedAt = DateTime.UtcNow,
            };
            
            var result = await _userManager.CreateAsync(user,registerDto.Password);
            if (result.Succeeded) 
            {
                //Successfull Kullanici kaydi
                await _userManager.AddToRoleAsync(user, "client");

                //Yeni kullaniciya giris yaptir
                //False diyerek browser kapandigi zaman butun cookileri sildik
                await _signInManager.SignInAsync(user,false);

                return RedirectToAction("Index","Home");

            }
            foreach(var error in result.Errors) 
            {
                ModelState.AddModelError("", error.Description);
            }

            
            return View(registerDto);
        }
    }
}
