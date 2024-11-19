using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    //[Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {

        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DataContext dataContext, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Roles.OrderByDescending(r => r.Id).ToListAsync());
        }
        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]

        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole identityRole)
        {
            if (!_roleManager.RoleExistsAsync(identityRole.Name).GetAwaiter().GetResult())
            {

                _roleManager.CreateAsync(new IdentityRole(identityRole.Name)).GetAwaiter().GetResult();
            }
            return Redirect("Index");
        }
    }
}
