using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperElectronic.Data;
using SuperElectronic.Models;
using System.Diagnostics;

namespace SuperElectronic.Controllers
{
    public class HomeController : Controller
    {
      private readonly SuperElectronicDbContext _dbContext;
        private readonly int pageSize = 8;
        public HomeController(SuperElectronicDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<IActionResult> Index(string ara,int pageIndex)
        {
            IQueryable<Product> query = _dbContext.Products;
            if (ara != null)
            {
                query = query.Where(p => p.Name.ToLower().Contains(ara));
            }
            query = query.OrderByDescending(p => p.Id);
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            decimal count = query.Count();
            //ToplamSayfa bulmak icin countu page sizea bolduk math ceilingle ona en yakin
            //sayiyi atadik totalpagese
            int totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var products = await query.ToListAsync();
            ViewData["Ara"] = ara ?? "";
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalPages;
            
            return View(products);
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
