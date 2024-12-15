namespace Final.Models
{
    public class Payment
    {
        public ulong PaymentId { get; set; } // Primary Key
        public int NomorMeja { get; set; } // Foreign Key (Table Number)
        public ulong MenuId { get; set; } // Foreign Key (Menu ID)
        public int Jumlah { get; set; } // Quantity
        public string MetodePembayaran { get; set; } // Enum: tunai, kartu kredit, kartu debit, qris
        public string Status { get; set; } // Enum: belum dibayar, lunas
    }
}
