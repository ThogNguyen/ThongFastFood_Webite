using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThongFastFood_Api.Models;
using ThongFastFood_Api.Models.Response;
using ThongFastFood_Api.Repositories.UserService;

namespace ThongFastFood_Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AccountApiController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountApiController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _accountService.GetUsersAsync();
			if (users == null)
			{
				return NotFound(); 
			}
			return Ok(users);
		}

		[HttpGet]
		public async Task<IActionResult> GetUserById(string id)
		{
			var user = await _accountService.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}

		[HttpDelete]
		public async Task<IActionResult> RemoveUser(string id)
		{
			var response = await _accountService.DeleteUserAsync(id);
			if (response.IsSuccess)
			{
				return Ok(response);		
			}
			else
			{
				return BadRequest(response);
			}
		}

		[HttpPut]
		public async Task<IActionResult> PutUser(string id, UserVM userVM)
		{
			var response = await _accountService.UpdateUserAsync(id, userVM);
			if (response.IsSuccess)
			{
				return Ok(response);
			}
			else
			{
				return BadRequest(response);
			}
		}
	}
}
