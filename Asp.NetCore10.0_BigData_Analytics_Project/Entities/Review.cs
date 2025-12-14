namespace Asp.NetCore10._0_BigData_Analytics_Project.Entities
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public string PurchaseType { get; set; }
        public byte Rating { get; set; }
        public string Sentiment { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }

    }
}
