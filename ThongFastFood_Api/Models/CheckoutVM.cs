namespace ThongFastFood_Api.Models
{
	public class CheckoutVM
	{
		public OrderVM Order { get; set; }
		public List<CartVM> CartItems { get; set; }
	}
}
