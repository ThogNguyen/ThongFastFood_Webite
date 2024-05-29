using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}
		public IActionResult Register()
		{
			return View();
		}
	}
}
