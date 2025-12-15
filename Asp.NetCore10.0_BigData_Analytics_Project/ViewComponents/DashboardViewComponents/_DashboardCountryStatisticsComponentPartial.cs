using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Asp.NetCore10._0_BigData_Analytics_Project.DTOs.ChartDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.ViewComponents.DashboardViewComponents
{
    public class _DashboardCountryStatisticsComponentPartial:ViewComponent
    {
        private readonly BigDataOrdersDBContext _context;
        public _DashboardCountryStatisticsComponentPartial(BigDataOrdersDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            // Customer tablosuna join'li sorgu
            var data = _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.Customer.CustomerCountry != null)
                .GroupBy(o => o.Customer.CustomerCountry)
                .Select(g => new CountryOrderDTO
                {
                    Country = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(5)
                .ToList();

            // Toplam sipariş sayısı
            var totalOrders = data.Sum(x => x.OrderCount);

            // Yüzde hesapla
            foreach (var item in data)
            {
                item.Percentage = Math.Round((double)item.OrderCount / totalOrders * 100, 1);
            }

            return View(data);
        }
    }
}
