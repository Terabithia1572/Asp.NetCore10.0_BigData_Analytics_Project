using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsStatisticsComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _CustomerAnalyticsStatisticsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.TopCustomer = _context.Orders.GroupBy(o => new { o.Customer.CustomerName, o.Customer.CustomerSurname })
             .Select(g => new
             {
                 FullName = g.Key.CustomerName + " " + g.Key.CustomerSurname,
                 TotalOrders = g.Count()
             }).OrderByDescending(x => x.TotalOrders).Select(x => x.FullName).FirstOrDefault();

            ViewBag.TopCity = _context.Orders.GroupBy(o => o.Customer.CustomerCity).Select(g => new
            {
                City = g.Key,
                TotalOrders = g.Count()
            }).OrderByDescending(x => x.TotalOrders).Select(x => x.City).FirstOrDefault();

            ViewBag.Last30DaysOrderCount = _context.Orders.Where(o => o.OrderDate >= DateTime.Now.AddDays(-30)).Select(o => o.CustomerID).Distinct().Count();

            ViewBag.TopPaymentMethod = _context.Orders.GroupBy(o => o.PaymentMethod).Select(g => new
            {
                PaymentMethod = g.Key,
                TotalOrders = g.Count(),
            }).OrderByDescending(x => x.TotalOrders).Select(y => y.PaymentMethod).FirstOrDefault();

            return View();
        }
    }
}
