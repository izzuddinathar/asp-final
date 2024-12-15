using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Final.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly string connectionString;

        public PaymentController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // List All Payments
        public IActionResult Index()
        {
            SetSharedViewBag();
            var payments = new List<Payment>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM payments";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(new Payment
                        {
                            PaymentId = Convert.ToUInt64(reader["payment_id"]),
                            NomorMeja = Convert.ToInt32(reader["nomor_meja"]),
                            MenuId = Convert.ToUInt64(reader["menu_id"]),
                            Jumlah = Convert.ToInt32(reader["jumlah"]),
                            MetodePembayaran = reader["metode_pembayaran"].ToString(),
                            Status = reader["status"].ToString()
                        });
                    }
                }
            }

            return View(payments);
        }

        // Show Add Payment Form
        public IActionResult Create()
        {
            SetSharedViewBag();

            // Fetch available tables and menus for dropdowns
            ViewBag.Tables = GetAvailableTables();
            ViewBag.Menus = GetAvailableMenus();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Payment payment)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO payments (nomor_meja, menu_id, jumlah, metode_pembayaran, status) " +
                            "VALUES (@nomor_meja, @menu_id, @jumlah, @metode_pembayaran, @status)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nomor_meja", payment.NomorMeja);
                    command.Parameters.AddWithValue("@menu_id", payment.MenuId);
                    command.Parameters.AddWithValue("@jumlah", payment.Jumlah);
                    command.Parameters.AddWithValue("@metode_pembayaran", payment.MetodePembayaran);
                    command.Parameters.AddWithValue("@status", payment.Status);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Show Edit Payment Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();

            Payment payment = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch payment data
                var query = "SELECT * FROM payments WHERE payment_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            payment = new Payment
                            {
                                PaymentId = Convert.ToUInt64(reader["payment_id"]),
                                NomorMeja = Convert.ToInt32(reader["nomor_meja"]),
                                MenuId = Convert.ToUInt64(reader["menu_id"]),
                                Jumlah = Convert.ToInt32(reader["jumlah"]),
                                MetodePembayaran = reader["metode_pembayaran"].ToString(),
                                Status = reader["status"].ToString()
                            };
                        }
                    }
                }
            }

            // Fetch available tables and menus
            ViewBag.Tables = GetAvailableTables();
            ViewBag.Menus = GetAvailableMenus();

            return View(payment);
        }

        [HttpPost]
        public IActionResult Edit(Payment payment)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE payments SET nomor_meja = @nomor_meja, menu_id = @menu_id, jumlah = @jumlah, " +
                            "metode_pembayaran = @metode_pembayaran, status = @status WHERE payment_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", payment.PaymentId);
                    command.Parameters.AddWithValue("@nomor_meja", payment.NomorMeja);
                    command.Parameters.AddWithValue("@menu_id", payment.MenuId);
                    command.Parameters.AddWithValue("@jumlah", payment.Jumlah);
                    command.Parameters.AddWithValue("@metode_pembayaran", payment.MetodePembayaran);
                    command.Parameters.AddWithValue("@status", payment.Status);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Fetch Available Tables
        private List<int> GetAvailableTables()
        {
            var tables = new List<int>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT DISTINCT nomor_meja FROM tables";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(Convert.ToInt32(reader["nomor_meja"]));
                    }
                }
            }

            return tables;
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

        // Handle Delete Payment
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM payments WHERE payment_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
