using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Final.Controllers
{
    public class SalesProgramController : BaseController
    {
        private readonly string connectionString;

        public SalesProgramController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // List All Sales Programs
        public IActionResult Index()
        {
            SetSharedViewBag();
            var salesPrograms = new List<SalesProgram>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM sales_programs";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        salesPrograms.Add(new SalesProgram
                        {
                            SalesProgramId = Convert.ToUInt64(reader["program_id"]),
                            NamaProgram = reader["nama_program"].ToString(),
                            Diskon = Convert.ToDecimal(reader["diskon"]),
                            TanggalBerlaku = Convert.ToDateTime(reader["tanggal_berlaku"]),
                            JamBerlaku = TimeSpan.Parse(reader["jam_berlaku"].ToString()),
                            MenuId = reader["menu_id"] != DBNull.Value ? Convert.ToUInt64(reader["menu_id"]) : (ulong?)null,
                            KategoriProduk = reader["kategori_produk"] != DBNull.Value ? reader["kategori_produk"].ToString() : null
                        });
                    }
                }
            }

            return View(salesPrograms);
        }

        // Show Add Sales Program Form
        public IActionResult Create()
        {
            SetSharedViewBag();
            ViewBag.Menus = GetAvailableMenus();
            return View();
        }

        [HttpPost]
        public IActionResult Create(SalesProgram salesProgram)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO sales_programs (nama_program, diskon, tanggal_berlaku, jam_berlaku, menu_id, kategori_produk) " +
                            "VALUES (@nama_program, @diskon, @tanggal_berlaku, @jam_berlaku, @menu_id, @kategori_produk)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama_program", salesProgram.NamaProgram);
                    command.Parameters.AddWithValue("@diskon", salesProgram.Diskon);
                    command.Parameters.AddWithValue("@tanggal_berlaku", salesProgram.TanggalBerlaku);
                    command.Parameters.AddWithValue("@jam_berlaku", salesProgram.JamBerlaku);
                    command.Parameters.AddWithValue("@menu_id", salesProgram.MenuId.HasValue ? salesProgram.MenuId : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@kategori_produk", !string.IsNullOrEmpty(salesProgram.KategoriProduk) ? salesProgram.KategoriProduk : (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Show Edit Sales Program Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();

            SalesProgram salesProgram = null;

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM sales_programs WHERE program_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            salesProgram = new SalesProgram
                            {
                                SalesProgramId = Convert.ToUInt64(reader["program_id"]),
                                NamaProgram = reader["nama_program"].ToString(),
                                Diskon = Convert.ToDecimal(reader["diskon"]),
                                TanggalBerlaku = Convert.ToDateTime(reader["tanggal_berlaku"]),
                                JamBerlaku = TimeSpan.Parse(reader["jam_berlaku"].ToString()),
                                MenuId = reader["menu_id"] != DBNull.Value ? Convert.ToUInt64(reader["menu_id"]) : (ulong?)null,
                                KategoriProduk = reader["kategori_produk"] != DBNull.Value ? reader["kategori_produk"].ToString() : null
                            };
                        }
                    }
                }
            }

            ViewBag.Menus = GetAvailableMenus();

            return View(salesProgram);
        }

        [HttpPost]
        public IActionResult Edit(SalesProgram salesProgram)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE sales_programs SET nama_program = @nama_program, diskon = @diskon, tanggal_berlaku = @tanggal_berlaku, " +
                            "jam_berlaku = @jam_berlaku, menu_id = @menu_id, kategori_produk = @kategori_produk WHERE program_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", salesProgram.SalesProgramId);
                    command.Parameters.AddWithValue("@nama_program", salesProgram.NamaProgram);
                    command.Parameters.AddWithValue("@diskon", salesProgram.Diskon);
                    command.Parameters.AddWithValue("@tanggal_berlaku", salesProgram.TanggalBerlaku);
                    command.Parameters.AddWithValue("@jam_berlaku", salesProgram.JamBerlaku);
                    command.Parameters.AddWithValue("@menu_id", salesProgram.MenuId.HasValue ? salesProgram.MenuId : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@kategori_produk", !string.IsNullOrEmpty(salesProgram.KategoriProduk) ? salesProgram.KategoriProduk : (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Handle Delete Sales Program
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM sales_programs WHERE program_id = @id";
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
