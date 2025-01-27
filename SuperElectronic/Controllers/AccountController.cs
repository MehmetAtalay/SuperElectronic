using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperElectronic.Models;

namespace SuperElectronic.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                await _signInManager.SignOutAsync();

            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
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

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                //Successfull Kullanici kaydi
                await _userManager.AddToRoleAsync(user, "client");

                //Yeni kullaniciya giris yaptir
                //False diyerek browser kapandigi zaman butun cookileri sildik
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");

            }
            //Butun Hatalari Div ile ustte gostermek icin ama su an gerek yok
            //foreach(var error in result.Errors) 
            //{
            //    ModelState.AddModelError("", error.Description);
            //}


            return View(registerDto);
        }
        public async Task<IActionResult> Login()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email,
               loginDto.Password, loginDto.RememberMe, false);
            return RedirectToAction("Index", "Home");
        }
    }
}
