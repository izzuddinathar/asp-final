using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;

namespace Final.Controllers
{
    public class MenuController : BaseController
    {
        private readonly string connectionString;

        public MenuController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // List All Menus
        public IActionResult Index()
        {
            SetSharedViewBag(); // Ensure RBAC menus are available

            var menus = new List<Menu>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM menus";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menus.Add(new Menu
                        {
                            MenuId = Convert.ToUInt64(reader["menu_id"]),
                            NamaMenu = reader["nama_menu"].ToString(),
                            Deskripsi = reader["deskripsi"].ToString(),
                            Harga = Convert.ToDecimal(reader["harga"]),
                            Kategori = reader["kategori"].ToString(),
                            Gambar = reader["gambar"].ToString()
                        });
                    }
                }
            }
            return View(menus);
        }

        // Show Add Menu Form
        public IActionResult Create()
        {
            SetSharedViewBag();
            return View();
        }

        // Handle Add Menu Form Submission
        [HttpPost]
        public IActionResult Create(Menu menu)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO menus (nama_menu, deskripsi, harga, kategori, gambar) " +
                            "VALUES (@nama_menu, @deskripsi, @harga, @kategori, @gambar)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama_menu", menu.NamaMenu);
                    command.Parameters.AddWithValue("@deskripsi", menu.Deskripsi);
                    command.Parameters.AddWithValue("@harga", menu.Harga);
                    command.Parameters.AddWithValue("@kategori", menu.Kategori);
                    command.Parameters.AddWithValue("@gambar", menu.Gambar ?? "default.png");
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        // Show Edit Menu Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();
            Menu menu = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM menus WHERE menu_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            menu = new Menu
                            {
                                MenuId = Convert.ToUInt64(reader["menu_id"]),
                                NamaMenu = reader["nama_menu"].ToString(),
                                Deskripsi = reader["deskripsi"].ToString(),
                                Harga = Convert.ToDecimal(reader["harga"]),
                                Kategori = reader["kategori"].ToString(),
                                Gambar = reader["gambar"].ToString()
                            };
                        }
                    }
                }
            }
            return View(menu);
        }

        // Handle Edit Menu Form Submission
        [HttpPost]
        public IActionResult Edit(Menu menu)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE menus SET nama_menu = @nama_menu, deskripsi = @deskripsi, harga = @harga, " +
                            "kategori = @kategori, gambar = @gambar WHERE menu_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", menu.MenuId);
                    command.Parameters.AddWithValue("@nama_menu", menu.NamaMenu);
                    command.Parameters.AddWithValue("@deskripsi", menu.Deskripsi);
                    command.Parameters.AddWithValue("@harga", menu.Harga);
                    command.Parameters.AddWithValue("@kategori", menu.Kategori);
                    command.Parameters.AddWithValue("@gambar", menu.Gambar ?? "default.png");
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }

        // Handle Delete Menu
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM menus WHERE menu_id = @id";
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
