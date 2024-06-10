using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PagedList;
using System.Security.Claims;
using System.Text;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminQLUserDataController : Controller
    {
		Uri baseUrl = new Uri("https://localhost:7244/api");

		private readonly HttpClient _httpClient;
		private readonly INotyfService _notyf;
		public AdminQLUserDataController(HttpClient httpClient, INotyfService noty)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = baseUrl;
			_notyf = noty;
		}
		public async Task<IActionResult> User(int? page)
        {
			List<UserVM> users = new List<UserVM>();

			HttpResponseMessage apiMessage =
				await _httpClient.GetAsync(_httpClient.BaseAddress + "/AccountApi/GetUsers");

			if (apiMessage.IsSuccessStatusCode)
			{
				string data = await apiMessage.Content.ReadAsStringAsync();
				users = JsonConvert.DeserializeObject<List<UserVM>>(data);
			}
			int pageSize = 6;
			int pageNumber = page ?? 1;
			IPagedList<UserVM> pagedUsers = users.ToPagedList(pageNumber, pageSize);
			return View(pagedUsers);
		}
		[HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            UserVM user = new UserVM();

            HttpResponseMessage apiMessage =
               await _httpClient.GetAsync(_httpClient.BaseAddress + 
               "/AccountApi/GetUserById?id=" + id);

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<UserVM>(data);
            }
            else
            {
                _notyf.Error("Không load được user này!.");
                return RedirectToAction("Index");
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(string id, UserVM model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage apiMessage =
                await _httpClient.PutAsync(_httpClient.BaseAddress + 
                "/AccountApi/PutUser?id=" + id, content);

            if (apiMessage.IsSuccessStatusCode)
            {
                _notyf.Success("Cập nhật thông tin thành công");
                return RedirectToAction(nameof(EditUser));
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            HttpResponseMessage apiMessage = 
                await _httpClient.DeleteAsync(_httpClient.BaseAddress +
                "/AccountApi/RemoveUser?id=" + id);

            if (apiMessage.IsSuccessStatusCode)
            {
                _notyf.Success("Xóa người dùng thành công");
            }
            return RedirectToAction("User");
        }
    }
}
