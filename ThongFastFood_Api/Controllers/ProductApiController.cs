﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Repositories.ProductService;

namespace ThongFastFood_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productSer;
        public ProductApiController(IProductService productSer)
        {
            _productSer = productSer;
        }

        [HttpGet]
        public IActionResult GetProducts(int? categoryId, string? sort, string? search, string? priceRange)
        {
            var products = _productSer.GetProducts(categoryId, sort, search, priceRange);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetIdProduct(int id)
        {
            var product = _productSer.GetIdProduct(id);

            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest("Không tìm thấy id này");
            }
        }

		[HttpGet("{id}")]
		public IActionResult GetProductsByCategory(int id)
		{
			var products = _productSer.GetProductByCategoryId(id);
			return Ok(products);
		}

		[HttpPost]
        public IActionResult PostProduct(ProductVM model)
        {
            if(model.Category_Id != null)
            {
                if (ModelState.IsValid && model.ProductImage != null)
                {
                    return Ok(_productSer.AddProduct(model));
                }
                else
                {
                    return BadRequest("Thêm Thất Bại");
                }
            }
            else
            {
                return BadRequest("Mã loại không được bỏ trống");
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, ProductVM model)
        {
            if (ModelState.IsValid)
            {
                var updatedProduct = _productSer.UpdateProduct(id, model);
                if (updatedProduct != null)
                {
                    return Ok(updatedProduct);
                }
                else
                {
                    return BadRequest("Sửa thất bại");
                }
            }
            return Ok(model);
        }
    }
}
