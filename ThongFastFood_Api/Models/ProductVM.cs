﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ThongFastFood_Api.Data;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace ThongFastFood_Api.Models
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được trống")]
        public string ProductName { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 1")]
        public int ProductPrice { get; set; }
        public string? ProductImage { get; set; } 
        public DateTime? AddDate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int Category_Id { get; set; }
    }
}
