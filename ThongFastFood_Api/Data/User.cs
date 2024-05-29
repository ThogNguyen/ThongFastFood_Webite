using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Tên không được bỏ trống")]
        [StringLength(50)]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Email không được bỏ trống")]
        [EmailAddress(ErrorMessage = "Sai định dạng Email")]
        [StringLength(50)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        // mẫu: 012 345 6789 | số 0 là mặc định phải có ở đầu + 9 số sau
        [StringLength(15)]
        public string PhoneNo { get; set; }


        [Required(ErrorMessage = "Địa chỉ không được bỏ trống")]
        [StringLength(100)]
        public string Address { get; set; }


        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có độ dài từ 6 đến 12 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Xác nhận mật khẩu không được bỏ trống")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có độ dài từ 6 đến 12 ký tự")]
        [Column(TypeName = "nvarchar(100)")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        [DataType(DataType.Password)]
        public string ComfirmPassword { get; set; }


        //khóa ngoại
        [ForeignKey("Role")]
        public int Role_Id { get; set; }
        public Role Role { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
