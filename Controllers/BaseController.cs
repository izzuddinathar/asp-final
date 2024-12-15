using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    public class BaseController : Controller
    {
        protected void SetSharedViewBag()
        {
            var role = HttpContext.Session.GetString("Role") ?? "Guest";
            var user = HttpContext.Session.GetString("Nama") ?? "Unknown User";

            // Role-based menus
            var menus = new List<(string Label, string Route)>();
            switch (role.ToLower())
            {
                case "owner":
                    menus.Add(("Sales Reports", "/Reports"));
                    menus.Add(("Sales Programs", "/Programs"));
                    break;

                case "admin":
                    menus.Add(("User Management", "/User/Index"));
                    menus.Add(("Menu Management", "/Menu"));
                    menus.Add(("Payments Management", "/Payments"));
                    menus.Add(("Inventory Management", "/Inventory"));
                    break;

                case "waiters":
                    menus.Add(("Tables", "/Tables"));
                    menus.Add(("Reservations", "/Reservations"));
                    menus.Add(("Orders", "/Orders"));
                    break;

                case "cook":
                    menus.Add(("Orders", "/Orders"));
                    break;

                case "cleaner":
                    menus.Add(("Tables", "/Tables"));
                    break;

                default:
                    menus.Add(("Home", "/"));
                    break;
            }

            // Set data for shared layout
            ViewBag.Menus = menus;
            ViewBag.User = user;
            ViewBag.Role = role;
        }
    }
}
