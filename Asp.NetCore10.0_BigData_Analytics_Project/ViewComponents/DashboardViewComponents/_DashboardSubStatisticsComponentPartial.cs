using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.DashboardViewComponents
{
    public class _DashboardSubStatisticsComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _DashboardSubStatisticsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.CategoryCount = _context.Categories.Count();
            ViewBag.ProductCount = _context.Products.Count();
            ViewBag.CustomerCount = _context.Customers.Count();
            ViewBag.OrderCount = _context.Orders.Count();
            ViewBag.CountryCount = _context.Customers.Select(x => x.CustomerCountry).Distinct().Count();
            ViewBag.CityCount = _context.Customers.Select(x => x.CustomerCity).Distinct().Count();

            return View();
        }
    }
}
