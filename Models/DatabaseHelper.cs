using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Final.Models
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=localhost;Database=laravel;User=root;Password=password;";

        // Hash password
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public User LoginUser(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Query to verify user credentials
                var query = "SELECT * FROM users WHERE username = @username AND password = @password";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", HashPassword(password));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = Convert.ToUInt64(reader["user_id"]),
                                Nama = reader["nama"].ToString(),
                                Username = reader["username"].ToString(),
                                Role = reader["role"].ToString()
                            };
                        }
                    }
                }
            }
            return null; // No user found
        }


        // Register user
        public bool RegisterUser(User user)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if username or email already exists
                var checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username OR email = @email";
                using (var checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@username", user.Username);
                    checkCommand.Parameters.AddWithValue("@email", user.Email);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0) return false;
                }

                // Insert user data
                var query = @"INSERT INTO users (nama, no_telp, email, foto, username, password, role, created_at)
                              VALUES (@nama, @no_telp, @email, @foto, @username, @password, @role, NOW())";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama", user.Nama);
                    command.Parameters.AddWithValue("@no_telp", user.NoTelp);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@foto", user.Foto ?? "default.png");
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", HashPassword(user.Password));
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.ExecuteNonQuery();
                }
                return true;
            }
        }
    }
}
