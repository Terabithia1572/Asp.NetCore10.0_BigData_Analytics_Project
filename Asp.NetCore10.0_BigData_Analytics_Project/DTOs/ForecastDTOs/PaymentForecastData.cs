namespace Asp.NetCore10._0_BigData_Analytics_Project.DTOs.ForecastDTOs
{
    public class PaymentForecastData
    {
        public string PaymentMethod { get; set; } // Kredi Kartı, PayPal, Banka Transferi vb.
        public float MonthIndex { get; set; } // Ay indeksi (1-12)
        public float OrderCount { get; set; } // O ay içindeki sipariş sayısı
    }
}
