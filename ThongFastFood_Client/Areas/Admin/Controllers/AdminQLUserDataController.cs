using Microsoft.AspNetCore.Mvc;

namespace ThongFastFood_Client.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminQLUserDataController : Controller
    {
        public IActionResult User()
        {
            return View();
        }
        public IActionResult CreateUser()
        {
            return View();
        }
        public IActionResult EditUser()
        {
            return View();
        }
    }
}
