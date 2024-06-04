using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThongFastFood_Api.Models
{
	public class CartVM
	{
		public int CartId { get; set; }
		
		public string Name { get; set; }
		[Required(ErrorMessage = "Số lượng sản phẩm không được để trống.")]
		[Range(1, 50, ErrorMessage = "Số lượng sản phẩm phải từ 1 đến 50.")]
		public int Quantity { get; set; }

		public int Price { get; set; }
		public string? Image { get; set; }


		[NotMapped]
		public int Subtotal
		{
			get { return Quantity * Price; }
			set { } 
		}
		public int? Product_Id { get; set; }
		public string User_Id { get; set; }


		[NotMapped]
		public int TotalAmount { get; set; }

		// Tính toán TotalAmount động
		public static int CalculateTotalAmount(IEnumerable<CartVM> cartItems)
		{
			return cartItems.Sum(item => item.Subtotal);
		}
	}
}
