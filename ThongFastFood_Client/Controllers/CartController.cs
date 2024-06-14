using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;
using ThongFastFood_Client.Models;
using ThongFastFood_Client.Services;

namespace ThongFastFood_Client.Controllers
{
	public class CartController : Controller
	{
		Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly INotyfService _notyf;
		private readonly IVNPayService _vnPayService;

		public CartController(HttpClient httpClient, INotyfService noty, IVNPayService vnPayService)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_notyf = noty;
			_vnPayService = vnPayService;
		}

		public async Task<IActionResult> CartInfo()
		{
			List<CartVM> carts = new List<CartVM>();

			// Lấy userId của người dùng hiện tại
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Kiểm tra nếu userId là null
			if (userId == null)
			{
				_notyf.Error("Bạn cần đăng nhập thực hiện.");
				// Chuyển hướng người dùng đến trang đăng nhập trong khu vực Identity
				return RedirectToAction("Login", "Account", new { area = "Identity" });
			}

			HttpResponseMessage apiMessage =
				await _httpClient.GetAsync(_httpClient.BaseAddress + "/CartApi/GetCartItemsByUser?userId=" + userId);

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				carts = JsonConvert.DeserializeObject<List<CartVM>>(data);
			}

			return View(carts);
		}
		
		public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
		{
			// Lấy userId của người dùng hiện tại
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Kiểm tra nếu userId là null
			if (userId == null)
			{
				_notyf.Error("Bạn cần đăng nhập để thực hiện.");
				// Chuyển hướng người dùng đến trang đăng nhập trong khu vực Identity
				return RedirectToAction("Login", "Account", new { area = "Identity" });
			}

			var requestBody = new CartVM
			{
				User_Id = userId,
				Product_Id = productId,
				Quantity = quantity
			};

			string data = JsonConvert.SerializeObject(requestBody);
			Console.WriteLine(data);
			StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

			HttpResponseMessage apiMessage =
				await _httpClient.PostAsync(_httpClient.BaseAddress + "/CartApi/PostItem?userId=" + userId + "&productId=" + productId + "&quantity=" + quantity, content);

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Sản phẩm đã được thêm vào giỏ hàng");
			}
			else
			{
				string errorMessage = await apiMessage.Content.ReadAsStringAsync();
				_notyf.Error("Có lỗi xảy ra: " + errorMessage);
			}

			return RedirectToAction("CartInfo");
		}

		public async Task<IActionResult> UpdateCart(int cartId, int quantity)
		{
			// Lấy userId của người dùng hiện tại
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Kiểm tra nếu userId là null
			if (userId == null)
			{
				_notyf.Error("Bạn cần đăng nhập để thực hiện.");
				// Chuyển hướng người dùng đến trang đăng nhập trong khu vực Identity
				return RedirectToAction("Login", "Account", new { area = "Identity" });
			}

			var requestBody = new CartVM
			{
				User_Id = userId,
				CartId = cartId,
				Quantity = quantity
			};

			string data = JsonConvert.SerializeObject(requestBody);
			Console.WriteLine(data);
			StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

			HttpResponseMessage apiMessage =
				await _httpClient.PutAsync(_httpClient.BaseAddress + "/CartApi/PutItem?userId=" + userId + "&cartId=" + cartId + "&quantity=" + quantity, content);

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Sản phẩm đã được cập nhật");
			}
			else
			{
				string errorMessage = await apiMessage.Content.ReadAsStringAsync();
				_notyf.Error("Có lỗi xảy ra: " + errorMessage);
			}

			return RedirectToAction("CartInfo");
		}

		public async Task<IActionResult> RemoveItem(int cartId)
		{
			// Lấy userId của người dùng hiện tại
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			HttpResponseMessage apiMessage =
				await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/CartApi/RemoveItem?userId=" + userId + "&cartId=" + cartId);

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Sản phẩm đã được xóa");
			}
			else
			{
				string errorMessage = await apiMessage.Content.ReadAsStringAsync();
				_notyf.Error("Có lỗi xảy ra: " + errorMessage);
			}

			return RedirectToAction("CartInfo");
		}

		public async Task<IActionResult> ClearItem()
		{
			// Lấy userId của người dùng hiện tại
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			HttpResponseMessage apiMessage =
				await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/CartApi/ClearCart?userId=" + userId);

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Danh sách sản phẩm đã được xóa.");
			}
			else
			{
				string errorMessage = await apiMessage.Content.ReadAsStringAsync();
				_notyf.Error("Có lỗi xảy ra: " + errorMessage);
			}
			return RedirectToAction("CartInfo");
		}

		[HttpGet]
		public async Task<IActionResult> CheckOut()
		{
			List<CartVM> carts = new List<CartVM>();

			// Lấy userId của người dùng hiện tại
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			HttpResponseMessage apiMessage =
				await _httpClient.GetAsync(_httpClient.BaseAddress 
				+ "/CartApi/GetCartItemsByUser?userId=" + userId);

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				carts = JsonConvert.DeserializeObject<List<CartVM>>(data);
			}

			return View(carts);
		}

		[HttpPost]
		public async Task<IActionResult> CheckOut(OrderVM model, string payment)
		{
			// Lấy userId của người dùng hiện tại từ claims
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			#region Lấy tổng tiền sản phẩm trong giỏ hàng
			HttpResponseMessage cartApiMessage =
				await _httpClient.GetAsync(_httpClient.BaseAddress
				+ "/CartApi/GetCartItemsByUser?userId=" + userId);

			List<CartVM> carts = new List<CartVM>();
			if (cartApiMessage.IsSuccessStatusCode)
			{
				string redata = await cartApiMessage.Content.ReadAsStringAsync();
				carts = JsonConvert.DeserializeObject<List<CartVM>>(redata);
			}

			// Calculate the total amount
			double totalAmount = CartVM.CalculateTotalAmount(carts);
			#endregion

			if (payment == "VNPAY")
			{
				var vnPayModel = new VnPaymentRequestModel
				{
					Amount = totalAmount,
					CreatedDate = DateTime.Now,
					Description = $"{model.CustomerName} {model.PhoneNo}",
					FullName = model.CustomerName, 
					DeliveryAddress = model.DeliveryAddress,
					PhoneNo = model.PhoneNo,
					Note = model.Note,
					OrderId = new Random().Next(1000, 10000),
				};
				return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
			}

			var requestBody = new OrderVM
			{
				CustomerName = model.CustomerName,
				DeliveryAddress = model.DeliveryAddress,
				PhoneNo = model.PhoneNo,
				Note = model.Note
			};

			string data = JsonConvert.SerializeObject(requestBody);
			Console.WriteLine(data);
			StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

			HttpResponseMessage apiMessage =
				await _httpClient.PostAsync(_httpClient.BaseAddress 
				+ "/OrderApi/PostOrder?userId=" + userId + "&payment=" + payment, content);

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Thanh toán thành công!");
			}
			else
			{
				_notyf.Error("Thông tin không được để trống.");
				return RedirectToAction(nameof(CheckOut));
			}

			return RedirectToAction("CustomerOrder", "MainPage");
		}

		[Authorize]
		public IActionResult PaymentCallBack()
		{
			var response = _vnPayService.PaymentExecute(Request.Query);

			// nếu kết quả không bằng 00 => thanh toán thành công
			if (response == null || response.VnPayResponseCode != "00")
			{
				if(response.VnPayResponseCode == "24")
				{
					_notyf.Success("Hủy thanh toán thành công");
					return RedirectToAction("Index", "MainPage");
				}
				else
				{
					_notyf.Error($"Lỗi thanh toán với VNPay: {response.VnPayResponseCode}");
					return RedirectToAction("Index", "MainPage");
				}
			}

			// Lấy userId của người dùng hiện tại từ claims
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			string payment = TypeOfPayment.VNPAY;

			// Tạo đối tượng OrderVM để lưu trữ thông tin đơn hàng
			var order = new OrderVM
			{
				CustomerName = response.FullName,
				DeliveryAddress = response.DeliveryAddress,
				PhoneNo = response.PhoneNo,
				Note = response.Note
			};

			string data = JsonConvert.SerializeObject(order);
			Console.WriteLine(data);
			StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

			HttpResponseMessage apiMessage =
				_httpClient.PostAsync(_httpClient.BaseAddress
				+ "/OrderApi/PostOrder?userId=" + userId + "&payment=" + payment, content).Result;

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Thanh toán thành công!");
			}
            return RedirectToAction("CustomerOrder", "MainPage");
        }
	}
}
