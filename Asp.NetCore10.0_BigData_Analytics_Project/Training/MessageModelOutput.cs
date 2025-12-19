using Microsoft.ML.Data;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Training
{
    public class MessageModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; } = "";
        public float[] Score { get; set; }
    }
}
