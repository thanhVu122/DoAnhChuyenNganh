using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class LienHeController : Controller
    {
        private readonly DataContext _dataContext;

        public LienHeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public ActionResult Index()
        {
            var lienhe = _dataContext.LienHes
                .ToList();
            return View(lienhe);

        }
    }
}
