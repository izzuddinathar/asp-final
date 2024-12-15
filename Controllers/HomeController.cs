using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            SetSharedViewBag(); // Reuse menu logic
            return View();
        }
    }
}
