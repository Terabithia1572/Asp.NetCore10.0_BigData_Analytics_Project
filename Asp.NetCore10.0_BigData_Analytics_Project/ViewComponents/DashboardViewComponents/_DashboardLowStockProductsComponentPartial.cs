using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.DashboardViewComponents
{
    public class _DashboardLowStockProductsComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _DashboardLowStockProductsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var values = _context.Products.Include(x => x.Category).Where(y => y.StockQuantity <= 9).Take(15).ToList();
            return View(values);
        }
    }
}
