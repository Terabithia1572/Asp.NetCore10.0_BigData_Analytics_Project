using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.DTOs.LoyaltyDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class CustomerLoyaltyController : Controller
    {
        private readonly BigDataOrdersDBContext _context;

        public CustomerLoyaltyController(BigDataOrdersDBContext context)
        {
            _context = context;
        }

        public IActionResult ItalyLoyaltyScore()
        {
            var loyaltScores = _context.Customers
                 .Include(c => c.Orders)
                 .ThenInclude(o => o.Product)
                 .Where(c => c.CustomerCountry == "İtalya" && c.CustomerCity == "Parma" || c.CustomerCity == "Bologna" || c.CustomerCity == "Como" || c.CustomerCity == "Siena" || c.CustomerCity == "Verona" || c.CustomerCity == "Bergamo" || c.CustomerCity == "Bari" || c.CustomerCity == "Venedik")
                 .Select(c => new
                 {
                     CustomerName = c.CustomerName + " " + c.CustomerSurname,
                     TotalOrders = c.Orders.Count(),
                     TotalSpent = c.Orders.Sum(o => o.Quantity * o.Product.UnitPrice),
                     LastOrderDate = c.Orders.Max(o => (DateTime?)o.OrderDate)
                 })
                 .AsEnumerable()
                 .Select(x =>
                 {
                     var daySinceLastOrder = (x.LastOrderDate.HasValue)
                         ? (DateTime.Now - x.LastOrderDate.Value).TotalDays
                         : double.MaxValue;

                     double recenyScore = daySinceLastOrder switch
                     {
                         <= 30 => 100,
                         <= 90 => 75,
                         <= 180 => 50,
                         <= 365 => 25,
                         _ => 10
                     };

                     double frequencyScore = x.TotalOrders switch
                     {
                         >= 20 => 100,
                         >= 10 => 80,
                         >= 5 => 60,
                         >= 2 => 40,
                         1 => 20,
                         _ => 10
                     };

                     double monetaryScore = x.TotalSpent switch
                     {
                         >= 5000 => 100,
                         >= 3000 => 80,
                         >= 1000 => 60,
                         >= 500 => 40,
                         >= 100 => 20,
                         _ => 10
                     };

                     double loyaltyScore = (recenyScore * 0.4) + (frequencyScore * 0.3) + (monetaryScore * 0.3);

                     return new LoyaltyScoreDTO
                     {
                         CustomerName = x.CustomerName,
                         TotalOrders = x.TotalOrders,
                         TotalSpent = Math.Round(x.TotalSpent, 2),
                         LastOrderDate = x.LastOrderDate,
                         LoyaltyScore = Math.Round(loyaltyScore, 2)
                     };
                 }).OrderByDescending(x => x.LoyaltyScore).ToList();
            return View(loyaltScores);
        }
    }
}
