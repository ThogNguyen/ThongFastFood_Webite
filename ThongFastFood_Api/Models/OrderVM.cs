﻿using System.ComponentModel.DataAnnotations.Schema;

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
	}
}
