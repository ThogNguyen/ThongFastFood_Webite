using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ThongFastFood_Api.Models;
using ThongFastFood_Client.Services;

namespace ThongFastFood_Client.Controllers
{
    public class ReportController : Controller
    {
        Uri baseUrl = new Uri("https://localhost:7244/api");

        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;
        private readonly IPDFService _pdfService;

        public ReportController(IPDFService pdfService, HttpClient httpClient, IWebHostEnvironment environment)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseUrl;
            _environment = environment;
            _pdfService = pdfService;
        }

        public IActionResult Report()
        {
            return View();
        }

        public async Task<IActionResult> ExportPdfOrder(int orderId)
        {
            OrderView order = new OrderView();
            HttpResponseMessage apiMessage =
               await _httpClient.GetAsync(_httpClient.BaseAddress + "/OrderApi/GetOrderById?orderId=" + orderId);

            if (apiMessage.IsSuccessStatusCode)
            {
                string data = await apiMessage.Content.ReadAsStringAsync();
                order = JsonConvert.DeserializeObject<OrderView>(data);
            }
            else
            {
                return NotFound("Đơn hàng không tồn tại.");
            }

            // Render view với dữ liệu đơn hàng
            string html = await this.RenderViewAsync("Report/OrderTemplate", order, true);

            // Tạo PDF từ HTML
            var result = _pdfService.GeneratePdf(html);

            return File(result, "application/pdf", $"{DateTime.UtcNow.Ticks}.pdf");
        }

    }
}
