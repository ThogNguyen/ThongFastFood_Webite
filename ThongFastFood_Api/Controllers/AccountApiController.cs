using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Repositories.UserService;

namespace ThongFastFood_Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AccountApiController : ControllerBase
	{
		private readonly IAccountService accountService;

		public AccountApiController(IAccountService accountService)
		{
			this.accountService = accountService;
		}

		/*[HttpPost]
		public async Task<IActionResult> Login(LoginVM model)
		{
			var token = await accountService.LoginAsync(model);
			if (token == null)
			{
				return Unauthorized("Invalid login attempt.");
			}

			return Ok(token);
		}*/

		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM model)
		{
			var result = await accountService.RegisterAsync(model);
			if (result.Succeeded)
			{
				return Ok("Tạo user thành công.");
			}

			return BadRequest(result.Errors);
		}
	}
}
