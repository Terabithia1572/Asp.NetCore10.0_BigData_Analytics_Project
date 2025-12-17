using Asp.NetCore10._0_BigData_Analytics_Project.Context;
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
            var loyaltyScores=_context.Customers // Müşteriler
                .Include(c=>c.Orders) // Siparişler
                .ThenInclude(o=>o.Product) // Ürünler
                .Where(c=>c.CustomerCountry=="İtalya") // İtalya'daki müşteriler
                .Select(c=> new // Sadakat Skoru Hesaplama
                {
                    CustoemrName=c.CustomerName+" "+c.CustomerSurname, // Müşteri Adı
                    TotalOrders =c.Orders.Count(), // Toplam Sipariş Sayısı
                    TotalSpent =c.Orders.Sum(o=>o.Quantity* o.Product.UnitPrice), // Toplam Harcama
                    LastOrderDate =c.Orders.Max(o=>(DateTime?)o.OrderDate), // Son Sipariş Tarihi
                })
                .AsEnumerable() // Bellek içi işleme için
                .Select(x => // Sadakat Skoru Hesaplama
                {
                    var daySinceLastOrder = x.LastOrderDate.HasValue  // Son Sipariş Tarihinden Bu Yana Geçen Gün Sayısı
                    ? (DateTime.Now - x.LastOrderDate.Value).TotalDays  // Eğer sipariş varsa
                    : double.MaxValue; // Eğer sipariş yoksa maksimum değer


                })
            return View();
        }
    }
}
