namespace Final.Models
{
    public class Order
    {
        public ulong OrderId { get; set; } // Primary Key
        public int NomorMeja { get; set; } // Foreign Key (Table Number)
        public ulong MenuId { get; set; } // Foreign Key (Menu ID)
        public int Jumlah { get; set; } // Quantity
        public string StatusPesanan { get; set; } // Enum: dipesan, diproses, disajikan
    }
}
