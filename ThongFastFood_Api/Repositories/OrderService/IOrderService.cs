using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Api.Repositories.OrderService
{
	public interface IOrderService
	{
		Task<ResponseMessage> CreateOrderAsync(string userId, OrderVM orderVM);
		Task<List<OrderView>> GetOrdersByUserIdAsync(string userId);
		Task<OrderView> GetOrderByIdAsync(int orderId);
		Task<ResponseMessage> CancelOrderAsync(int orderId);
        Task<List<OrderView>> GetAllOrdersAsync();
        Task<ResponseMessage> UpdateOrderStatusAsync(int orderId, string newStatus);

    }
}
