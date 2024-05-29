using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Controllers
{
	public class CartController : Controller
	{
		public IActionResult CartInfo()
		{
			return View();
		}

		public IActionResult CheckOut()
		{
			return View();
		}
	}
}
