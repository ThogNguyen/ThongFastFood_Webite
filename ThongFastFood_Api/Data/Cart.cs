using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Range(0, 50)]
        public int Quantity { get; set; }
        [Required]
        [StringLength(400)]
        public string Image { get; set; }
        [Required]
        public int Subtotal { get; set; }
        [Required]
        public int TotalAmount { get; set; }
        //khóa ngoại
        [ForeignKey("User")]
        [Column(TypeName = "nvarchar(450)")]
        public string User_Id { get; set; }
        public ApplicationUser User { get; set; }

        // Cho phép null nếu chỉ có combo trong giỏ hàng
        [ForeignKey("Product")]
        public int? Product_Id { get; set; }
        public Product Product { get; set; }
    }
}
