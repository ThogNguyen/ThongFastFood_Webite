﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.ProductService
{
    public class ProductService : IProductService
    {
        private readonly FoodStoreDbContext db;
        public ProductService(FoodStoreDbContext context)
        {
            db = context;
        }

        public ProductVM AddProduct(ProductVM model)
        { 
            // Tạo đối tượng Product từ ProductModel
            var product = new Product
            {
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice,
                ProductImage = model.ProductImage,
                AddDate = model.AddDate,
                Description = model.Description,
                Category_Id = model.Category_Id
            };

            // Thêm sản phẩm vào cơ sở dữ liệu
            db.Products.Add(product);
            db.SaveChanges();

            // Trả về model của sản phẩm đã được thêm
            return model;
        }


        public void DeleteProduct(int id)
        {
            // Xóa sản phẩm từ cơ sở dữ liệu
            var product = db.Products.Find(id);
            if (product != null)
            {
                // Remove the product from the database
                db.Products.Remove(product);
                db.SaveChanges();
            }
        }

        public ProductVM GetIdProduct(int id)
        {
            // Lấy thông tin của sản phẩm theo ID từ cơ sở dữ liệu
            var product = db.Products.Find(id);

            if (product != null)
            {
                return new ProductVM
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductImage = product.ProductImage,
                    AddDate = product.AddDate,
                    Description = product.Description,
                    Category_Id = product.Category_Id,
                };
            }
            return null;
        }

		public List<ProductVM> GetProductByCategoryId(int id)
		{
			var products = db.Products
				.Where(p => p.Category_Id == id)
				.Select(p => new ProductVM
				{
					ProductId = p.ProductId,
					ProductName = p.ProductName,
					ProductPrice = p.ProductPrice,
					ProductImage = p.ProductImage,
					AddDate = p.AddDate,
					Description = p.Description,
					Category_Id = p.Category_Id
				})
				.ToList();

			return products;
		}

		public List<ProductVM> GetProducts()
        {
            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            var productVM = db.Products.Select(p => new ProductVM
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice,
                ProductImage = p.ProductImage, // Ensure only the filename is returned
                AddDate = p.AddDate,
                Description = p.Description,
                Category_Id = p.Category_Id
            }).ToList();

            return productVM;
        }

		public ProductVM UpdateProduct(int id, ProductVM model)
		{
			var product = db.Products.Find(id);
			if (product != null)
			{
				product.ProductName = model.ProductName;
				product.ProductPrice = model.ProductPrice;
                product.ProductImage = model.ProductImage;
                product.AddDate = model.AddDate;
				product.Description = model.Description;
				product.Category_Id = model.Category_Id;

				db.SaveChanges();

				return model;
			}
			return null;
		}
	}
}
