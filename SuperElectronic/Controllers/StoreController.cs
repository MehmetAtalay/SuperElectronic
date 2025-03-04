using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperElectronic.Data;
using SuperElectronic.Models;

namespace SuperElectronic.Controllers
{
    public class StoreController : Controller
    {

        private readonly SuperElectronicDbContext _context;
       
        public StoreController(SuperElectronicDbContext context)
        {
           
            _context = context;
          
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
