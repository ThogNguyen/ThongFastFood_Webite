using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Client.Controllers
{
	public class MainPageController : Controller
	{
		Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly IWebHostEnvironment _environment;
		private readonly INotyfService _notyf;
		public MainPageController(HttpClient httpClient, IWebHostEnvironment environment, INotyfService noty)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_environment = environment;
			_notyf = noty;
		}

		public async Task<IActionResult> Index()
		{
			List<ProductVM> products = new List<ProductVM>();
			HttpResponseMessage apiMessage =
			   await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetProducts");

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				products = JsonConvert.DeserializeObject<List<ProductVM>>(data);
			}
			return View(products);
		}
		public IActionResult About()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> UserInfo()
		{
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			UserVM user = new UserVM();

			HttpResponseMessage apiMessage =
			   await _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountApi/GetUserById?id=" + userId);

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				user = JsonConvert.DeserializeObject<UserVM>(data);
			}
			else
			{
				_notyf.Error("Không load được user này!.");
				return RedirectToAction("Index");
			}

			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> UserInfo(UserVM model)
		{
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			string data = JsonConvert.SerializeObject(model);
			StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

			HttpResponseMessage apiMessage =
				await _httpClient.PutAsync(_httpClient.BaseAddress + "/AccountApi/PutUser?id=" + userId, content);

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Cập nhật thông tin thành công");
				return RedirectToAction(nameof(UserInfo));
			}

			return View(model);
		}

		// danh sách đơn hàng
		public async Task<IActionResult> CustomerOrder()
		{
			// Lấy userId của người dùng hiện tại
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			List<OrderView> orders = new List<OrderView>();

			HttpResponseMessage apiMessage = 
				await _httpClient.GetAsync(_httpClient.BaseAddress + "/OrderApi/GetOrdersByUserId?userId=" + userId);

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				orders = JsonConvert.DeserializeObject<List<OrderView>>(data);
			}

			var customerOrderView = new CustomerOrderView
			{
				Orders = orders
			};

			return View(customerOrderView);
		}

		// hủy đơn hàng
		public async Task<IActionResult> CancelCustomerOrder(int orderId)
		{
			var content = new StringContent(string.Empty);

			HttpResponseMessage apiMessage = await 
				_httpClient.PutAsync(_httpClient.BaseAddress + "/OrderApi/CancelOrder/?orderId="
				+ orderId, content);

			if (apiMessage.IsSuccessStatusCode)
			{
				_notyf.Success("Đơn hàng đã được hủy");
				return RedirectToAction("CustomerOrder");
			}
			else
			{
				if (apiMessage.StatusCode == HttpStatusCode.NotFound)
				{
					_notyf.Error("Đơn hàng không tồn tại.");
				}
				else if (apiMessage.StatusCode == HttpStatusCode.BadRequest)
				{
					string errorMessage = await apiMessage.Content.ReadAsStringAsync();
					_notyf.Error(errorMessage);
				}
				else
				{
					_notyf.Error("Có lỗi xảy ra khi gửi yêu cầu.");
				}

				// You might want to handle other potential errors here as needed
				return RedirectToAction("CustomerOrder");
			}
		}
	}
}
