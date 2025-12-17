namespace Asp.NetCore10._0_BigData_Analytics_Project.DTOs.LoyaltyMLDTOs
{
    public class ResultLoyaltyScoreMLDTO
    {
        public string CustomerName { get; set; }
        public double Recency { get; set; }
        public double Frequency { get; set; }
        public double Monetary { get; set; }
        public double ActualLoyaltyScore { get; set; }
        public double PredictedLoyaltyScore { get; set; }
    }
}
