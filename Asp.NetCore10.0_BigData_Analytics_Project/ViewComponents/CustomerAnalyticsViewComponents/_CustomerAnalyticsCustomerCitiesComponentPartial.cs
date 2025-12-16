using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.CustomerAnalyticsViewComponents
{
    public class _CustomerAnalyticsCustomerCitiesComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _CustomerAnalyticsCustomerCitiesComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var topCities = _context.Orders
                   .Include(o => o.Customer)
                   .Where(o => o.Customer.CustomerCity != null)
                   .GroupBy(o => o.Customer.CustomerCity)
                   .Select(g => new
                   {
                       CityName = g.Key,
                       OrderCount = g.Count()
                   })
                   .OrderByDescending(x => x.OrderCount)
                   .Take(7)
                   .ToList();

            // Chart.js'e göndermek için ViewBag veya ViewModel kullanılabilir
            ViewBag.CityLabels = topCities.Select(x => x.CityName).ToList();
            ViewBag.CityValues = topCities.Select(x => x.OrderCount).ToList();

            return View();
        }
    }
}
