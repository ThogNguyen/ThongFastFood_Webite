using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Repositories.UserService;

namespace ThongFastFood_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleManagerApiController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public RoleManagerApiController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUserRole<string>>>> GetAllUserRoles()
        {
            var userRoles = await _userRoleService.GetAllAsync();
            return Ok(userRoles);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserRole(IdentityUserRole<string> userRole)
        {
            var result = await _userRoleService.UpdateAsync(userRole);
            if (result)
            {
                return Ok("Cập nhật vai trò cho user thành công");
            }
            else
            {
                return BadRequest("Cập nhật vai trò thất bại");
            }
        }
    }
}
