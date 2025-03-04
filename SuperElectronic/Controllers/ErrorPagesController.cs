using Microsoft.AspNetCore.Mvc;

namespace SuperElectronic.Controllers
{
    public class ErrorPagesController : Controller
    {
        public IActionResult ErrorPage404()
        {
            return View();
        }
    }
}
