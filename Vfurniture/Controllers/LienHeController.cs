using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Controllers
{
    public class LienHeController : Controller
    {
        private readonly DataContext _dataContext;
        public LienHeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public async Task< IActionResult> ListLienHeIndex()
        {
            var lienhe = _dataContext.LienHes.ToList();
            return View(lienhe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LienHe contact)
        {
            if (ModelState.IsValid)
            {
                _dataContext.LienHes.Add(contact);
                _dataContext.SaveChanges();

                TempData["Success"] = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất.";
                return RedirectToAction("Index");
            }
            return View(contact);
        }
    }
}
