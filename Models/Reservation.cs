namespace Final.Models
{
    public class Reservation
    {
        public ulong ReservasiId { get; set; } // Primary Key
        public string NamaPelanggan { get; set; } // Customer Name
        public string NomorKontak { get; set; } // Contact Number
        public DateTime WaktuReservasi { get; set; } // Reservation Date and Time
        public int NomorMeja { get; set; } // Foreign Key (Table Number)
        public string Status { get; set; } // Enum: terjadwal, dibatalkan, selesai
    }
}
