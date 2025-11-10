using Asp.NetCore10._0_BigData_Analytics_Project.Entities;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Context
{
    public class BigDataOrdersDBContext : DbContext
    {
        public BigDataOrdersDBContext(DbContextOptions<BigDataOrdersDBContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

    }
}
