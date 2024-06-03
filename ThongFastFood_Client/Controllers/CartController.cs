using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		private readonly IWebHostEnvironment _environment;
		private readonly INotyfService _notyf;
		public CartController(HttpClient httpClient, IWebHostEnvironment environment, INotyfService noty)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_environment = environment;
			_notyf = noty;
		}


		public async Task<IActionResult> CartInfo()
		{
			List<CartVM> carts = new List<CartVM>();

			// Lấy userId của người dùng hiện tại
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

		
		public async Task<IActionResult> AddToCart(int productId, int quantity)
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

			var requestBody = new AddtoCartRequest
			{
				UserId = userId,
				ProductId = productId,
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

		public IActionResult CheckOut()
		{
			return View();
		}
	}
}
