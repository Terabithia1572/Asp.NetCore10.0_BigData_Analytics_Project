using Microsoft.ML.Data;

namespace Asp.NetCore10._0_BigData_Analytics_Project.DTOs.LoyaltyMLDTOs
{
    public class LoyaltyScoreMLPredictionDTO
    {
        [ColumnName("Score")]
        public float LoyaltyScore { get; set; }
    }
}
