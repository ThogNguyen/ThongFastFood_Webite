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

namespace ThongFastFood_Client.Controllers
{
	public class CartController : Controller
	{
		Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly INotyfService _notyf;
		public CartController(HttpClient httpClient, INotyfService noty)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_notyf = noty;
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
				await _httpClient.PostAsync(_httpClient.BaseAddress + "/CartApi/AddItem?userId=" + userId + "&productId=" + productId + "&quantity=" + quantity, content);

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
				await _httpClient.PutAsync(_httpClient.BaseAddress + "/CartApi/UpdateItem?userId=" + userId + "&cartId=" + cartId + "&quantity=" + quantity, content);

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
		public async Task<IActionResult> CheckOut(OrderVM model)
		{
			// Lấy userId của người dùng hiện tại từ claims
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

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
				+ "/OrderApi/CreateOrder?userId=" + userId, content);

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Thanh toán thành công!");
			}
			else
			{
				string errorMessage = await apiMessage.Content.ReadAsStringAsync();
				_notyf.Error("Có lỗi xảy ra: " + errorMessage);
			}

			return RedirectToAction("Index","MainPage");
		}
	}
}
