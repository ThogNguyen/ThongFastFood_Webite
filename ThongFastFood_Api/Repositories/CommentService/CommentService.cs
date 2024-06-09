using Microsoft.EntityFrameworkCore;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;

namespace ThongFastFood_Api.Repositories.CommentService
{
	public class CommentService : ICommentService
	{
		private readonly FoodStoreDbContext db;

		public CommentService(FoodStoreDbContext context)
		{
			db = context;
		}

		public ResponseMessage AddComment(int productId, string userId, string comment)
		{
			var user = db.Users.FirstOrDefault(u => u.Id == userId);
			if (user == null)
			{
				return new ResponseMessage
				{
					Message = "Người dùng không tồn tại.",
					IsSuccess = false
				};
			}

			var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
			if (product == null)
			{
				return new ResponseMessage
				{
					Message = "Sản phẩm không tồn tại.",
					IsSuccess = false
				};
			}

			var newComment = new Review
			{
				CustomerName = user.FullName,
				ReviewDate = DateTime.Now,
				Comment = comment,
				Product_Id = productId,
				User_Id = userId
			};

			db.Reviews.Add(newComment);
			db.SaveChanges();

			return new ResponseMessage
			{
				Message = "Bình luận đã được thêm thành công.",
				IsSuccess = true
			};
		}

		public List<CommentVM> GetCommentsByProductID(int productId)
		{
			var comments = db.Reviews
				.Where(r => r.Product_Id == productId)
				.Select(r => new CommentVM
				{
					CommentId = r.Id,
					CustomerName = r.CustomerName,
					ReviewDate = r.ReviewDate,
					Comment = r.Comment
				})
				.ToList();

			return comments;
		}
	}
}
