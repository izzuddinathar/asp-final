using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Final.Controllers
{
    public class SalesReportController : BaseController
    {
        private readonly string connectionString;

        public SalesReportController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // List All Sales Reports
        public IActionResult Index()
        {
            SetSharedViewBag();
            var reports = new List<SalesReport>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM sales_reports";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reports.Add(new SalesReport
                        {
                            ReportId = Convert.ToUInt64(reader["report_id"]),
                            Tanggal = Convert.ToDateTime(reader["tanggal"]),
                            MenuId = Convert.ToUInt64(reader["menu_id"]),
                            Harga = Convert.ToDecimal(reader["harga"]),
                            Total = Convert.ToDecimal(reader["total"])
                        });
                    }
                }
            }

            return View(reports);
        }

        // Show Add Report Form
        public IActionResult Create()
        {
            SetSharedViewBag();

            // Fetch available menus for the dropdown
            ViewBag.Menus = GetAvailableMenus();

            return View();
        }

        [HttpPost]
        public IActionResult Create(SalesReport report)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO sales_reports (tanggal, menu_id, harga, total) " +
                            "VALUES (@tanggal, @menu_id, @harga, @total)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tanggal", report.Tanggal);
                    command.Parameters.AddWithValue("@menu_id", report.MenuId);
                    command.Parameters.AddWithValue("@harga", report.Harga);
                    command.Parameters.AddWithValue("@total", report.Total);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Show Edit Report Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();

            SalesReport report = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM sales_reports WHERE report_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            report = new SalesReport
                            {
                                ReportId = Convert.ToUInt64(reader["report_id"]),
                                Tanggal = Convert.ToDateTime(reader["tanggal"]),
                                MenuId = Convert.ToUInt64(reader["menu_id"]),
                                Harga = Convert.ToDecimal(reader["harga"]),
                                Total = Convert.ToDecimal(reader["total"])
                            };
                        }
                    }
                }
            }

            // Fetch available menus for dropdown
            ViewBag.Menus = GetAvailableMenus();

            return View(report);
        }

        [HttpPost]
        public IActionResult Edit(SalesReport report)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE sales_reports SET tanggal = @tanggal, menu_id = @menu_id, harga = @harga, total = @total " +
                            "WHERE report_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", report.ReportId);
                    command.Parameters.AddWithValue("@tanggal", report.Tanggal);
                    command.Parameters.AddWithValue("@menu_id", report.MenuId);
                    command.Parameters.AddWithValue("@harga", report.Harga);
                    command.Parameters.AddWithValue("@total", report.Total);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Handle Delete Report
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM sales_reports WHERE report_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Fetch Available Menus
        private List<(ulong MenuId, string NamaMenu)> GetAvailableMenus()
        {
            var menus = new List<(ulong, string)>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT menu_id, nama_menu FROM menus";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menus.Add((Convert.ToUInt64(reader["menu_id"]), reader["nama_menu"].ToString()));
                    }
                }
            }

            return menus;
        }
    }
}
