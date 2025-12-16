using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsMainStatisticsComponentPartial : ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;

        public _CustomerAnalyticsMainStatisticsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var totalCustomerCount = _context.Customers.Count();

            ViewBag.TotalCustomerCount = totalCustomerCount;

            var totalOrderCount = _context.Orders.Count();

            var averageOrderPerCustomerCount = totalOrderCount / totalCustomerCount;

            ViewBag.AverageOrderPerCustomerCount = averageOrderPerCustomerCount;

            var threeMonthsAgo = DateTime.Now.AddMonths(-3);

            var activeCustomerCount = _context.Orders.Where(o => o.OrderDate >= threeMonthsAgo).Select(o => o.CustomerID).Distinct().Count();

            ViewBag.ActiveCustomerCount = activeCustomerCount;

            var sixMonthsAgo = DateTime.Now.AddMonths(-6);

            var inactiveCustomerCount = _context.Customers.Count(c => !_context.Orders.Any(o => o.CustomerID == c.CustomerID && o.OrderDate >= sixMonthsAgo));

            ViewBag.InactiveCustomerCount = inactiveCustomerCount;

            return View();
        }
    }
}
