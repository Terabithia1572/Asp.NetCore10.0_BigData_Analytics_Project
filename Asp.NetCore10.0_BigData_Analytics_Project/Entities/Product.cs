namespace Asp.NetCore10._0_BigData_Analytics_Project.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public string CountryOfOrigin { get; set; }
        public string ProductImageURL { get; set; }
    }
}
