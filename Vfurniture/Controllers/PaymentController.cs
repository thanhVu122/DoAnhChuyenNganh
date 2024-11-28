using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vfurniture.Models;
using Vfurniture.Reponsitory;
using Vfurniture.Service.Momo;

namespace Vfurniture.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IMomoService _momoService;
        
        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;
          
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayWithMomo(OrderInfoModel model)
        {
            Console.WriteLine($"Amount: {model.Amount}");
            Console.WriteLine($"OrderId: {model.OrderId}");
            Console.WriteLine($"OrderInfo: {model.OrderInfomation}");
            Console.WriteLine($"FullName: {model.FullName}");
            var response = await _momoService.MomoCreatePayment(model);
            return Redirect(response.PayUrl);
        }
        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }
    }
}
