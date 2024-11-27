using Microsoft.AspNetCore.Mvc;
using Vfurniture.Models;
using Vfurniture.Service.Momo;

namespace Vfurniture.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
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
            var response = _momoService.PayExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }
    }
}
