using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.DashboardViewComponents
{
    public class _DashboardTodayOrdersComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;

        public _DashboardTodayOrdersComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var last10Orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Where(o => o.OrderDate >= today && o.OrderDate < tomorrow)
                .OrderByDescending(o => o.OrderDate)
                .Take(10)
                .Select(o => new
                {
                    o.OrderID,
                    o.Product.ProductName,
                    CustomerName = o.Customer.CustomerName + " " + o.Customer.CustomerSurname,
                    o.Quantity,
                    o.PaymentMethod,
                    o.OrderStatus,
                    o.OrderDate
                })
                .ToList();
            return View(last10Orders);
        }
    }
}
