using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.ViewComponents
{
	public class ListCategoryViewComponent : ViewComponent
	{
		Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly IWebHostEnvironment _environment;
		public ListCategoryViewComponent(HttpClient httpClient, IWebHostEnvironment environment)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_environment = environment;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<CategoryVM> categories = new List<CategoryVM>();
			HttpResponseMessage apiMessage =
			   await _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryApi/GetCategories");

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				categories = JsonConvert.DeserializeObject<List<CategoryVM>>(data);
			}
			return View(categories);
		}
	}
}
