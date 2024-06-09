using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Models
{
	public class DetailProductVM
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int ProductPrice { get; set; }
		public string? ProductImage { get; set; } // Đường dẫn lưu trữ ảnh
		public DateTime? AddDate { get; set; }
		public string? Description { get; set; }
		public int Category_Id { get; set; }
		public string UserId { get; set; }
		public List<CommentVM> Comments { get; set; }
	}


	public class CommentVM
	{
        public int CommentId { get; set; }
        public string CustomerName { get; set; }
		public DateTime ReviewDate { get; set; }
		public string Comment { get; set; }
	}
}
