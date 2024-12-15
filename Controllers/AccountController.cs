using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Final.Models;

namespace Final.Controllers
{
    public class AccountController : Controller
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (dbHelper.RegisterUser(user))
                return RedirectToAction("Login");
            ViewBag.Error = "Username or Email already exists.";
            return View();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = dbHelper.LoginUser(username, password);
            if (user != null)
            {
                // Set session variables
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("Nama", user.Nama);

                // Redirect based on role
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
