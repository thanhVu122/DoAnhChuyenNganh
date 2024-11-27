namespace Vfurniture.Models.Momo
{
    public class MomoCreatePaymentResponseModel
    {
        public string RequestId { get; set; }      // ID yêu cầu
        public int ErrorCode { get; set; }         // Mã lỗi (nếu có)
        public string OrderId { get; set; }        // ID đơn hàng
        public string Message { get; set; }        // Thông báo (từ MoMo trả về)
        public string LocalMessage { get; set; }   // Thông báo địa phương (thông tin chi tiết lỗi)
        public string RequestType { get; set; }    // Loại yêu cầu (ví dụ: "captureMoMoWallet")
        public string Signature { get; set; }      // Chữ ký để xác thực yêu cầu
        public string PayUrl { get; set; }         // URL thanh toán
        public string QrCodeUrl { get; set; }      // URL mã QR (nếu có)
        public string DeepLink { get; set; }       // Deep link cho thanh toán
        public string DeepLinkWebInApp { get; set; } // Deep link cho thanh toán trên web trong ứng dụng
    }
}
