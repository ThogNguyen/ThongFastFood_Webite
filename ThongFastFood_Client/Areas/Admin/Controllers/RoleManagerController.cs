using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class RoleManagerController : Controller
    {
        Uri baseUrl = new Uri("https://localhost:7244/api");

        private readonly HttpClient _httpClient;
        private readonly INotyfService _notyf;
        public RoleManagerController(HttpClient httpClient, INotyfService noty)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
            _notyf = noty;
        }
        public async Task<IActionResult> RoleManager()
        {
            List<UserRolesVM> userRoles = new List<UserRolesVM>();

            HttpResponseMessage apiMessage =
                    await _httpClient.GetAsync(_httpClient.BaseAddress + "/RoleManagerApi/GetAllUserRoles");

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                userRoles = JsonConvert.DeserializeObject<List<UserRolesVM>>(data);
            }
            return View(userRoles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoleUser(string id)    
        {
            // Khởi tạo danh sách vai trò
            List<IdentityRole> roles = new List<IdentityRole>();

            // Gọi API để lấy danh sách loại sản phẩm
            HttpResponseMessage resMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/RoleApi/GetRoles");
            if (resMessage.IsSuccessStatusCode)
            {
                string roleData = await resMessage.Content.ReadAsStringAsync();
                roles = JsonConvert.DeserializeObject<List<IdentityRole>>(roleData);
            }

            // Gọi API để lấy thông tin role
            HttpResponseMessage apiMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/RoleManagerApi/GetRoleByUserId/" + id);
            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                UserRolesVM model = JsonConvert.DeserializeObject<UserRolesVM>(data);

                // Truyền danh sách loại sản phẩm vào ViewBag
                ViewBag.Role_Id = new SelectList(roles, "Id", "Name", model.RoleName);

                // Truyền id user vào ViewData
                ViewData["UserId"] = model.RoleId;

                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditRoleUser(string id, UserRolesVM model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            // Gửi yêu cầu PUT đến API để cập nhật vai trò của người dùng
            HttpResponseMessage resMessage =
                await _httpClient.PutAsync(_httpClient.BaseAddress + "/RoleManagerApi/PutUserRole?id=" + id, content);

            if (resMessage.IsSuccessStatusCode)
            {
                _notyf.Information("Sửa vai trò thành công");

                // Chuyển hướng về trang danh sách vai trò
                return RedirectToAction("RoleManager");
            }

            // Nếu có lỗi xảy ra hoặc dữ liệu không hợp lệ, hiển thị lại form sửa sản phẩm với dữ liệu hiện tại
            return View(model);
        }
    }
}
