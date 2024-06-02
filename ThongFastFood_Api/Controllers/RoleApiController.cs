using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThongFastFood_Api.Repositories.UserService;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ThongFastFood_Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class RoleApiController : ControllerBase
	{
		private readonly IRoleService roleService;

		public RoleApiController(IRoleService roleService)
		{
			this.roleService = roleService;
		}

		[HttpGet]
		public ActionResult<List<IdentityRole>> GetRoles()
		{
			var roles = roleService.GetRoles();
			return Ok(roles);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<IdentityRole>> GetRoleById(string id)
		{
			var role = await roleService.GetIdentityRoleAsync(id);
			if (role == null)
			{
				return NotFound();
			}
			return Ok(role);
		}

		[HttpPost]
		public async Task<ActionResult<IdentityRole>> AddRole(IdentityRole identityRole)
		{
			var role = await roleService.AddRoleAsync(identityRole);
			if (role == null)
			{
				return BadRequest("Vai trò đã tồn tại.");
			}
			return Ok(role);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<IdentityRole>> UpdateRole(string id, IdentityRole identityRole)
		{
			if (id != identityRole.Id)
			{
				return BadRequest("Không tìm thấy role này");
			}

			var updatedRole = await roleService.UpdateRoleAsync(identityRole);
			if (updatedRole == null)
			{
				return BadRequest("Không thể cập nhật vai trò hoặc vai trò không tồn tại");
			}
			return Ok(updatedRole);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<IdentityRole>> DeleteRole(string id)
		{
			var role = await roleService.DeleteRoleAsync(id);
			if (role == null)
			{
				return NotFound();
			}
			return Ok(role);
		}
	}
}
