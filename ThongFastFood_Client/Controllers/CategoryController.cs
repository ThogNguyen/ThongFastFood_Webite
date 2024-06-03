using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PagedList;
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
		public async Task<IActionResult> ListProduct(int? page, int? id, string search, string sort, string priceRange)
		{
			List<ProductVM> products = new List<ProductVM>();

			if (id.HasValue)
			{
				// Nếu id có giá trị, tìm sản phẩm theo mã loại 
				HttpResponseMessage apiMessage = 
					await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetProductsByCategory/" + id.Value);
				if (apiMessage.IsSuccessStatusCode)
				{
					string data = await apiMessage.Content.ReadAsStringAsync();
					products = JsonConvert.DeserializeObject<List<ProductVM>>(data);
				}
			}
			else
			{
				// Nếu id không có giá trị, lấy tất cả sản phẩm
				HttpResponseMessage apiMessage = 
					await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetProducts?search=" 
					+ search + "&sort=" + sort + "&priceRange=" + priceRange);
				if (apiMessage.IsSuccessStatusCode)
				{
					string data = await apiMessage.Content.ReadAsStringAsync();
					products = JsonConvert.DeserializeObject<List<ProductVM>>(data);
				}
			}

            int pageSize = 6;
            int pageNumber = page ?? 1;
            IPagedList<ProductVM> pagedProducts = products.ToPagedList(pageNumber, pageSize);

			ViewData["CurrentSort"] = sort;
			ViewData["CurrentPriceRange"] = priceRange;
            ViewBag.CurrentCategory = id;

            return View(pagedProducts);
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
