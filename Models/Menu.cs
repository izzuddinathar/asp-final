namespace Final.Models
{
    public class Menu
    {
        public ulong MenuId { get; set; } // Primary Key
        public string NamaMenu { get; set; } // Menu Name
        public string Deskripsi { get; set; } // Description
        public decimal Harga { get; set; } // Price
        public string Kategori { get; set; } // Enum: cemilan, makanan, minuman
        public string Gambar { get; set; } // Image Path
    }
}
