using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Final.Controllers
{
    public class TableController : BaseController
    {
        private readonly string connectionString;

        public TableController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // List All Tables
        public IActionResult Index()
        {
            SetSharedViewBag();
            var tables = new List<Table>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM tables";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(new Table
                        {
                            TableId = Convert.ToUInt64(reader["table_id"]),
                            NomorMeja = Convert.ToInt32(reader["nomor_meja"]),
                            Kapasitas = Convert.ToInt32(reader["kapasitas"]),
                            Status = reader["status"].ToString()
                        });
                    }
                }
            }

            return View(tables);
        }

        // Show Add Table Form
        public IActionResult Create()
        {
            SetSharedViewBag();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Table table)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO tables (nomor_meja, kapasitas, status) VALUES (@nomor_meja, @kapasitas, @status)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nomor_meja", table.NomorMeja);
                    command.Parameters.AddWithValue("@kapasitas", table.Kapasitas);
                    command.Parameters.AddWithValue("@status", table.Status);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Show Edit Table Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();
            Table table = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM tables WHERE table_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            table = new Table
                            {
                                TableId = Convert.ToUInt64(reader["table_id"]),
                                NomorMeja = Convert.ToInt32(reader["nomor_meja"]),
                                Kapasitas = Convert.ToInt32(reader["kapasitas"]),
                                Status = reader["status"].ToString()
                            };
                        }
                    }
                }
            }

            return View(table);
        }

        [HttpPost]
        public IActionResult Edit(Table table)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE tables SET nomor_meja = @nomor_meja, kapasitas = @kapasitas, status = @status " +
                            "WHERE table_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", table.TableId);
                    command.Parameters.AddWithValue("@nomor_meja", table.NomorMeja);
                    command.Parameters.AddWithValue("@kapasitas", table.Kapasitas);
                    command.Parameters.AddWithValue("@status", table.Status);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Handle Delete
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM tables WHERE table_id = @id";
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
