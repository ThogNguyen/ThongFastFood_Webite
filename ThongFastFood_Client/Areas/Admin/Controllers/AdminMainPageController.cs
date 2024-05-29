using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Route("Admin")]
    public class AdminMainPageController : Controller
	{
		public IActionResult MainPage()
		{
			return View();
		}
	}
}
