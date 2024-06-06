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


		[HttpPost]
		public async Task<IActionResult> CreateOrder(string userId, OrderVM orderVM)
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

		[HttpDelete]
		public async Task<IActionResult> DeleteOrder(int orderId)
		{
			var response = await _orderSer.DeleteOrderAsync(orderId);

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
