using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Repositories.CategoryService;

namespace ThongFastFood_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryApiController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryService.GetCategory();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetIdCategory(int id)
        {
            var category = _categoryService.GetIdCategory(id);

            if (category != null)
            {
                return Ok(category);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult PostCategory(CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                var addedCategory = _categoryService.AddCategory(model);
                return Ok(addedCategory);
            }
            else
            {
                return BadRequest("Thêm Thất Bại");
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutCategory(int id, CategoryVM model)
        {
            var category = _categoryService.GetIdCategory(id);

            if (category != null)
            {
                var updatedCategory = _categoryService.UpdateCategory(id, model);
                return Ok(updatedCategory);
            }
            else
            {
                return BadRequest("Sửa thất bại");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveCategory(int id)
        {
            var category = _categoryService.GetIdCategory(id);
            if (category != null)
            {
                _categoryService.DeleteCategory(id);
                return Ok("Xóa thành công");
            }
            else
            {
                return BadRequest("Xóa thất bại");
            }
        }
    }

}
