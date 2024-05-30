using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.ViewComponents
{
	public class RelatedProductsViewComponent : ViewComponent
	{
		Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		public RelatedProductsViewComponent(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
		}

		public async Task<IViewComponentResult> InvokeAsync(int categoryId, int currentProductId)
		{
			List<ProductVM> relatedProducts = new List<ProductVM>();
			HttpResponseMessage apiMessage =
			   await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetProductsByCategory/" + categoryId);

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				relatedProducts = JsonConvert.DeserializeObject<List<ProductVM>>(data);

				// Loại bỏ sản phẩm hiện tại khỏi danh sách các sản phẩm liên quan
				relatedProducts = relatedProducts.Where(p => p.ProductId != currentProductId).ToList();
			}
			return View(relatedProducts);
		}
	}
}
