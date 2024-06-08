using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR.Protocol;
using Newtonsoft.Json;
using PagedList;
using System.Text;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminQLOrderController : Controller
    {
        Uri baseUrl = new Uri("https://localhost:7244/api");

        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly INotyfService _notyf;
        public AdminQLOrderController(HttpClient httpClient, IWebHostEnvironment environment, INotyfService noty)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
            _environment = environment;
            _notyf = noty;
        }

        // load sản phẩm
        public async Task<IActionResult> Order(int? page)
        {
            List<OrderView> orders = new List<OrderView>();

            HttpResponseMessage apiMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/OrderApi/GetAllOrders");

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<List<OrderView>>(data);
            }

            int pageSize = 6;
            int pageNumber = page ?? 1;
            IPagedList<OrderView> pagedProducts = orders.ToPagedList(pageNumber, pageSize);

            return View(pagedProducts);
        }

        [HttpGet]
		public async Task<IActionResult> EditOrder(int orderId)
        {
            OrderVM orderVM = new OrderVM();

            HttpResponseMessage apiMessage =
                await _httpClient.GetAsync(_httpClient.BaseAddress + "/OrderApi/GetOrderById?orderId=" + orderId);

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                OrderView order = JsonConvert.DeserializeObject<OrderView>(data);

                orderVM.Status = order.Status; // Gán trạng thái vào orderVM

                ViewData["OrderId"] = order.OrderId;

                // chuyển list trạng thái dành cho Admin thanh selectlist items
                var adminStatuses = OrderStatus.AdminStatuses
                    .Select(status => new SelectListItem { Text = status, Value = status });

                ViewBag.AdminStatuses = adminStatuses;
            }
            else
            {
                _notyf.Error("Không tìm thấy đơn hàng.");
                return RedirectToAction("Order");
            }

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder(int orderId, string newStatus)
        {
            // Tạo đối tượng chứa dữ liệu cần gửi lên API
            var data = new { OrderId = orderId, NewStatus = newStatus };

            // Chuyển đổi dữ liệu model sang chuỗi JSON
            string jsonData = JsonConvert.SerializeObject(data);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để cập nhật trạng thái đơn hàng
            HttpResponseMessage resMessage =
                await _httpClient.PutAsync(_httpClient.BaseAddress +
				"/OrderApi/PutOrderStatus?orderId=" + orderId + "&newStatus=" + newStatus, content);

            if (resMessage.IsSuccessStatusCode)
            {
                _notyf.Success("Cập nhật trạng thái đơn hàng thành công");
                // Nếu thành công, chuyển hướng về trang danh sách đơn hàng
                return RedirectToAction("Order");
            }
            else
            {
                string errorMessage = await resMessage.Content.ReadAsStringAsync();
                _notyf.Error("Có lỗi xảy ra: " + errorMessage);
                return RedirectToAction("Order");
            }
        }
    }
}
