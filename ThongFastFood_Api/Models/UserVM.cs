using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Models
{
	public class UserVM
	{
        public string Id { get; set; }
        [Required(ErrorMessage = "Tên không được bỏ trống")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Email không được bỏ trống")]
		[EmailAddress(ErrorMessage = "Sai định dạng Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
		[RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
		// mẫu: 012 345 6789 | số 0 là mặc định phải có ở đầu + 9 số sau
		public string PhoneNumber { get; set; }
		[Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
		public string Address { get; set; }
	}
}
