using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class ForecastController : Controller
    {
        private readonly BigDataOrdersDBContext _context;
        private readonly MLContext _mlContext;

        public ForecastController(BigDataOrdersDBContext context, MLContext mlContext)
        {
            _context = context;
            _mlContext = mlContext;
        }

        public IActionResult PaymentMethodForecast()
        {
            //2025 Yılı Verilerinin Çekilmesi
            var startDate= new DateTime(2025, 1, 1); //2025 yılının başı
            var endDate= new DateTime(2025, 12, 31); //2025 yılının sonu

            var monthlyPaymentData = _context.Orders
               .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
               .AsEnumerable()
               .GroupBy(o => new
               {
                   Month = new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1),
                   o.PaymentMethod
               })
               .Select(g => new
               {
                   Month = g.Key.Month,
                   PaymentMethod = g.Key.PaymentMethod,
                   OrderCount = g.Count()
               })
               .OrderBy(x => x.Month)
               .ToList();

            return View();
        }
    }
}
