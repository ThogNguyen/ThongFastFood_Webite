using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Repositories.CommentService;

namespace ThongFastFood_Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CommentApiController : ControllerBase
	{
		private readonly ICommentService _commentService;

		public CommentApiController(ICommentService commentService)
		{
			_commentService = commentService;
		}

		[HttpGet("{productId}")]
		public IActionResult GetCommentsByProductID(int productId)
		{
			var comments = _commentService.GetCommentsByProductID(productId);
			return Ok(comments);
		}

		[HttpPost]
		public IActionResult PostComment(int productId, string userId, CommentVM model)
		{
			var result = _commentService.AddComment(productId, userId, model.Comment);
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
