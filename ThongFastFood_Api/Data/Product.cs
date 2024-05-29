using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }


        [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống")]
        [StringLength(50)]
        public string ProductName { get; set; }


        [Required(ErrorMessage = "Giá gốc sản phẩm không được bỏ trống")]
        public int ProductPrice { get; set; }


        [Required(ErrorMessage = "Hình sản phẩm không được bỏ trống")]
        [StringLength(400)]
        public string ProductImage { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime? AddDate { get; set; }


        [Required(ErrorMessage = "Mô tả sản phẩm không được bỏ trống")]
        [StringLength(400)]
        public string Description { get; set; }


        //khóa ngoại
        [ForeignKey("Category")]
        public int Category_Id { get; set; }
        public Category Category { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Cart> GioHangs { get; set; }
        public ICollection<ComboItem> ComboItems { get; set; }
    }
}
