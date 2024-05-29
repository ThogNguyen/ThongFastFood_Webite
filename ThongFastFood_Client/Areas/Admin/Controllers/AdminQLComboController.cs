using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminQLComboController : Controller
    {
        Uri baseUrl = new Uri("https://localhost:7244/api");

        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        public AdminQLComboController(HttpClient httpClient, IWebHostEnvironment environment)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
            _environment = environment;
        }

        public async Task<IActionResult> Combo()
        {
            List<ComboVM> comboList = new List<ComboVM>();

            HttpResponseMessage apiMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/ComboApi/GetCombos");

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                comboList = JsonConvert.DeserializeObject<List<ComboVM>>(data);
            }

            return View(comboList);
        }

        [HttpGet]
        public IActionResult CreateCombo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCombo(ComboVM model, IFormFile cbfile)
        {
            if (ModelState.IsValid)
            {
                if (cbfile != null)
                {
                    // Lưu tệp tin ảnh vào thư mục img/product trong wwwroot
                    string fileName = Path.GetFileName(cbfile.FileName);
                    string filePath = _environment.WebRootPath + "/img/product/combo/" + fileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cbfile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn tệp tin ảnh vào ProductModel
                    model.ComboImage = fileName;
                }

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response =
                    await _httpClient.PostAsync(_httpClient.BaseAddress + "/ComboApi/PostCombo", content);

                if (response.IsSuccessStatusCode)
                {
                    // Chuyển hướng đến trang sản phẩm
                    return RedirectToAction("Combo");
                }
            }

            // Nếu không thành công hoặc có lỗi xảy ra, hiển thị lại form với model để người dùng nhập lại thông tin
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCombo(int id)
        {
            // Gọi API để lấy thông tin sản phẩm theo id
            HttpResponseMessage apiMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/ComboApi/GetCombo/" + id);
            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                ComboVM model = JsonConvert.DeserializeObject<ComboVM>(data);

                // Truyền đường dẫn ảnh cũ vào ViewData
                ViewData["ComboImage"] = model.ComboImage;
                @ViewData["ComboId"] = model.ComboId;

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditCombo(int id, ComboVM model, IFormFile cbfile)
        {
            if (!ModelState.IsValid)
            {
                // Kiểm tra nếu trường ảnh không được điền vào
                if (cbfile == null)
                {
                    // Nếu không, đặt TempData để thông báo lỗi
                    TempData["ImageCombo"] = "Vui lòng chọn ảnh sản phẩm.";
                    return RedirectToAction("EditCombo", new { id = id });
                }

                // Gọi API để lấy thông tin sản phẩm theo id
                HttpResponseMessage reapiMessage =
                    await _httpClient.GetAsync(_httpClient.BaseAddress + "/ComboApi/GetCombo/" + id);
                if (reapiMessage.IsSuccessStatusCode)
                {
                    string redata = await reapiMessage.Content.ReadAsStringAsync();
                    ComboVM remodel = JsonConvert.DeserializeObject<ComboVM>(redata);

                    // Truyền đường dẫn ảnh cũ vào ViewData
                    ViewData["ComboImage"] = remodel.ComboImage;
                    @ViewData["ComboId"] = remodel.ComboId;

                    return View(remodel);
                }

                return NotFound();
            }


            string newFileName = cbfile.FileName;

            if (!string.IsNullOrEmpty(newFileName))
            {
                // Xác định đường dẫn đến ảnh cũ
                string oldImagePath = Path.Combine(_environment.WebRootPath + "/img/product/combo/" + newFileName);

                // Kiểm tra xem ảnh cũ có tồn tại không
                if (System.IO.File.Exists(oldImagePath))
                {
                    // Nếu tồn tại, thực hiện xóa ảnh cũ
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Lưu tệp tin ảnh mới vào thư mục img/product trong wwwroot
            string fileName = Path.GetFileName(cbfile.FileName);
            string newFilePath = Path.Combine(_environment.WebRootPath + "/img/product/combo/" + fileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await cbfile.CopyToAsync(stream);
            }

            // Lưu đường dẫn tệp tin ảnh mới vào ProductModel
            model.ComboImage = fileName;

            // Chuyển đổi dữ liệu model sang chuỗi JSON
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            // Gửi yêu cầu PUT đến API để cập nhật sản phẩm
            HttpResponseMessage apiMessage = await _httpClient.PutAsync(_httpClient.BaseAddress + "/ComboApi/PutCombo/" + id, content);

            if (apiMessage.IsSuccessStatusCode)
            {
                // Nếu thành công, chuyển hướng về trang danh sách combo
                return RedirectToAction("Combo");
            }

            // Nếu có lỗi xảy ra hoặc dữ liệu không hợp lệ, hiển thị lại form sửa sản phẩm với dữ liệu hiện tại
            return View(model);
        }

        public async Task<IActionResult> DeleteCombo(int id)
        {
            HttpResponseMessage apiMessage =
                await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/ComboApi/RemoveCombo/" + id);

            if (apiMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Combo");
            }
            return View();
        }
    }
}
