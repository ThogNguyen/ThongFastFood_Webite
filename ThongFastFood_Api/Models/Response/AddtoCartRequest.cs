namespace ThongFastFood_Api.Models.Response
{
	public class AddtoCartRequest
	{
		public string UserId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
	}
}
