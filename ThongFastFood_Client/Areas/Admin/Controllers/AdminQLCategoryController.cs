using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using ThongFastFood_Api.Data;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminQLCategoryController : Controller
    {
        Uri baseUrl = new Uri("https://localhost:7244/api");

        private readonly HttpClient _httpClient;
        public AdminQLCategoryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
        }

        public IActionResult Category()
        {
            List<Category> categories = new List<Category>();

            HttpResponseMessage apiMessage =
                    _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryApi/GetCategories").Result;

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = apiMessage.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<Category>>(data);
            }
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            string data = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage resMessage =
                _httpClient.PostAsync(_httpClient.BaseAddress + "/CategoryApi/PostCategory", content).Result;

            if (resMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Category");
            }

            return View(category);
        }


        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            HttpResponseMessage apiMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryApi/GetIdCategory/" + id).Result;
            if (apiMessage.IsSuccessStatusCode)
            {
                string data = apiMessage.Content.ReadAsStringAsync().Result;
                Category category = JsonConvert.DeserializeObject<Category>(data);

                return View(category);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult EditCategory(int id, Category category)
        {
            string data = JsonConvert.SerializeObject(category);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage apiMessage =
                _httpClient.PutAsync(_httpClient.BaseAddress + "/CategoryApi/PutCategory/" + id, content).Result;

            if (apiMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Category");
            }

            return View(category);
        }

        public IActionResult DeleteCategory(int id)
        {
            HttpResponseMessage apiMessage =
                _httpClient.DeleteAsync(_httpClient.BaseAddress + "/CategoryApi/RemoveCategory/" + id).Result;

            if (apiMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Category");
            }
            return View();
        }
    }
}
