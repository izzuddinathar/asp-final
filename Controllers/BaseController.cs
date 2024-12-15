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
                    menus.Add(("Sales Reports", "/SalesReport/Index"));
                    menus.Add(("Sales Programs", "/SalesProgram/index"));
                    break;

                case "admin":
                    menus.Add(("User Management", "/User/Index"));
                    menus.Add(("Menu Management", "/Menu/Index"));
                    menus.Add(("Payments Management", "/Payment/Index"));
                    menus.Add(("Inventory Management", "/Inventory/Index"));
                    break;

                case "waiters":
                    menus.Add(("Tables", "/Table/Index"));
                    menus.Add(("Reservations", "/Reservation/Index"));
                    menus.Add(("Orders", "/Order/Index"));
                    break;

                case "cook":
                    menus.Add(("Orders", "/Order/Index"));
                    break;

                case "cleaner":
                    menus.Add(("Tables", "/Table/Index"));
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
