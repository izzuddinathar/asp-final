namespace Final.Models
{
    public class SalesReport
    {
        public ulong ReportId { get; set; } // Primary Key
        public DateTime Tanggal { get; set; } // Date of Report
        public ulong MenuId { get; set; } // Foreign Key (Menu ID)
        public decimal Harga { get; set; } // Price of the Item
        public decimal Total { get; set; } // Total Sales Amount
    }
}
