using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult ListProduct()
		{
			return View();
		}

		public IActionResult DetailProduct()
		{
			return View();
		}
	}
}
