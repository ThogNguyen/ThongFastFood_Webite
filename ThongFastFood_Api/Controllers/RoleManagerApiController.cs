using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Repositories.UserService;

namespace ThongFastFood_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleManagerApiController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerApiController(IUserRoleService userRoleService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRoleService = userRoleService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUserRoles()
        {
            var userRoles = await _userRoleService.GetAll();

            return Ok(userRoles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleByUserId(string id)
        {
            var user = await _userRoleService.GetUserRolesByUserId(id);
            if(user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Không tìm thấy user này");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutUserRole(string id, UserRolesVM userRole)
        {
            userRole.UserId = id;
            var updatedUserRole = await _userRoleService.UpdateUserRole(id, userRole);
            if (updatedUserRole != null)
            {
                return Ok(updatedUserRole); // Trả về thông tin người dùng và vai trò sau khi cập nhật thành công
            }
            else
            {
                return BadRequest("Cập nhật vai trò thất bại");
            }
        }
    }
}
