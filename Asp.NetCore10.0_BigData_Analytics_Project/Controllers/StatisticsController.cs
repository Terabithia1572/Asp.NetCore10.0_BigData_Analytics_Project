using Asp.NetCore10._0_BigData_Analytics_Project.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.NetCore10._0_BigData_Analytics_Project.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly BigDataOrdersDBContext _context;

        public StatisticsController(BigDataOrdersDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.CategoryCount = _context.Categories.Count(); // Toplam kategori sayısı 
            ViewBag.CustomerCount = _context.Customers.Count(); // Toplam müşteri sayısı
            ViewBag.ProductCount = _context.Products.Count(); // Toplam ürün sayısı
            ViewBag.OrderCount = _context.Orders.Count(); // Toplam sipariş sayısı

            ViewBag.CustomerCount = _context.Customers.Select(x => x.CustomerCountry).Distinct().Count(); // Müşteriye ait Farklı ülke sayısı
            ViewBag.CustomerCity = _context.Customers.Select(x => x.CustomerCity).Distinct().Count(); // Müşteriye ait Farklı şehir sayısı
            ViewBag.OrderStatusByCompleted = _context.Orders.Where(x => x.OrderStatus == "Teslim Edildi").Count();
            ViewBag.OrderStatusByCancelled = _context.Orders.Where(x => x.OrderStatus == "Kargoda").Count();

            ViewBag.OctoberOrders = _context.Orders
   .FromSqlRaw("SELECT * FROM Orders WHERE OrderDate >= '2025-10-01' AND OrderDate < '2025-11-01'")
   .Count();

            ViewBag.Orders2025Count = _context.Orders.Where(x => x.OrderDate.Year == 2025).Count();

            ViewBag.AverageProductPrice = _context.Products.Average(x => x.UnitPrice);
            ViewBag.AverageProductQuantity = _context.Products.Average(x => x.StockQuantity);


            return View();
        }
    }
}
