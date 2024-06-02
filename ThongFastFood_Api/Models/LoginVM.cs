using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Models
{
	public class LoginVM
	{
		[Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
		[StringLength(100, ErrorMessage = "Mật khẩu phải từ {2} đến {1} ký tự", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}
