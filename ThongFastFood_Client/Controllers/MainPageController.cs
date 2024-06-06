using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Controllers
{
	public class MainPageController : Controller
	{
		Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly IWebHostEnvironment _environment;
		public MainPageController(HttpClient httpClient, IWebHostEnvironment environment)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_environment = environment;
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
		public IActionResult UserInfo()
		{
			return View();
		}
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

		public async Task<IActionResult> GetOrder(int id)
		{
			HttpResponseMessage apiMessage =
				await _httpClient.GetAsync(_httpClient.BaseAddress + "/OrderApi/GetOrdersByUserId/" + id);

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				var order = JsonConvert.DeserializeObject<OrderView>(data);
				return View(order);
			}

			return NotFound();
		}

	}
}
