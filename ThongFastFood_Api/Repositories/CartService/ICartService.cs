using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Api.Repositories.CartService
{
	public interface ICartService
	{
		Task<ResponseMessage> AddToCart(string userId, int productId, int quantity = 1);
		Task<ResponseMessage> UpdateQuantity(string userId, int cartId, int quantity);
		Task<ResponseMessage> RemoveFromCart(string userId, int cartId);
		Task<ResponseMessage> ClearCart(string userId);
		Task<List<CartVM>> GetCartItemsByUser(string userId);
	}
}
