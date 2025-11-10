namespace Asp.NetCore10._0_BigData_Analytics_Project.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? CategoryDescription { get; set; }

        public string? CategoryImageUrl { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
