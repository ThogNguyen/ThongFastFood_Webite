using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Controllers
{
	public class MainPageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult About()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}
		public IActionResult UserInfo()
		{
			return View();
		}
		public IActionResult CustomerOrder()
		{
			return View();
		}
	}
}
