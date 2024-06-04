using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Api.Repositories.CartService
{
	public class CartService : ICartService
	{
		private readonly FoodStoreDbContext db;

		public CartService(FoodStoreDbContext context)
		{
			db = context;
		}
		public async Task<ResponseMessage> AddToCart(string userId, int productId, int quantity = 1)
		{
			if (productId <= 0)
			{
				return new ResponseMessage
				{
					Message = "Thiếu thông tin sản phẩm",
					IsSuccess = false,
				};
			}

			if (quantity <= 0)
			{
				return new ResponseMessage
				{
					Message = "Số lượng phải lớn hơn 0",
					IsSuccess = false,
				};
			}

			var userExists = await db.Users.AnyAsync(u => u.Id == userId);
			var productExists = await db.Products.AnyAsync(p => p.ProductId == productId);
			if (!userExists)
			{
				return new ResponseMessage
				{
					Message = "Người dùng không tồn tại",
					IsSuccess = false,
				};
			}

			if (!productExists)
			{
				return new ResponseMessage
				{
					Message = "Sản phẩm không tồn tại",
					IsSuccess = false,
				};
			}

			var cartItem = await db.Carts
					.FirstOrDefaultAsync(c => c.User_Id == userId && c.Product_Id == productId);


			if (cartItem != null)
			{
				cartItem.Quantity += quantity;
				db.Carts.Update(cartItem);
			}
			else
			{
				var product = await db.Products.FindAsync(productId);
				if (product != null)
				{
					cartItem = new Cart
					{
						User_Id = userId,
						Product_Id = productId,
						Name = product.ProductName,
						Quantity = quantity,
						Price = product.ProductPrice,
						Image = product.ProductImage
					};
					await db.Carts.AddAsync(cartItem);
				}
			}

			await db.SaveChangesAsync();

			return new ResponseMessage
			{
				Message = "Sản phẩm đã được thêm vào giỏ hàng",
				IsSuccess = true,
			};
		}

		public async Task<List<CartVM>> GetCartItemsByUser(string userId)
		{
			var cartItems = await db.Carts.Where(c => c.User_Id == userId)
			.Select(c => new CartVM {
				CartId = c.CartId,
				Name = c.Name,
				Quantity = c.Quantity,
				Price = c.Price,
				Image = c.Image,
				Subtotal = c.Quantity * c.Price,
				Product_Id = c.Product_Id
			}).ToListAsync();

			return cartItems;
		}

		public async Task<ResponseMessage> ClearCart(string userId)
		{
			var userExists = await db.Users.AnyAsync(u => u.Id == userId);
			if (!userExists)
			{
				return new ResponseMessage
				{
					Message = "Người dùng không tồn tại",
					IsSuccess = false
				};
			}

			var cartItems = await db.Carts
				.Where(c => c.User_Id == userId)
				.ToListAsync();

			db.Carts.RemoveRange(cartItems);
			await db.SaveChangesAsync();

			return new ResponseMessage
			{
				Message = "Giỏ hàng đã được xóa",
				IsSuccess = true
			};
		}

		public async Task<ResponseMessage> RemoveFromCart(string userId, int cartId)
		{
			var cartItem = await db.Carts
			.FirstOrDefaultAsync(c => c.User_Id == userId && c.CartId == cartId);

			if (cartItem == null)
			{
				return new ResponseMessage
				{
					Message = "Mục giỏ hàng không tồn tại",
					IsSuccess = false
				};
			}

			db.Carts.Remove(cartItem);
			await db.SaveChangesAsync();

			return new ResponseMessage
			{
				Message = "Mục giỏ hàng đã được xóa",
				IsSuccess = true
			};
		}

		public async Task<ResponseMessage> UpdateQuantity(string userId, int cartId, int quantity)
		{
			var cartItem = await db.Carts
					.FirstOrDefaultAsync(c => c.User_Id == userId && c.CartId == cartId);

			if (cartItem == null)
			{
				return new ResponseMessage
				{
					Message = "Mục giỏ hàng không tồn tại",
					IsSuccess = false
				};
			}

			if (quantity <= 0)
			{
				return new ResponseMessage
				{
					Message = "Số lượng phải lớn hơn 0",
					IsSuccess = false
				};
			}

			cartItem.Quantity = quantity;
			db.Carts.Update(cartItem);
			await db.SaveChangesAsync();

			return new ResponseMessage
			{
				Message = "Số lượng đã được cập nhật",
				IsSuccess = true
			};
		}
	}
}
