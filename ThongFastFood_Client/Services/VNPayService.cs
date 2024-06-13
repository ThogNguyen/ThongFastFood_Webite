using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using ThongFastFood_Api.Models;
using ThongFastFood_Client.Helpers;
using ThongFastFood_Client.Models;

namespace ThongFastFood_Client.Services
{
	public class VNPayService : IVNPayService
	{
		private readonly IConfiguration _config;

		public VNPayService(IConfiguration config)
		{
			_config = config;
		}

		public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
		{
			var tick = DateTime.Now.ToString("yyyyMMddHHmmss");
			var vnpay = new VnPayLibrary();

			// Thêm các thông tin cần thiết vào yêu cầu thanh toán
			vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
			vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
			vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
			vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());

			vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
			vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
			vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
			vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

			var orderInfo = $"{model.OrderId}|{model.FullName}|{model.DeliveryAddress}|{model.PhoneNo}|{model.Note}";
			vnpay.AddRequestData("vnp_OrderInfo", orderInfo);

			vnpay.AddRequestData("vnp_OrderType", "other"); // Giá trị mặc định: other
			vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);

			vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu giao dịch tại hệ thống của merchant

			// Tạo URL thanh toán chứa thông tin yêu cầu và chữ ký bảo mật
			var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

			return paymentUrl;
		}

		public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
		{
			var vnpay = new VnPayLibrary();

			// Đọc dữ liệu phản hồi từ VNPay
			foreach (var (key, value) in collections)
			{
				if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
				{
					vnpay.AddResponseData(key, value.ToString());
				}
			}

			// Lấy các thông tin cần thiết từ phản hồi
			var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
			var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
			var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
			var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
			var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

			var orderInfoParts = vnp_OrderInfo.Split('|');
			var orderId = orderInfoParts[0];
			var fullName = orderInfoParts[1];
			var deliveryAddress = orderInfoParts[2];
			var phoneNo = orderInfoParts[3];
			var note = orderInfoParts[4];

			// Xác thực chữ ký bảo mật
			bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
			if (!checkSignature)
			{
				return new VnPaymentResponseModel
				{
					Success = false
				};
			}

			return new VnPaymentResponseModel
			{
				Success = true,
				PaymentMethod = TypeOfPayment.VNPAY,
				OrderDescription = vnp_OrderInfo,
				OrderId = vnp_orderId.ToString(),
				TransactionId = vnp_TransactionId.ToString(),
				Token = vnp_SecureHash,
				VnPayResponseCode = vnp_ResponseCode,
				FullName = fullName,
				DeliveryAddress = deliveryAddress,
				PhoneNo = phoneNo,
				Note = note
			};
		}
	}
}
