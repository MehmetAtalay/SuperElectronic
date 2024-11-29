using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SuperElectronic.Data;
using SuperElectronic.Models;

namespace SuperElectronic.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SuperElectronicDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(SuperElectronicDbContext dbContext,
            IWebHostEnvironment webHostEnvironment)
        {
            this._dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;

        }
        //Urunleri Listeleyen action
        public async Task<IActionResult> Index(string? ara)
        {
            // Order By Descending Yaptimki En son Eklenen Urun en Ustte gozuksun
            var products = await _dbContext.Products.OrderByDescending(p => p.Id).ToListAsync();
            var products2 = await _dbContext.Products.ToListAsync();
            
            //Null deilse viewdataya atcaz nullsa bos bir string eklicez
            if (!String.IsNullOrEmpty(ara))
            {
                products2 = products2.Where(x => x.Name.ToLower().Contains(ara)).ToList();
                return View(products2);
            }
            ViewData["Ara"] = ara ?? "";
            return View(products);
           

        }
        //Urun olusturmak icin olan Action Adi Buton Adi Yeni urune bastigimizda bizi forma yonlendiren
        //Butonun actionu
        //Simdilik asynchrous yapmama gerek yok but methodu

        public async Task<IActionResult> Create()
        {

            return View();
        }
        // Bir post methodu oldugu icin HTTPPOST ekledim

        [HttpPost]
        //FromForm ile verinin form verisi kullanilarak post ediliceginide ayrica belirttim.
        //Bu method sonra gerekirse async yapilacak
        public async Task<IActionResult> Create([FromForm] ProductDTO productDTO)
        {
            // Bu validationu model icinde yapmadim , cunku dosyanin image olup olmadina client side validation atamak
            // cok daha zor , Image Validationlari cok kolay deil belki ilerde imagin sadece bos veya dolu deil
            // gercekten image olduguna dairde bir validasyon atarim.
            if (productDTO.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Bir resim yukleyiniz lutfen");
            }
            if (!ModelState.IsValid)
            {
                return View(productDTO);
            }
            //Burda resmin adi essiz olsun diye ilk bir guid attik
            string resminAdi = Guid.NewGuid().ToString();
            //Resmin adina birde dosyanin uzantisini ekledik
            resminAdi += Path.GetExtension(productDTO.ImageFile!.FileName);
            //WebroothPath defaultta wwwRoot onu deistirmiycem
            string imageFullPath = _webHostEnvironment.WebRootPath + "/UrunResimleri/" + resminAdi;
            //Create ile  ImageFullPathda bir dosya olusturdum
            // Using ile kullandim cunku bu isimiz bittiginde bunlari dispose etmem gerekiyo
            //Direk Dispose methodunuda cagirabilirdim ama bu methodun daha iyi oldugu sogleniyo
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                await productDTO.ImageFile.CopyToAsync(stream);
            }

            Product product = new Product()
            {
                Name = productDTO.Name,
                Brand = productDTO.Brand,
                Category = productDTO.Category,
                CreatedAt = DateTime.Now,
                Description = productDTO.Description,
                ImageFileName = resminAdi,
                Price = productDTO.Price,
            };

            //veri tabanindaki tabloya attik.
            await _dbContext.Products.AddAsync(product);
            //veri tabanimiza kaydettik deisikleri.
            await _dbContext.SaveChangesAsync();

            //Form basarili bir sekilde post edilirse ProductsController in Index viewine redirect ettim.
            return RedirectToAction("Index", "Products");
        }

        public async Task<IActionResult> Duzenle(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            var productDTO = new ProductDTO()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
            };
            //ProductDtoda bu bilgiler olmadigi icin Viewdata ile hallettim.
            // Burdan viewe verileri bu sekilde aktardim.
            //ViewData server side
            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
            return View(productDTO);

        }
        [HttpPost]
        public async Task<IActionResult> Duzenle([FromForm] ProductDTO productDTO, int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            //Extra modelde olmayan verilerde oldugu icin icine ekledim.
            if (!ModelState.IsValid)
            {
                //Yine ayni forma doncemiz icin verileri silmeden gosterebilmem icin 
                //tekrar viewdata ile cekiyorum productan.
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
                return View(productDTO);
            }

            string yeniResminAdi = product.ImageFileName;
            //Burda imageyi kontrol etmeme gerek yok tekrar cunku zaten opsiyonel yaptim duzenleme yaparken
            //Ama diyelimki null deil o zaman mecburen yeni resim ekleyip eski resmide silmem lazim
            if (productDTO.ImageFile != null)
            {
                //Yeni re
                yeniResminAdi = Guid.NewGuid().ToString();
                yeniResminAdi += Path.GetExtension(productDTO.ImageFile.FileName);
                string imageFullPath = _webHostEnvironment.WebRootPath + "/UrunResimleri/" + yeniResminAdi;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    await productDTO.ImageFile.CopyToAsync(stream);
                }
                //Eski resmide silelim
                string eskiResim = _webHostEnvironment.WebRootPath + "/UrunResimleri/" + product.ImageFileName;
                System.IO.File.Delete(eskiResim);
            }

            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            product.Brand = productDTO.Brand;
            product.Category = productDTO.Category;
            product.ImageFileName = yeniResminAdi;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Products");


        }
        public async Task<IActionResult> SilmeSayfasi(int id)
        {
            var product = await _dbContext.FindAsync<Product>(id);
            return View(product);
        }
        public async Task<IActionResult> Sil(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = _webHostEnvironment.WebRootPath + "/UrunResimleri/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);
            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Products");

        }
        public async Task<IActionResult> Detay(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null) 
            {
                RedirectToAction("Index", "Home");
            }
            ProductDTO productDTO = new ProductDTO()
            {
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Category = product.Category,
                Description = product.Description,

            };
            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
            return View(productDTO);
        }
    }
}
