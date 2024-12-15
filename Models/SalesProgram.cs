namespace Final.Models
{
    public class SalesProgram
    {
        public ulong SalesProgramId { get; set; } // Primary Key
        public string NamaProgram { get; set; } // Program Name
        public decimal Diskon { get; set; } // Discount
        public DateTime TanggalBerlaku { get; set; } // Applicable Date
        public TimeSpan JamBerlaku { get; set; } // Applicable Time
        public ulong? MenuId { get; set; } // Foreign Key (Optional)
        public string KategoriProduk { get; set; } // Enum: cemilan, makanan, minuman (Optional)
    }
}
