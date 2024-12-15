using Microsoft.AspNetCore.Mvc;
using Final.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace Final.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly string connectionString;

        public ReservationController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // List All Reservations
        public IActionResult Index()
        {
            SetSharedViewBag();
            var reservations = new List<Reservation>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM reservations";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new Reservation
                        {
                            ReservasiId = Convert.ToUInt64(reader["reservasi_id"]),
                            NamaPelanggan = reader["nama_pelanggan"].ToString(),
                            NomorKontak = reader["nomor_kontak"].ToString(),
                            WaktuReservasi = Convert.ToDateTime(reader["waktu_reservasi"]),
                            NomorMeja = Convert.ToInt32(reader["nomor_meja"]),
                            Status = reader["status"].ToString()
                        });
                    }
                }
            }

            return View(reservations);
        }

        // Show Add Reservation Form
        public IActionResult Create()
        {
            SetSharedViewBag();

            // Fetch available tables for the dropdown
            var tables = new List<int>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT DISTINCT nomor_meja FROM tables WHERE status = 'tersedia'";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(Convert.ToInt32(reader["nomor_meja"]));
                    }
                }
            }
            ViewBag.Tables = tables;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO reservations (nama_pelanggan, nomor_kontak, waktu_reservasi, nomor_meja, status) " +
                            "VALUES (@nama_pelanggan, @nomor_kontak, @waktu_reservasi, @nomor_meja, @status)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nama_pelanggan", reservation.NamaPelanggan);
                    command.Parameters.AddWithValue("@nomor_kontak", reservation.NomorKontak);
                    command.Parameters.AddWithValue("@waktu_reservasi", reservation.WaktuReservasi);
                    command.Parameters.AddWithValue("@nomor_meja", reservation.NomorMeja);
                    command.Parameters.AddWithValue("@status", reservation.Status);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // Show Edit Reservation Form
        public IActionResult Edit(int id)
        {
            SetSharedViewBag();

            Reservation reservation = null;
            var tables = new List<int>();

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch reservation data
                var query = "SELECT * FROM reservations WHERE reservasi_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reservation = new Reservation
                            {
                                ReservasiId = Convert.ToUInt64(reader["reservasi_id"]),
                                NamaPelanggan = reader["nama_pelanggan"].ToString(),
                                NomorKontak = reader["nomor_kontak"].ToString(),
                                WaktuReservasi = Convert.ToDateTime(reader["waktu_reservasi"]),
                                NomorMeja = Convert.ToInt32(reader["nomor_meja"]),
                                Status = reader["status"].ToString()
                            };
                        }
                    }
                }

                // Fetch available tables
                var tableQuery = "SELECT DISTINCT nomor_meja FROM tables";
                using (var command = new MySqlCommand(tableQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(Convert.ToInt32(reader["nomor_meja"]));
                    }
                }
            }
            ViewBag.Tables = tables;

            return View(reservation);
        }

        [HttpPost]
        public IActionResult Edit(Reservation reservation)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "UPDATE reservations SET nama_pelanggan = @nama_pelanggan, nomor_kontak = @nomor_kontak, " +
                            "waktu_reservasi = @waktu_reservasi, nomor_meja = @nomor_meja, status = @status " +
                            "WHERE reservasi_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", reservation.ReservasiId);
                    command.Parameters.AddWithValue("@nama_pelanggan", reservation.NamaPelanggan);
                    command.Parameters.AddWithValue("@nomor_kontak", reservation.NomorKontak);
                    command.Parameters.AddWithValue("@waktu_reservasi", reservation.WaktuReservasi);
                    command.Parameters.AddWithValue("@nomor_meja", reservation.NomorMeja);
                    command.Parameters.AddWithValue("@status", reservation.Status);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM reservations WHERE reservasi_id = @id";
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
