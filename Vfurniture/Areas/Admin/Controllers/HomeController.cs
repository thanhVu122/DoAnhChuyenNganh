using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vfurniture.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
