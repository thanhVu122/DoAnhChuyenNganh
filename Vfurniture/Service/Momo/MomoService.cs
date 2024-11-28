using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Vfurniture.Models.Momo;
using System.Security.Cryptography;
using System.Text;
using Vfurniture.Models;
using Vfurniture.Models.Momo;
using Vfurniture.Service;
namespace Vfurniture.Service.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;
        public MomoService(IOptions<MomoOptionModel> options) { _options = options; }
        public async Task<MomoCreatePaymentResponseModel> MomoCreatePayment(OrderInfoModel model)
        {
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfomation = "Khách hàng: " + model.FullName + ".Nội dung: " + model.OrderInfomation;
            var rawData =
                    $"partnerCode={_options.Value.PartnerCode}&" +
                    $"accessKey={_options.Value.AccessKey}&" +
                    $"requestId={model.OrderId}&" +
                    $"amount={model.Amount}&" +
                    $"orderId={model.OrderId}&" +
                    $"orderInfo={model.OrderInfomation}&" +
                    $"returnUrl={_options.Value.ReturnUrl}&" +
                    $"notifyUrl={_options.Value.NotifyUrl}&" +
                    $"extraData=";
            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);
            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };

            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Tạo một đối tượng biểu diễn dữ liệu request
            var requestData = new
            {
                accessKey = _options.Value.AccessKey,         // Khóa truy cập từ cấu hình
                partnerCode = _options.Value.PartnerCode,    // Mã đối tác từ cấu hình
                requestType = _options.Value.RequestType,    // Loại yêu cầu từ cấu hình
                notifyUrl = _options.Value.NotifyUrl,        // URL thông báo khi hoàn thành giao dịch
                returnUrl = _options.Value.ReturnUrl,        // URL trả về sau khi giao dịch
                orderId = model.OrderId,                     // Mã đơn hàng
                amount = model.Amount.ToString(),            // Số tiền (chuyển thành chuỗi)
                orderInfo = model.OrderInfomation,          // Thông tin đơn hàng
                requestId = model.OrderId,                   // ID của yêu cầu (thường trùng với orderId)
                extraData = "",                              // Dữ liệu bổ sung (nếu có, để trống nếu không cần)
                signature = signature                        // Chữ ký xác thực (đã tạo trước đó)
            };
            // Thêm dữ liệu vào body của request
            request.AddParameter(
                "application/json",                         // Loại nội dung (Content-Type)
                JsonConvert.SerializeObject(requestData),  // Chuyển đối tượng requestData thành chuỗi JSON
                ParameterType.RequestBody                  // Loại tham số là body của request
            );

            // Gửi yêu cầu bất đồng bộ và nhận phản hồi
            var response = await client.ExecuteAsync(request);

            // Chuyển đổi phản hồi JSON thành đối tượng C#
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);

        }
        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;
            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo
            };
        }
        private string ComputeHmacSha256(string message, string secretKey)
        {
            // Chuyển đổi secretKey và message thành mảng byte
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            // Tạo mảng byte để lưu kết quả hash
            byte[] hashBytes;

            // Sử dụng HMACSHA256 với keyBytes
            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            // Chuyển byte[] thành chuỗi hex
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hashString;
        }
    }
}
