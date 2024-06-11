using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.ProductService
{
    public interface IProductService
    {
        List<ProductVM> GetProducts(int? categoryId, string? sort, string? search, string? priceRange);
        ProductVM AddProduct(ProductVM model);
        ProductVM GetIdProduct(int id);
		List<ProductVM> GetProductByCategoryId(int id);
		ProductVM UpdateProduct(int id, ProductVM model);
    }
}
