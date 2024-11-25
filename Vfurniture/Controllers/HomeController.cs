using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Controllers
{
	public class HomeController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, DataContext dataContext)
		{
			_dataContext = dataContext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			var sanpham=_dataContext.SanPhams.OrderByDescending(p=>p.MaSanPham).ToList();	
			return View(sanpham);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statuscode)

        {
            if (statuscode == 404)
			{
				return View("NotFound");
			}
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

    }
}
