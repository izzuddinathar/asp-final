using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Final.Controllers
{
    public class InventoryController : BaseController
    {
        private readonly string connectionString;

        public InventoryController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // List All Inventory Items
        public IActionResult Index()
        {
            SetSharedViewBag();
            var inventories = new List<Inventory>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM inventories";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inventories.Add(new Inventory
                        {
                            InventoryId = Convert.ToUInt64(reader["inventory_id"]),
                            NamaBahanPokok = reader["nama_bahan_pokok"].ToString(),
                            Jumlah = Convert.ToInt32(reader["jumlah"]),
                            Satuan = reader["satuan"].ToString(),
                            Supplier = reader["supplier"].ToString()
                        });
                    }
                }
            }

            return View(inventories);
        }

        // Show Add Inventory Form
        public IActionResult Create()
        {
            SetSharedViewBag();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Inventory inventory)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO inventories (nama_bahan_pokok, jumlah, satuan, supplier) " +
                            "VALUES (@nama_bahan_pokok, @jumlah, @satuan, @supplier)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama_bahan_pokok", inventory.NamaBahanPokok);
                    command.Parameters.AddWithValue("@jumlah", inventory.Jumlah);
                    command.Parameters.AddWithValue("@satuan", inventory.Satuan);
                    command.Parameters.AddWithValue("@supplier", inventory.Supplier);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Show Edit Inventory Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();

            Inventory inventory = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM inventories WHERE inventory_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            inventory = new Inventory
                            {
                                InventoryId = Convert.ToUInt64(reader["inventory_id"]),
                                NamaBahanPokok = reader["nama_bahan_pokok"].ToString(),
                                Jumlah = Convert.ToInt32(reader["jumlah"]),
                                Satuan = reader["satuan"].ToString(),
                                Supplier = reader["supplier"].ToString()
                            };
                        }
                    }
                }
            }

            return View(inventory);
        }

        [HttpPost]
        public IActionResult Edit(Inventory inventory)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE inventories SET nama_bahan_pokok = @nama_bahan_pokok, jumlah = @jumlah, " +
                            "satuan = @satuan, supplier = @supplier WHERE inventory_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", inventory.InventoryId);
                    command.Parameters.AddWithValue("@nama_bahan_pokok", inventory.NamaBahanPokok);
                    command.Parameters.AddWithValue("@jumlah", inventory.Jumlah);
                    command.Parameters.AddWithValue("@satuan", inventory.Satuan);
                    command.Parameters.AddWithValue("@supplier", inventory.Supplier);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Handle Delete Inventory
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM inventories WHERE inventory_id = @id";
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
