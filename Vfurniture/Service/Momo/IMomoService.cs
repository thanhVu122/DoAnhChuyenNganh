using Vfurniture.Models.Momo;
using Vfurniture.Models;

namespace Vfurniture.Service.Momo
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> MomoCreatePayment(OrderInfoModel model);
        MomoExecuteResponseModel PayExecuteAsync(IQueryCollection collection);
    }
}
