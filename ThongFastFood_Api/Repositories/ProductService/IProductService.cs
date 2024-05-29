using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.ProductService
{
    public interface IProductService
    {
        public List<ProductVM> GetProducts();
        ProductVM AddProduct(ProductVM model);
        ProductVM GetIdProduct(int id);
        ProductVM UpdateProduct(int id, ProductVM model);
        void DeleteProduct(int id);
    }
}
