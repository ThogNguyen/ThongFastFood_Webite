using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Models
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Tên loại sản phẩm không được trống")]
        public string CategoryName { get; set; }
    }
}
