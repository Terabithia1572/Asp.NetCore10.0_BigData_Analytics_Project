namespace Asp.NetCore10._0_BigData_Analytics_Project.DTOs.LoyaltyDTOs
{
    public class LoyaltyScoreDTO
    {
        public string CustomerName { get; set; } // Müşteri Adı
        public int TotalOrders { get; set; } // Toplam Sipariş Sayısı
        public double TotalSpent { get; set; } // Toplam Harcama
        public DateTime? LastOrderDate { get; set; } // Son Sipariş Tarihi
        public double LoyaltyScore { get; set; } // Sadakat Skoru
    }
}
