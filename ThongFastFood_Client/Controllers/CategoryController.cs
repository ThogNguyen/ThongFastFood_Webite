using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PagedList;
using System.Security.Claims;
using System.Text;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Controllers
{
	public class CategoryController : Controller
	{
        Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly INotyfService _notyf;
		public CategoryController(HttpClient httpClient, INotyfService noty)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_notyf = noty;
		}
		public async Task<IActionResult> ListProduct(int? page, int? id, string search, string sort, string priceRange)
		{
			List<ProductVM> products = new List<ProductVM>();
			int totalProducts = 0;

			if (id.HasValue)
			{
				// Nếu id có giá trị, tìm sản phẩm theo mã loại 
				HttpResponseMessage apiMessage = 
					await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetProductsByCategory/" + id);
				if (apiMessage.IsSuccessStatusCode)
				{
					string data = await apiMessage.Content.ReadAsStringAsync();
					products = JsonConvert.DeserializeObject<List<ProductVM>>(data);
					totalProducts = products.Count;
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
					totalProducts = products.Count;
				}
			}

            int pageSize = 6;
            int pageNumber = page ?? 1;
            IPagedList<ProductVM> pagedProducts = products.ToPagedList(pageNumber, pageSize);

			ViewData["CurrentSort"] = sort;
			ViewData["CurrentPriceRange"] = priceRange;
			ViewData["CurrentCategoryId"] = id;
			ViewData["TotalProducts"] = totalProducts;

			return View(pagedProducts);
		}

		/*GetIdProduct*/
		public async Task<IActionResult> DetailProduct(int id)
		{
			DetailProductVM productDetail = new DetailProductVM(); 
			HttpResponseMessage apiMessageProduct =
			   await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetIdProduct/" + id); 
			// Sử dụng ProductApi để lấy thông tin sản phẩm

			if (apiMessageProduct.IsSuccessStatusCode)
			{
				string data = await apiMessageProduct.Content.ReadAsStringAsync();
				productDetail = JsonConvert.DeserializeObject<DetailProductVM>(data);
			}

			// Sau khi lấy thông tin sản phẩm,
			// tiếp tục lấy các đánh giá của sản phẩm từ CommentApi
			HttpResponseMessage apiMessageComments =
			   await _httpClient.GetAsync(_httpClient.BaseAddress + "/CommentApi/GetCommentsByProductID/" + id);

			if (apiMessageComments.IsSuccessStatusCode)
			{
				string commentData = await apiMessageComments.Content.ReadAsStringAsync();
				// Giả sử server trả về một mảng các CommentVM
				List<CommentVM> comments = JsonConvert.DeserializeObject<List<CommentVM>>(commentData);
				// Gán danh sách đánh giá vào thuộc tính Comments của productDetail
				productDetail.Comments = comments;
			}

			return View(productDetail);
		}

		[HttpPost]
		public async Task<IActionResult> WriteComment(int productId, CommentVM model)
		{
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userName = HttpContext.User.FindFirstValue("FullName");

			// Kiểm tra nếu userId là null
			if (userId == null)
			{
				_notyf.Error("Bạn cần đăng nhập để thực hiện.");
				// Chuyển hướng người dùng đến trang đăng nhập trong khu vực Identity
				return RedirectToAction("Login", "Account", new { area = "Identity" });
			}

			var requestBody = new CommentVM
			{
				CustomerName = userName,
				ReviewDate = DateTime.Now, // Ngày hiện tại
				Comment = model.Comment
			};

			string data = JsonConvert.SerializeObject(requestBody);
			Console.WriteLine(data);
			StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

			HttpResponseMessage apiMessage =
				await _httpClient.PostAsync(_httpClient.BaseAddress + "/CommentApi/PostComment?productId=" + productId + "&userId=" + userId, content);

			if (apiMessage.IsSuccessStatusCode)
			{
				// Thông báo thành công
				return RedirectToAction(nameof(DetailProduct), new { id = productId });
			}
			else
			{
                // Thông báo lỗi
                string errorMessage = await apiMessage.Content.ReadAsStringAsync();
                _notyf.Error(errorMessage);
                return View(model);
			}
		}
	}
}
