using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; }
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ReviewDate { get; set; }
        [Required]
        [StringLength(400)]
        public string Comment { get; set; }
        // khóa ngoại
        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }
        public User User { get; set; }
    }
}
