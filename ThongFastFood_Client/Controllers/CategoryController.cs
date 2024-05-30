using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Controllers
{
	public class CategoryController : Controller
	{
        Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly IWebHostEnvironment _environment;
		public CategoryController(HttpClient httpClient, IWebHostEnvironment environment)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_environment = environment;
		}
		public async Task<IActionResult> ListProduct()
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

		/*GetIdProduct*/
		public async Task<IActionResult> DetailProduct(int id)
		{
			ProductVM product = new ProductVM();
			HttpResponseMessage apiMessage =
			   await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetIdProduct/" + id);

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				product = JsonConvert.DeserializeObject<ProductVM>(data);
			}
			return View(product);
		}
	}
}
