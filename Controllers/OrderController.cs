using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Final.Controllers
{
    public class OrderController : BaseController
    {
        private readonly string connectionString;

        public OrderController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // List All Orders
        public IActionResult Index()
        {
            SetSharedViewBag();
            var orders = new List<Order>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM orders";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            OrderId = Convert.ToUInt64(reader["order_id"]),
                            NomorMeja = Convert.ToInt32(reader["nomor_meja"]),
                            MenuId = Convert.ToUInt64(reader["menu_id"]),
                            Jumlah = Convert.ToInt32(reader["jumlah"]),
                            StatusPesanan = reader["status_pesanan"].ToString()
                        });
                    }
                }
            }

            return View(orders);
        }

        // Show Add Order Form
        public IActionResult Create()
        {
            SetSharedViewBag();

            // Fetch available tables and menus for dropdowns
            ViewBag.Tables = GetAvailableTables();
            ViewBag.Menus = GetAvailableMenus();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO orders (nomor_meja, menu_id, jumlah, status_pesanan) " +
                            "VALUES (@nomor_meja, @menu_id, @jumlah, @status_pesanan)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nomor_meja", order.NomorMeja);
                    command.Parameters.AddWithValue("@menu_id", order.MenuId);
                    command.Parameters.AddWithValue("@jumlah", order.Jumlah);
                    command.Parameters.AddWithValue("@status_pesanan", order.StatusPesanan);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Show Edit Order Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();

            Order order = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch order data
                var query = "SELECT * FROM orders WHERE order_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new Order
                            {
                                OrderId = Convert.ToUInt64(reader["order_id"]),
                                NomorMeja = Convert.ToInt32(reader["nomor_meja"]),
                                MenuId = Convert.ToUInt64(reader["menu_id"]),
                                Jumlah = Convert.ToInt32(reader["jumlah"]),
                                StatusPesanan = reader["status_pesanan"].ToString()
                            };
                        }
                    }
                }
            }

            // Fetch available tables and menus
            ViewBag.Tables = GetAvailableTables();
            ViewBag.Menus = GetAvailableMenus();

            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE orders SET nomor_meja = @nomor_meja, menu_id = @menu_id, jumlah = @jumlah, " +
                            "status_pesanan = @status_pesanan WHERE order_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", order.OrderId);
                    command.Parameters.AddWithValue("@nomor_meja", order.NomorMeja);
                    command.Parameters.AddWithValue("@menu_id", order.MenuId);
                    command.Parameters.AddWithValue("@jumlah", order.Jumlah);
                    command.Parameters.AddWithValue("@status_pesanan", order.StatusPesanan);
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

        // Handle Delete Order
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM orders WHERE order_id = @id";
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
