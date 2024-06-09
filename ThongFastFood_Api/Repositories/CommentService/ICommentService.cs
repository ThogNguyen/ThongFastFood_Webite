using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Api.Repositories.CommentService
{
	public interface ICommentService
	{
		List<CommentVM> GetCommentsByProductID(int productId);
		ResponseMessage AddComment(int productId, string userId, string comment);
	}
}
