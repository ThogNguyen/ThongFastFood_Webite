using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
    {
		Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly INotyfService _notyf;
		public RoleController(HttpClient httpClient, INotyfService noty)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_notyf = noty;
		}
		public async Task<IActionResult> Role()
        {
			List<IdentityRole> roles = new List<IdentityRole>();

			HttpResponseMessage apiMessage =
					await _httpClient.GetAsync(_httpClient.BaseAddress + "/RoleApi/GetRoles");

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = apiMessage.Content.ReadAsStringAsync().Result;
				roles = JsonConvert.DeserializeObject<List<IdentityRole>>(data);
			}
			return View(roles);
		}

		[HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }
		[HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole role)
        {
            string data = JsonConvert.SerializeObject(role);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage resMessage =
                await _httpClient.PostAsync(_httpClient.BaseAddress + "/RoleApi/AddRole", content);

            if (resMessage.IsSuccessStatusCode)
            {
                _notyf.Information("Thêm vai trò thành công");
                return RedirectToAction("Role");
            }

            return View(role);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            HttpResponseMessage apiMessage = 
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/RoleApi/GetRoleById/" + id);
            if (apiMessage.IsSuccessStatusCode)
            {
                string data = apiMessage.Content.ReadAsStringAsync().Result;
                IdentityRole role = JsonConvert.DeserializeObject<IdentityRole>(data);

                return View(role);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string id, IdentityRole role)
        {
            string data = JsonConvert.SerializeObject(role);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage apiMessage =
                await _httpClient.PutAsync(_httpClient.BaseAddress + "/RoleApi/PutRole/" + id, content);

            if (apiMessage.IsSuccessStatusCode)
            {
                _notyf.Information("Sửa vai trò thành công");
                return RedirectToAction("Role");
            }

            return View(role);
        }

        
        public async Task<IActionResult> DeleteRole(string id)
        {
            HttpResponseMessage apiMessage = 
                await _httpClient.DeleteAsync(_httpClient.BaseAddress + "/RoleApi/RemoveRole/" + id);

            if (apiMessage.IsSuccessStatusCode)
            {
                _notyf.Success("Xóa vai trò thành công");
                return RedirectToAction("Role");
            }
            return View();
        }
    }
}
