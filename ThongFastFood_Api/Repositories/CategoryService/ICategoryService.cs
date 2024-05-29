using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.CategoryService
{
    public interface ICategoryService
    {
        public List<CategoryVM> GetCategory();
        CategoryVM AddCategory(CategoryVM model);
        CategoryVM GetIdCategory(int id);
        CategoryVM UpdateCategory(int id, CategoryVM model);
        void DeleteCategory(int id);
    }
}
