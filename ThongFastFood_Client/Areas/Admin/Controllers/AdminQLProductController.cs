using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Drawing.Printing;
using System.IO;
using System.Net.Http;
using System.Text;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminQLProductController : Controller
    {
        Uri baseUrl = new Uri("https://localhost:7244/api");

        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly INotyfService _notyf;
        public AdminQLProductController(HttpClient httpClient, IWebHostEnvironment environment, INotyfService noty)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
            _environment = environment;
            _notyf = noty;
        }

        // load sản phẩm
        public async Task<IActionResult> Product(int? page, string search, string sort)
        {
            List<ProductVM> products = new List<ProductVM>();

            HttpResponseMessage apiMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress +
                "/ProductApi/GetProducts?sort=" + sort + "&search=" + search);

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductVM>>(data);
            }

            int pageSize = 6;
            int pageNumber = page ?? 1;
            IPagedList<ProductVM> pagedProducts = products.ToPagedList(pageNumber, pageSize);   

            ViewData["CurrentSort"] = sort;
            ViewData["NameSort"] = sort == "name_asc" ? "name_desc" : "name_asc";
            ViewData["PriceSort"] = sort == "price_asc" ? "price_desc" : "price_asc";
            ViewData["CurrentSearch"] = search;

            return View(pagedProducts);
        }


        //thêm sản phẩm (view)
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            List<Category> categories = new List<Category>();
            HttpResponseMessage apiMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryApi/GetCategories");

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<Category>>(data);
            }

            ViewBag.Category_Id = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        //thêm sản phẩm (create)
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductVM model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // Lưu tệp tin ảnh vào thư mục img/product trong wwwroot
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = _environment.WebRootPath + "/img/product/" + fileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn tệp tin ảnh vào ProductModel
                    model.ProductImage = fileName;
                }

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = 
                    await _httpClient.PostAsync(_httpClient.BaseAddress + "/ProductApi/PostProduct", content);

                if (response.IsSuccessStatusCode)
                {
                    _notyf.Information("Thêm sản phẩm thành công");
                    // Chuyển hướng đến trang sản phẩm
                    return RedirectToAction("Product");
                }
            }

			List<Category> categories = new List<Category>();
			HttpResponseMessage apiMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryApi/GetCategories");

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				categories = JsonConvert.DeserializeObject<List<Category>>(data);
			}
            ViewBag.Category_Id = new SelectList(categories, "CategoryId", "CategoryName");

			// Nếu không thành công hoặc có lỗi xảy ra, hiển thị lại form với model để người dùng nhập lại thông tin
			return View(model);
        }

        //sửa sản phẩm (view)
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            #region Lấy danh sách loại sản phẩm

            // Khởi tạo danh sách loại sản phẩm
            List<Category> categories = new List<Category>();

            // Gọi API để lấy danh sách loại sản phẩm
            HttpResponseMessage cateMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryApi/GetCategories");
            if (cateMessage.IsSuccessStatusCode)
            {
                string cateData = await cateMessage.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<Category>>(cateData);
            }
            #endregion

            // Gọi API để lấy thông tin sản phẩm theo id
            HttpResponseMessage apiMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetIdProduct/" + id);
            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                ProductVM model = JsonConvert.DeserializeObject<ProductVM>(data);

                // Truyền danh sách loại sản phẩm vào ViewBag
                ViewBag.Category_Id = new SelectList(categories, "CategoryId", "CategoryName");

                // Truyền đường dẫn ảnh cũ vào ViewData
                ViewData["ProductImage"] = model.ProductImage;
                ViewData["ProductId"] = model.ProductId;
                ViewData["AddedDate"] = model.AddDate;

                return View(model);
            }

            return NotFound();
        }

        //sửa sản phẩm (update)
        [HttpPost]
        public async Task<IActionResult> EditProduct(int id, ProductVM model, IFormFile? file, string OldProductImage)
        {
            if (!ModelState.IsValid)
            {
                // Kiểm tra nếu trường ảnh không được điền vào
                if (string.IsNullOrEmpty(OldProductImage))
                {
                    // Nếu không, đặt TempData để thông báo lỗi
                    TempData["ImageNull"] = "Lỗi không tìm thấy ảnh.";
                    return RedirectToAction(nameof(EditProduct));
                }

                return RedirectToAction(nameof(EditProduct));
            }

            if (file != null)
            {
                // Lưu tệp tin ảnh mới vào thư mục img/product trong wwwroot
                string fileName = Path.GetFileName(file.FileName);
                string newFilePath = Path.Combine(_environment.WebRootPath, "img", "product", fileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn tệp tin ảnh mới vào ProductModel
                model.ProductImage = fileName;
            }
            else
            {
                // Giữ nguyên tên ảnh cũ
                model.ProductImage = OldProductImage;
            }

            // Chuyển đổi dữ liệu model sang chuỗi JSON
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            // Gửi yêu cầu PUT đến API để cập nhật sản phẩm
            HttpResponseMessage resMessage = 
                await _httpClient.PutAsync(_httpClient.BaseAddress + "/ProductApi/PutProduct/" + id, content);

            if (resMessage.IsSuccessStatusCode)
            {
                _notyf.Information("Sửa sản phẩm thành công");
                // Nếu thành công, chuyển hướng về trang danh sách sản phẩm
                return RedirectToAction("Product");
            }

            // Nếu có lỗi xảy ra hoặc dữ liệu không hợp lệ, hiển thị lại form sửa sản phẩm với dữ liệu hiện tại
            return View(model);
        }
    }
}
