using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Drawing.Printing;
using System.Net.Http;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                IsActive = true,
                Category_Id = model.Category_Id
            };

            // Thêm sản phẩm vào cơ sở dữ liệu
            db.Products.Add(product);
            db.SaveChanges();

            // Trả về model của sản phẩm đã được thêm
            return model;
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
                    IsActive = product.IsActive,
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
                    IsActive = p.IsActive,
                    Category_Id = p.Category_Id
				})
				.ToList();

			return products;
		}

		public List<ProductVM> GetProducts(int? categoryId, string? sort, string? search, string? priceRange)
        {
            IQueryable<Product> query = db.Products;

            #region Lọc sản phẩm theo loại
            if (categoryId != null)
            {
                query = query.Where(p => p.Category_Id == categoryId);
            }
            #endregion

            #region Lọc sản phẩm theo tên
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.ProductName.Contains(search));
			}

			#endregion

			#region Lọc sản phẩm theo giá
			if (!string.IsNullOrEmpty(priceRange))
			{
				switch (priceRange)
				{
					case "0-20k":
						query = query.Where(p => p.ProductPrice >= 0 && p.ProductPrice <= 20000);
						break;
					case "20-50k":
						query = query.Where(p => p.ProductPrice >= 20000 && p.ProductPrice <= 50000);
						break;
					case "50k":
						query = query.Where(p => p.ProductPrice > 50000);
						break;
                    default:
                        break;
				}
			}
			#endregion
			#region Sắp xếp sản phẩm
			if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "name_asc":
                        query = query.OrderBy(x => x.ProductName);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(x => x.ProductName);
                        break;
                    case "price_asc":
                        query = query.OrderBy(x => x.ProductPrice);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(x => x.ProductPrice);
                        break;
                    default:
                        query = query.OrderBy(x => x.ProductId);
                        break;
                }
            }
            #endregion

            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            var productVM = query.Select(p => new ProductVM
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice,
                ProductImage = p.ProductImage,
                AddDate = p.AddDate,
                Description = p.Description,
                IsActive = p.IsActive,
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
                product.IsActive = model.IsActive;
                product.Category_Id = model.Category_Id;

				db.SaveChanges();

				return model;
			}
			return null;    
		}
    }
}
