using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;

namespace Final.Controllers
{
    public class UserController : BaseController
    {
        private string connectionString = "Server=localhost;Database=laravel;User=root;Password=password;";

        // List All Users
        public IActionResult Index()
        {
            SetSharedViewBag(); // Ensures ViewBag.Menus is populated

            // Fetch users data (existing logic)
            var users = new List<User>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM users";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserId = Convert.ToUInt64(reader["user_id"]),
                            Nama = reader["nama"].ToString(),
                            NoTelp = reader["no_telp"].ToString(),
                            Email = reader["email"].ToString(),
                            Foto = reader["foto"].ToString(),
                            Role = reader["role"].ToString()
                        });
                    }
                }
            }

            return View(users);
        }



        // Show Create Form
        public IActionResult Create()
        {
            SetSharedViewBag();
            return View();
        }

        // Handle Create User
        [HttpPost]
        public IActionResult Create(User user)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO users (nama, no_telp, email, foto, username, password, role) " +
                            "VALUES (@nama, @no_telp, @email, @foto, @username, @password, @role)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama", user.Nama);
                    command.Parameters.AddWithValue("@no_telp", user.NoTelp);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@foto", user.Foto ?? "default.png");
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", user.Password); // Assume hashed
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Show Edit Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();
            User user = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM users WHERE user_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserId = Convert.ToUInt64(id), // Fixed here
                                Nama = reader["nama"].ToString(),
                                NoTelp = reader["no_telp"].ToString(),
                                Email = reader["email"].ToString(),
                                Foto = reader["foto"].ToString(),
                                Username = reader["username"].ToString(),
                                Role = reader["role"].ToString()
                            };
                        }
                    }
                }
            }

            return View(user);
        }


        // Handle Edit User
        [HttpPost]
        public IActionResult Edit(User user)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE users SET nama = @nama, no_telp = @no_telp, email = @email, foto = @foto, " +
                            "role = @role WHERE user_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", user.UserId); // Fixed here
                    command.Parameters.AddWithValue("@nama", user.Nama);
                    command.Parameters.AddWithValue("@no_telp", user.NoTelp);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@foto", user.Foto ?? "default.png");
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }


        // Handle Delete User
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM users WHERE user_id = @id";
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
