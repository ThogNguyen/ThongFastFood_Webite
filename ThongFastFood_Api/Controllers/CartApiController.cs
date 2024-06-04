using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThongFastFood_Api.Models.Response;
using ThongFastFood_Api.Models;
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
		public async Task<IActionResult> ClearCart(string userId)
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
		public async Task<IActionResult> GetCartItemsByUser(string userId)
		{
			var cartItems = await _cartService.GetCartItemsByUser(userId);
			return Ok(cartItems);
		}

		[HttpDelete]
		public async Task<IActionResult> RemoveItem(string userId, int cartId)
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

		[HttpPut]
		public async Task<IActionResult> UpdateItem(string userId, int cartId, int quantity)
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
