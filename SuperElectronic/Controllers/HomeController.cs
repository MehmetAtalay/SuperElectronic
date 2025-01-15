using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperElectronic.Data;
using SuperElectronic.Models;
using System.Diagnostics;

namespace SuperElectronic.Controllers
{
    public class HomeController : Controller
    {
      private readonly SuperElectronicDbContext _dbcontext;

        public HomeController(SuperElectronicDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
       
        public async Task<IActionResult> Index()
        {
            var product = await _dbcontext.Products.OrderByDescending(x=>x.Id).Take(4).ToListAsync();
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
