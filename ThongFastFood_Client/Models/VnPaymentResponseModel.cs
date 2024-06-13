﻿namespace ThongFastFood_Client.Models
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; } 
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }


		// Thêm các thuộc tính để lưu giá trị từ VnPaymentRequestModel
		public string FullName { get; set; }
		public string DeliveryAddress { get; set; }
		public string PhoneNo { get; set; }
		public string Note { get; set; }
	}

	public class VnPaymentRequestModel
	{
		public int OrderId { get; set; }
		public string FullName { get; set; }
		public string? DeliveryAddress { get; set; }
		public string? PhoneNo { get; set; }
		public string? Note { get; set; }
		public string Description { get; set; }
		public double Amount { get; set; }
		public string PaymentMethod { get; set; }
		public DateTime CreatedDate { get; set; }
	}

}