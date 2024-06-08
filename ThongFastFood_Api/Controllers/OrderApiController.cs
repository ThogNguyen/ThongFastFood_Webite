using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Repositories.OrderService;

namespace ThongFastFood_Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class OrderApiController : ControllerBase
	{
		private readonly IOrderService _orderSer;

		public OrderApiController(IOrderService orderSer)
		{
			_orderSer = orderSer;
		}

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderSer.GetAllOrdersAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound(new { Message = "Không có đơn hàng nào được tìm thấy." });
            }

            return Ok(orders);
        }

        [HttpPost]
		public async Task<IActionResult> PostOrder(string userId, OrderVM orderVM)
		{
			var response = await _orderSer.CreateOrderAsync(userId, orderVM);

			// nếu đặt thành công
			if (response.IsSuccess)
			{
				// Trả về tb thành công
				return Ok(response);
			}
			else
			{
				// Trả về thông báo lỗi
				return BadRequest(response.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetOrdersByUserId(string userId)
		{
			var orders = await _orderSer.GetOrdersByUserIdAsync(userId);

			if (orders == null || !orders.Any())
			{
				return NotFound(new { Message = "Khách hàng này chưa đặt món lần nào." });
			}

			return Ok(orders);
		}

		[HttpGet]
		public async Task<IActionResult> GetOrderById(int orderId)
		{
			var order = await _orderSer.GetOrderByIdAsync(orderId);

			if (order == null)
			{
				return NotFound(new { Message = "Đơn hàng không tồn tại." });
			}

			return Ok(order);
		}

		[HttpPut]
		public async Task<IActionResult> CancelOrder(int orderId)
		{
			var response = await _orderSer.CancelOrderAsync(orderId);

			if (response.IsSuccess)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response.Message);
			}
		}

        [HttpPut]
        public async Task<IActionResult> PutOrderStatus(int orderId, string newStatus)
        {
            var response = await _orderSer.UpdateOrderStatusAsync(orderId, newStatus);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
    }
}
