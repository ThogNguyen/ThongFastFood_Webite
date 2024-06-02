using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Models
{
	public class RegisterVM
	{
		[Required(ErrorMessage = "Email không được bỏ trống")]
		[EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Họ và tên không được bỏ trống")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
		[RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
		[StringLength(100, ErrorMessage = "Mật khẩu phải từ {2} đến {1} ký tự", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
		public string ConfirmPassword { get; set; }  
	}
}
