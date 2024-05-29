using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Repositories.ComboService;
using ThongFastFood_Api.Repositories.ProductService;

namespace ThongFastFood_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ComboApiController : ControllerBase
    {
        private readonly IComboService _comboService;
        public ComboApiController(IComboService comboService)
        {
            _comboService = comboService;
        }


        [HttpGet]
        public IActionResult GetCombos()
        {
            var combos = _comboService.GetCombo();
            return Ok(combos);
        }

        [HttpGet("{id}")]
        public IActionResult GetCombo(int id)
        {
            var combo = _comboService.GetIdCombo(id);

            if (combo != null)
            {
                return Ok(combo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult PostCombo(ComboVM model)
        {
            if (ModelState.IsValid && model.ComboImage != null)
            {
                return Ok(_comboService.AddCombo(model));
            }
            else
            {
                return BadRequest("Thêm Thất Bại");
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutCombo(int id, ComboVM model)
        {
            if (ModelState.IsValid)
            {
                var updatedCombo = _comboService.UpdateCombo(id, model);
                if (updatedCombo != null)
                {
                    return Ok(updatedCombo);
                }
                else
                {
                    return BadRequest("Sửa thất bại");
                }
            }
            return Ok(model);
        }


        [HttpDelete("{id}")]
        public IActionResult RemoveCombo(int id)
        {
            var combo = _comboService.GetIdCombo(id);
            if (combo != null)
            {
                _comboService.DeleteCombo(id);
                return Ok("Xóa thành công");
            }
            else
            {
                return BadRequest("Xóa thất bại");
            }
        }
    }
}
