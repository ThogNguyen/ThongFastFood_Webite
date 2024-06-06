using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Api.Repositories.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly FoodStoreDbContext db;

		public OrderService(FoodStoreDbContext context)
		{
			db = context;
		}

		public async Task<ResponseMessage> CreateOrderAsync(string userId, OrderVM orderVM)
		{
			var userExists = await db.Users.AnyAsync(u => u.Id == userId);
			if (!userExists)
			{
				return new ResponseMessage
				{
					Message = "Người dùng không tồn tại",
					IsSuccess = false,
				};
			}

			// Tìm giỏ hàng của người dùng
			var userCartItems = await db.Carts
				.Where(c => c.User_Id == userId)
				.ToListAsync();

			if (userCartItems == null || !userCartItems.Any())
			{
				return new ResponseMessage
				{
					Message = "Giỏ hàng trống",
					IsSuccess = false,
				};
			}

			var order = new Order
			{
				OrderTime = DateTime.Now,
				CustomerName = orderVM.CustomerName,
				DeliveryAddress = orderVM.DeliveryAddress,
				PhoneNo = orderVM.PhoneNo,
				Status = OrderStatus.Pending,
				Note = orderVM.Note,
				Customer_Id = userId,
				TotalAmount = userCartItems.Sum(c => c.Quantity * c.Price),
				OrderDetails = userCartItems.Select(c => new OrderDetail
				{
					Product_Id = c.Product_Id,
					Quantity = c.Quantity,
					SubTotal = c.Quantity * c.Price
				}).ToList()
			};

			db.Orders.Add(order);

			db.Carts.RemoveRange(userCartItems);

			await db.SaveChangesAsync();

			return new ResponseMessage
			{
				Message = "Đặt hàng thành công",
				IsSuccess = true
			};
		}

		public async Task<List<OrderView>> GetOrdersByUserIdAsync(string userId)
		{
			var orders = await db.Orders
		.Include(o => o.OrderDetails)
			.ThenInclude(od => od.Product)  // Include product details
		.Where(o => o.Customer_Id == userId)
		.ToListAsync();

			if (orders == null || !orders.Any())
			{
				return new List<OrderView>();
			}

			var fullOrders = orders.Select(o => new OrderView
			{
				OrderId = o.OrderId,
				OrderTime = o.OrderTime,
				CustomerName = o.CustomerName,
				DeliveryAddress = o.DeliveryAddress,
				PhoneNo = o.PhoneNo,
				Status = o.Status,
				TotalAmount = o.TotalAmount,
				Note = o.Note,
				OrderDetails = o.OrderDetails.Select(od => new OrderDetailView
				{
					OrderDetailId = od.OrderDetailId,
					Quantity = od.Quantity,
					SubTotal = od.SubTotal,
					Product_Id = od.Product_Id,
					ProductName = od.Product.ProductName,
					ProductPrice = od.Product.ProductPrice,
					ProductImage = od.Product.ProductImage,
				}).ToList()
			}).ToList();

			return fullOrders;
		}

		public async Task<OrderView> GetOrderByIdAsync(int orderId)
		{
			var order = await db.Orders
				.Include(o => o.OrderDetails)
					.ThenInclude(od => od.Product)
				.FirstOrDefaultAsync(o => o.OrderId == orderId);

			if (order == null)
			{
				return null;
			}

			var orderView = new OrderView
			{
				OrderId = order.OrderId,
				OrderTime = order.OrderTime,
				CustomerName = order.CustomerName,
				DeliveryAddress = order.DeliveryAddress,
				PhoneNo = order.PhoneNo,
				Status = order.Status,
				TotalAmount = order.TotalAmount,
				Note = order.Note,
				OrderDetails = order.OrderDetails.Select(od => new OrderDetailView
				{
					OrderDetailId = od.OrderDetailId,
					Quantity = od.Quantity,
					SubTotal = od.SubTotal,
					Product_Id = od.Product_Id,
					ProductName = od.Product.ProductName,
					ProductPrice = od.Product.ProductPrice,
					ProductImage = od.Product.ProductImage,
				}).ToList()
			};

			return orderView;
		}

		public async Task<ResponseMessage> DeleteOrderAsync(int orderId)
		{
			var order = await db.Orders.FindAsync(orderId);
			if (order == null)
			{
				return new ResponseMessage 
				{ 
					Message = "Đơn hàng không tồn tại.", 
					IsSuccess = false 
				};
			}

			db.Orders.Remove(order);
			await db.SaveChangesAsync();
			return new ResponseMessage 
			{ 
				Message = "Xóa đơn hàng thành công.", 
				IsSuccess = true 
			};
		}
	}
}
