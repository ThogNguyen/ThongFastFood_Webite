namespace ThongFastFood_Api.Models
{
	public class CustomerOrderView
	{
		public List<OrderView> Orders { get; set; }
	}

	public class OrderView
	{
		public int OrderId { get; set; }
		public DateTime OrderTime { get; set; }
		public string CustomerName { get; set; }
		public string DeliveryAddress { get; set; }
		public string PhoneNo { get; set; }
		public string Status { get; set; }
		public int TotalAmount { get; set; }
		public string Note { get; set; }
		public List<OrderDetailView> OrderDetails { get; set; }
	}

	public class OrderDetailView
	{
		public int OrderDetailId { get; set; }
		public int Quantity { get; set; }
		public int SubTotal { get; set; }
		public int Product_Id { get; set; }
		public string ProductName { get; set; }
		public int ProductPrice { get; set; }
		public string ProductImage { get; set; }
	}
}
