using Microsoft.AspNetCore.Mvc;
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

        // GET: /LienHe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /LienHe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LienHe lienHe)
        {
            if (ModelState.IsValid)
            {
                // Save the contact form submission to the database
                _dataContext.LienHes.Add(lienHe);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cảm ơn bạn đã liên hệ với chúng tôi!";
                return RedirectToAction("Create");
            }
            return View(lienHe);
        }
    }
}
