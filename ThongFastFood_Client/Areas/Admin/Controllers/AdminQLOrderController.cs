using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminQLOrderController : Controller
    {
        public IActionResult Order()
        {
            return View();
        }
    }
}
