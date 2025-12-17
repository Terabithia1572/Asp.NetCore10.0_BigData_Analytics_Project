namespace Asp.NetCore10._0_BigData_Analytics_Project.DTOs.LoyaltyMLDTOs
{
    public class LoyaltyScoreMLDataDTO
    {
        public string CustomerName { get; set; }
        public float Recency { get; set; }
        public float Frequency { get; set; }
        public float Monetary { get; set; }
        public float LoyaltyScore { get; set; }
    }
}
