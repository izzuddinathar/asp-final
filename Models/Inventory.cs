namespace Final.Models
{
    public class Inventory
    {
        public ulong InventoryId { get; set; } // Primary Key
        public string NamaBahanPokok { get; set; } // Name of Raw Material
        public int Jumlah { get; set; } // Quantity
        public string Satuan { get; set; } // Unit (e.g., kg, liter, pcs)
        public string Supplier { get; set; } // Supplier
    }
}
