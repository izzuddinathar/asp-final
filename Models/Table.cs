namespace Final.Models
{
    public class Table
    {
        public ulong TableId { get; set; } // Primary Key
        public int NomorMeja { get; set; } // Unique Table Number
        public int Kapasitas { get; set; } // Capacity
        public string Status { get; set; } // Enum: dipesan, tersedia, terisi
    }
}
