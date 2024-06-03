using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Repositories.CartService;

namespace ThongFastFood_Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CartApiController : ControllerBase
	{
		private readonly ICartService _cartService;

		public CartApiController(ICartService cartService)
		{
			_cartService = cartService;
		}

		[HttpPost]
		public async Task<IActionResult> AddItem(string userId, int productId, int quantity = 1)
		{
			var cartItem = await _cartService.AddToCart(userId, productId, quantity);

			if (cartItem.IsSuccess)
			{
				return Ok(cartItem);
			}
			else
			{
				return BadRequest(cartItem.Message);
			}
		}

		[HttpDelete]
		public async Task<IActionResult> ClearCart([FromQuery] string userId)
		{
			var result = await _cartService.ClearCart(userId);

			if (result.IsSuccess)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetCartItemsByUser([FromQuery] string userId)
		{
			var cartItems = await _cartService.GetCartItemsByUser(userId);
			return Ok(cartItems);
		}

		[HttpDelete("remove")]
		public async Task<IActionResult> RemoveItem([FromQuery] string userId, [FromQuery] int cartId)
		{
			var result = await _cartService.RemoveFromCart(userId, cartId);

			if (result.IsSuccess)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}

		[HttpPut("update")]
		public async Task<IActionResult> UpdateItem([FromQuery] string userId, [FromQuery] int cartId, [FromQuery] int quantity)
		{
			var result = await _cartService.UpdateQuantity(userId, cartId, quantity);

			if (result.IsSuccess)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
	}
}
