using System.ComponentModel.DataAnnotations.Schema;

namespace ThongFastFood_Api.Models
{
	public class OrderVM
	{
		public string? CustomerName { get; set; }
		public string? DeliveryAddress { get; set; }
		public string? PhoneNo { get; set; }
		public string? Status { get; set; }
		public string? Note { get; set; }
	}
		
	public static class OrderStatus
	{
		public const string Pending = "Đang xử lí";
		public const string Completed = "Đã xác nhận";
		public const string InProgress = "Đang giao";
		public const string Delivered = "Đã giao";
		public const string Cancelled = "Đã hủy";

		public static readonly List<string> AdminStatuses = new List<string> 
		{ Pending, Completed, InProgress, Delivered };
	}

	public static class TypeOfPayment
	{
		public const string COD = "Ship (COD)";
		public const string VNPAY = "VNPAY";

		public static readonly List<string> ListPayments = new List<string>
		{ COD, VNPAY };
	}
}
