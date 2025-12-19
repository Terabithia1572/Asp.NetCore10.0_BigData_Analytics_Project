namespace Asp.NetCore10._0_BigData_Analytics_Project.Entities
{
    public class Message
    {
        public int MessageID { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public string MessageSubject { get; set; }
        public string MessageText { get; set; }
        public string SentimentLabel { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
