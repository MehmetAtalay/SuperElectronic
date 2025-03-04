using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SuperElectronic.Data;
using SuperElectronic.Models;
using SuperElectronic.Validators;

namespace SuperElectronic.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SuperElectronicDbContext _context;
       
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,SuperElectronicDbContext dbContext)
        { 
            _context = dbContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public async Task<IActionResult> Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index","Home");
            }

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
             
               
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email,
               loginDto.Password, loginDto.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                ViewBag.ErrorMessage = "Hatali Islem";
            }
            return View(loginDto);
        }
        [Authorize]
        public async Task<IActionResult>  Profile()
        {

            if (!_signInManager.IsSignedIn(User))
            {
                RedirectToAction("Index", "Home");
            }
            var userRole = "";
           
            if (User.IsInRole("admin")) { userRole = "Admin"; }
            if (User.IsInRole("seller")) { userRole = "Seller"; }
            if (User.IsInRole("client")) { userRole = "Client"; }
            var currentUser =  await _userManager.GetUserAsync(User);
            var user = new UserProfileDTO()
            {
                Id = currentUser!.Id,
                FirstName = currentUser!.FirstName,
                LastName = currentUser!.LastName,
                Email = currentUser!.Email!,
                PhoneNumber = currentUser!.PhoneNumber!  
            };
            ViewData["userRole"] = userRole;
            return View(user);
        }
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Index", "Home");
            }
            var userProfile = new UserProfileDTO()
            {
                FirstName = user!.FirstName,
                LastName = user!.LastName,
                Email = user!.Email!,
                PhoneNumber = user!.PhoneNumber!

            };
            return View(userProfile);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile([FromForm] UserProfileDTO userProfile)
        {
            if (_signInManager.IsSignedIn(User))
            {
                RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                RedirectToAction("Index", "Home");
            }
            var user = await _userManager.GetUserAsync(User);
            
            user!.FirstName = userProfile.FirstName;
            user!.LastName = userProfile.LastName;
            user.Email = userProfile.Email;
            user.PhoneNumber = userProfile.PhoneNumber;
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", "Account");
        }
       
       public IActionResult ChangePassword()
        {
            return View();
        }
     
        //public async Task<IActionResult> ChangePassword( PasswordDTO passwordDTO)
        //{
        //    var appUser = await _userManager.GetUserAsync(User);
        //    if (appUser != null)
        //    {
        //        RedirectToAction("Index", "Home");
        //    }
        //    if (!_signInManager.IsSignedIn(User))
        //    {
        //        RedirectToAction("Index", "Home");
        //    }

        //    //var name = appUser.FirstName;
        //    //var lastName = appUser.LastName;

        //    //ViewData["FirstName"] = name;
        //    //ViewData["LastName"] = lastName;
            
           
        //    var result = await _userManager.ChangePasswordAsync
        //    (appUser, passwordDTO.CurrentPassword, passwordDTO.NewPassword);
        //    if (result.Succeeded)
        //    {
        //        ViewBag.Success = "Şifre Basarıyla deiştirildi";
        //    }
        //    if (!result.Succeeded)
        //    {
        //        ViewBag.Error = "Şifre deiştirme başarısız " +result.Errors.First().Description;
        //    }
        //    return View();
        //}
        public IActionResult AccessDenied()
        {
            return View();
        }
       
    }
}
