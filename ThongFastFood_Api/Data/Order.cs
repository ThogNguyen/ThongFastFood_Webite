using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThongFastFood_Api.Data
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderTime { get; set; }
        [StringLength(50)]
        public string CustomerName { get; set; }
        [StringLength(100)]
        public string DeliveryAddress { get; set; }
        [StringLength(10)]
        public string PhoneNo { get; set; }
        [StringLength(20)]
        public string Status { get; set; }  
        public int TotalAmount { get; set; }
        [StringLength(400)]
        public string Note { get; set; }


		//khóa ngoại
		[ForeignKey("User")]
		[Column(TypeName = "nvarchar(450)")]
		public string Customer_Id { get; set; }
		public ApplicationUser User { get; set; }
		
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
