using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminInfoController : Controller
    {
        public IActionResult Info()
        {
            return View();
        }
    }
}
