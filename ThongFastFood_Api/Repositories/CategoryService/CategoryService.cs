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
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName
            };

            db.Categories.Add(category);
            db.SaveChanges();
            return model;
        }

		public bool DeleteCategory(int id)
        {
			var category = db.Categories.Find(id);

			var productsInCategory = db.Products.Any(p => p.Category_Id == id);
			if (productsInCategory)
			{
				return false;
			}

			db.Categories.Remove(category);
			db.SaveChanges();
			return true;
		}

        public CategoryVM GetIdCategory(int id)
        {
            var category = db.Categories.Find(id);

            if (category != null)
            {
                var categoryVM = new CategoryVM 
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName 
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
                    CategoryName = c.CategoryName 
                }).ToList();
            return categories;
        }

        public CategoryVM UpdateCategory(int id, CategoryVM model)
        {
            var category = db.Categories.Find(id);

            if(category != null)
            {
                category.CategoryName = model.CategoryName;

                db.SaveChanges();
            }

            return model;
        }
    }
}
