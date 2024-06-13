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

		public async Task<ResponseMessage> CreateOrderAsync(string userId, string payment, OrderVM orderVM)
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

			// kiểm tra hình thức thanh toán
			if (!TypeOfPayment.ListPayments.Contains(payment))
			{
				return new ResponseMessage
				{
					Message = "Hình thức thanh toán không tồn tại",
					IsSuccess = false,
				};
			}

			var order = new Order
			{
				OrderTime = DateTime.Now,
				CustomerName = orderVM.CustomerName,
				DeliveryAddress = orderVM.DeliveryAddress,
				PhoneNo = orderVM.PhoneNo,
				PaymentType = payment,
				Status = OrderStatus.Pending,
				Note = orderVM.Note ?? null,
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
                .OrderByDescending(o => o.OrderId)
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
				PaymentType = o.PaymentType,
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
				PaymentType = order.PaymentType,
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

		public async Task<ResponseMessage> CancelOrderAsync(int orderId)
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

			#region Đơn hàng đã hủy thì không được huỷ đơn nữa
			if (order.Status == OrderStatus.Cancelled)
			{
				return new ResponseMessage
				{
					Message = "Đơn hàng đã bị hủy trước đó.",
					IsSuccess = false
				};
			}
			#endregion
			#region Không hủy đơn khi đơn khác trạng thái đang xử lí
			if (order.Status != OrderStatus.Pending)
			{
				return new ResponseMessage
				{
					Message = "Không thể hủy đơn hàng sau khi đã xác nhận",
					IsSuccess = false
				};
			}
			#endregion
			#region Kiểm tra xem đơn hàng có hình thức thanh toán là "Đã thanh toán với VNPay" chưa
			if (order.PaymentType == TypeOfPayment.VNPAY)
			{
				return new ResponseMessage
				{
					Message = "Không thể hủy đơn hàng khi đã được thanh toán",
					IsSuccess = false
				};
			}
			#endregion

			order.Status = OrderStatus.Cancelled;
			await db.SaveChangesAsync();

			return new ResponseMessage
			{
				Message = "Đơn hàng đã được hủy.",
				IsSuccess = true
			};
		}

        public async Task<List<OrderView>> GetAllOrdersAsync()
        {
            var allOrders = db.Orders.ToList();
            List<OrderView> orderViews = new List<OrderView>();

            foreach (var order in allOrders)
            {
                var orderView = new OrderView
                {
                    OrderId = order.OrderId,
                    OrderTime = order.OrderTime,
                    CustomerName = order.CustomerName,
                    DeliveryAddress = order.DeliveryAddress,
                    PhoneNo = order.PhoneNo,
					PaymentType = order.PaymentType,
					Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    Note = order.Note
                };

                orderViews.Add(orderView);
            }

            return orderViews;
        }

        public async Task<ResponseMessage> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await db.Orders.FindAsync(orderId);

            if (order == null)
            {
                return new ResponseMessage { 
					IsSuccess = false, 
					Message = "Không tìm thấy đơn hàng." 
				};
            }

            if (!OrderStatus.AdminStatuses.Contains(newStatus))
            {
                return new ResponseMessage { 
					IsSuccess = false, 
					Message = "Trạng thái mới không hợp lệ." 
				};
            }

            #region Kiểm tra xem đơn hàng với trạng thái đã hủy không
            var cancelledOrder = await db.Orders.FirstOrDefaultAsync
                    (o => o.OrderId == orderId && o.Status == OrderStatus.Cancelled);

            if (cancelledOrder != null)
            {
                return new ResponseMessage
                {
                    IsSuccess = false,
                    Message = "Không thể cập nhật đơn hàng đã hủy."
                };
            }
			#endregion

			order.Status = newStatus;
            db.Orders.Update(order);
            await db.SaveChangesAsync();

            return new ResponseMessage { 
				IsSuccess = true, 
				Message = "Cập nhật trạng thái đơn hàng thành công." 
			};
        }

    }
}
