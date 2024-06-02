using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PagedList;
using System;
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
        public async Task<IActionResult> Product(int? page)
        {
            List<ProductVM> products = new List<ProductVM>();

            HttpResponseMessage apiMessage = 
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetProducts");

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductVM>>(data);
            }

            int pageSize = 6;
            int pageNumber = page ?? 1;
            IPagedList<ProductVM> pagedProducts = products.ToPagedList(pageNumber, pageSize);

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

        //thêm sản phẩm (thêm)
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
        public async Task<IActionResult> EditProduct(int id, ProductVM model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                // Kiểm tra nếu trường ảnh không được điền vào
                if (file == null)
                {
                    // Nếu không, đặt TempData để thông báo lỗi
                    TempData["ImageNull"] = "Vui lòng chọn ảnh sản phẩm.";
                    return RedirectToAction("EditProduct", new { id = id });
                }
                // Gọi API để lấy thông tin sản phẩm theo ID
                HttpResponseMessage apiMessage =
                    await _httpClient.GetAsync(_httpClient.BaseAddress + "/ProductApi/GetIdProduct/" + id);
                if (apiMessage.IsSuccessStatusCode)
                {
                    string redata = await apiMessage.Content.ReadAsStringAsync();
                    ProductVM res = JsonConvert.DeserializeObject<ProductVM>(redata);

                    // Gọi API để lấy danh sách loại sản phẩm
                    List<Category> categories = new List<Category>();
                    HttpResponseMessage cateMessage =
                        await _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryApi/GetCategories");
                    if (cateMessage.IsSuccessStatusCode)
                    {
                        string cateData = await cateMessage.Content.ReadAsStringAsync();
                        categories = JsonConvert.DeserializeObject<List<Category>>(cateData);
                    }

                    // Truyền danh sách loại sản phẩm và thông tin sản phẩm vào ViewBag hoặc ViewData
                    ViewBag.Category_Id = new SelectList(categories, "CategoryId", "CategoryName");
                    ViewData["ProductImage"] = res.ProductImage;
                    ViewData["ProductId"] = res.ProductId;
                    ViewData["AddedDate"] = res.AddDate;

                    // Trả về view với model đã nhận và dữ liệu từ API để hiển thị lại form sửa sản phẩm
                    return View(model);
                }
                else
                {
                    return NotFound();
                }
            }

            // Lấy tên ảnh cũ từ ViewData và đảm bảo không bị null
            string newFileName = file.FileName;

            if (!string.IsNullOrEmpty(newFileName))
            {
                // Xác định đường dẫn đến ảnh cũ
                string oldImagePath = Path.Combine(_environment.WebRootPath, "img", "product", newFileName);

                // Kiểm tra xem ảnh cũ có tồn tại không
                if (System.IO.File.Exists(oldImagePath))
                {
                    // Nếu tồn tại, thực hiện xóa ảnh cũ
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Lưu tệp tin ảnh mới vào thư mục img/product trong wwwroot
            string fileName = Path.GetFileName(file.FileName);
            string newFilePath = Path.Combine(_environment.WebRootPath, "img", "product", fileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Lưu đường dẫn tệp tin ảnh mới vào ProductModel
            model.ProductImage = fileName;

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


        //xoá sản phẩm (delete)
        public async Task<IActionResult> DeleteProduct(int id)
        {
            HttpResponseMessage apiMessage =
                await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/ProductApi/RemoveProduct/" + id);

            if (apiMessage.IsSuccessStatusCode)
            {
                _notyf.Success("Xóa sản phẩm thành công");
                return RedirectToAction("Product");
            }
            return View();
        }
    }
}
