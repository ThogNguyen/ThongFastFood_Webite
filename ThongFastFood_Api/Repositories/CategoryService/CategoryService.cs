using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly FoodStoreDbContext db;

        public CategoryService(FoodStoreDbContext context)
        {
            db = context;
        }

        public CategoryVM AddCategory(CategoryVM model)
        {
            var category = new Category
            {
                CategoryName = model.CategoryName,
                IsActive = true
            };

            db.Categories.Add(category);
            db.SaveChanges();
            return model;
        }

        public CategoryVM GetIdCategory(int id)
        {
            var category = db.Categories.Find(id);

            if (category != null)
            {
                var categoryVM = new CategoryVM 
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    IsActive = category.IsActive
                };
                return categoryVM;
            }
            else
            {
                return null;
            }
        }

        public List<CategoryVM> GetCategory()
        {
            var categories = db.Categories.Select(c => 
                new CategoryVM { 
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
					IsActive = c.IsActive
				}).ToList();
            return categories;
        }

        public CategoryVM UpdateCategory(int id, CategoryVM model)
        {
            var category = db.Categories.Find(id);

            if(category != null)
            {
                category.CategoryName = model.CategoryName;
                category.IsActive = model.IsActive;

                // chỉnh trạng thái loại thì các sản phẩm giống trạng thái của loại
                var relatedProducts = db.Products.Where(p => p.Category_Id == id).ToList();
                foreach (var product in relatedProducts)
                {
                    product.IsActive = model.IsActive;
                }

                db.SaveChanges();
            }

            return model;
        }
    }
}
