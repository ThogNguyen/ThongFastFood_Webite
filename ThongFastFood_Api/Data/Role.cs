using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Tên vai trò không được bỏ trống")]
        [StringLength(10)]
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
